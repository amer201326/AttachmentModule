using MyCompanyName.AbpZeroTemplate.Attachments;
using System.Collections.Generic;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using MyCompanyName.AbpZeroTemplate.Attachments.Exporting;
using MyCompanyName.AbpZeroTemplate.Attachments.Dtos;
using MyCompanyName.AbpZeroTemplate.Dto;
using Abp.Application.Services.Dto;
using MyCompanyName.AbpZeroTemplate.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using MyCompanyName.AbpZeroTemplate.Test.Dtos;
using MyCompanyName.AbpZeroTemplate.Storage;

namespace MyCompanyName.AbpZeroTemplate.Attachments
{
    [AbpAuthorize(AppPermissions.Pages_AttachmentFiles)]
    public class AttachmentFilesAppService : AbpZeroTemplateAppServiceBase, IAttachmentFilesAppService
    {
        private readonly IRepository<AttachmentFile, long> _attachmentFileRepository;
        private readonly IAttachmentFilesExcelExporter _attachmentFilesExcelExporter;
        private readonly IRepository<AttachmentType, int> _lookup_attachmentTypeRepository;
        private readonly IRepository<AttachmentEntityType, int> _AttachmentEntityTypeRepository;
        private readonly ITempFileCacheManager _tempFileCacheManager;


        public AttachmentFilesAppService(ITempFileCacheManager tempFileCacheManager, IRepository<AttachmentEntityType, int> AttachmentEntityTypeRepository, IRepository<AttachmentFile, long> attachmentFileRepository, IAttachmentFilesExcelExporter attachmentFilesExcelExporter, IRepository<AttachmentType, int> lookup_attachmentTypeRepository)
        {
            _attachmentFileRepository = attachmentFileRepository;
            _attachmentFilesExcelExporter = attachmentFilesExcelExporter;
            _lookup_attachmentTypeRepository = lookup_attachmentTypeRepository;
            _AttachmentEntityTypeRepository = AttachmentEntityTypeRepository;
            _tempFileCacheManager = tempFileCacheManager;

        }

        public async Task<PagedResultDto<GetAttachmentFileForViewDto>> GetAll(GetAllAttachmentFilesInput input)
        {

            var filteredAttachmentFiles = _attachmentFileRepository.GetAll()
                        .Include(e => e.AttachmentTypeFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.PhysicalName.Contains(input.Filter) || e.OriginalName.Contains(input.Filter) || e.ObjectId.Contains(input.Filter) || e.Path.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PhysicalNameFilter), e => e.PhysicalName == input.PhysicalNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OriginalNameFilter), e => e.OriginalName == input.OriginalNameFilter)
                        .WhereIf(input.MinSizeFilter != null, e => e.Size >= input.MinSizeFilter)
                        .WhereIf(input.MaxSizeFilter != null, e => e.Size <= input.MaxSizeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ObjectIdFilter), e => e.ObjectId == input.ObjectIdFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PathFilter), e => e.Path == input.PathFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AttachmentTypeArNameFilter), e => e.AttachmentTypeFk != null && e.AttachmentTypeFk.ArName == input.AttachmentTypeArNameFilter);

            var pagedAndFilteredAttachmentFiles = filteredAttachmentFiles
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var attachmentFiles = from o in pagedAndFilteredAttachmentFiles
                                  join o1 in _lookup_attachmentTypeRepository.GetAll() on o.AttachmentTypeId equals o1.Id into j1
                                  from s1 in j1.DefaultIfEmpty()

                                  select new GetAttachmentFileForViewDto()
                                  {
                                      AttachmentFile = new AttachmentFileDto
                                      {
                                          PhysicalName = o.PhysicalName,
                                          OriginalName = o.OriginalName,
                                          Size = o.Size,
                                          ObjectId = o.ObjectId,
                                          Path = o.Path,
                                          Id = o.Id
                                      },
                                      AttachmentTypeArName = s1 == null || s1.ArName == null ? "" : s1.ArName.ToString()
                                  };

            var totalCount = await filteredAttachmentFiles.CountAsync();

            return new PagedResultDto<GetAttachmentFileForViewDto>(
                totalCount,
                await attachmentFiles.ToListAsync()
            );
        }

        public async Task<GetAttachmentFileForViewDto> GetAttachmentFileForView(long id)
        {
            var attachmentFile = await _attachmentFileRepository.GetAsync(id);

            var output = new GetAttachmentFileForViewDto { AttachmentFile = ObjectMapper.Map<AttachmentFileDto>(attachmentFile) };

            if (output.AttachmentFile.AttachmentTypeId != null)
            {
                var _lookupAttachmentType = await _lookup_attachmentTypeRepository.FirstOrDefaultAsync((int)output.AttachmentFile.AttachmentTypeId);
                output.AttachmentTypeArName = _lookupAttachmentType?.ArName?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_AttachmentFiles_Edit)]
        public async Task<GetAttachmentFileForEditOutput> GetAttachmentFileForEdit(EntityDto<long> input)
        {
            var attachmentFile = await _attachmentFileRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetAttachmentFileForEditOutput { AttachmentFile = ObjectMapper.Map<CreateOrEditAttachmentFileDto>(attachmentFile) };

            if (output.AttachmentFile.AttachmentTypeId != null)
            {
                var _lookupAttachmentType = await _lookup_attachmentTypeRepository.FirstOrDefaultAsync((int)output.AttachmentFile.AttachmentTypeId);
                output.AttachmentTypeArName = _lookupAttachmentType?.ArName?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditAttachmentFileDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_AttachmentFiles_Create)]
        protected virtual async Task Create(CreateOrEditAttachmentFileDto input)
        {
            var attachmentFile = ObjectMapper.Map<AttachmentFile>(input);


            if (AbpSession.TenantId != null)
            {
                attachmentFile.TenantId = (int?)AbpSession.TenantId;
            }


            await _attachmentFileRepository.InsertAsync(attachmentFile);
        }

        [AbpAuthorize(AppPermissions.Pages_AttachmentFiles_Edit)]
        protected virtual async Task Update(CreateOrEditAttachmentFileDto input)
        {
            var attachmentFile = await _attachmentFileRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, attachmentFile);
        }

        [AbpAuthorize(AppPermissions.Pages_AttachmentFiles_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _attachmentFileRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetAttachmentFilesToExcel(GetAllAttachmentFilesForExcelInput input)
        {

            var filteredAttachmentFiles = _attachmentFileRepository.GetAll()
                        .Include(e => e.AttachmentTypeFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.PhysicalName.Contains(input.Filter) || e.OriginalName.Contains(input.Filter) || e.ObjectId.Contains(input.Filter) || e.Path.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PhysicalNameFilter), e => e.PhysicalName == input.PhysicalNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OriginalNameFilter), e => e.OriginalName == input.OriginalNameFilter)
                        .WhereIf(input.MinSizeFilter != null, e => e.Size >= input.MinSizeFilter)
                        .WhereIf(input.MaxSizeFilter != null, e => e.Size <= input.MaxSizeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ObjectIdFilter), e => e.ObjectId == input.ObjectIdFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PathFilter), e => e.Path == input.PathFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AttachmentTypeArNameFilter), e => e.AttachmentTypeFk != null && e.AttachmentTypeFk.ArName == input.AttachmentTypeArNameFilter);

            var query = (from o in filteredAttachmentFiles
                         join o1 in _lookup_attachmentTypeRepository.GetAll() on o.AttachmentTypeId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         select new GetAttachmentFileForViewDto()
                         {
                             AttachmentFile = new AttachmentFileDto
                             {
                                 PhysicalName = o.PhysicalName,
                                 OriginalName = o.OriginalName,
                                 Size = o.Size,
                                 ObjectId = o.ObjectId,
                                 Path = o.Path,
                                 Id = o.Id
                             },
                             AttachmentTypeArName = s1 == null || s1.ArName == null ? "" : s1.ArName.ToString()
                         });


            var attachmentFileListDtos = await query.ToListAsync();

            return _attachmentFilesExcelExporter.ExportToFile(attachmentFileListDtos);
        }


        [AbpAuthorize(AppPermissions.Pages_AttachmentFiles)]
        public async Task<List<AttachmentFileAttachmentTypeLookupTableDto>> GetAllAttachmentTypeForTableDropdown()
        {
            return await _lookup_attachmentTypeRepository.GetAll()
                .Select(attachmentType => new AttachmentFileAttachmentTypeLookupTableDto
                {
                    Id = attachmentType.Id,
                    DisplayName = attachmentType == null || attachmentType.ArName == null ? "" : attachmentType.ArName.ToString()
                }).ToListAsync();
        }
        [NonAction]
        public async Task<List<UploadFilesInput>> GetAttacments(string ObjectID, int AttachmentEntityTypeId)
        {
            var result = new List<UploadFilesInput>();
            //var lastAttachment = await _attachmentFileRepository.GetAll().Where(e => e.ObjectId == ObjectID && e.AttachmentTypeId == AttachmentEntityTypeId).OrderByDescending(e => e.Version).FirstOrDefaultAsync();
            //if (lastAttachment != null)
            //{
                var Attachments = await _attachmentFileRepository.GetAll().Where(e => e.ObjectId == ObjectID && e.AttachmentTypeId == AttachmentEntityTypeId /*&& e.Version == lastAttachment.Version*/).ToListAsync();
                foreach (var item in Attachments)
                {
                    result.Add(new UploadFilesInput() { FileName = item.OriginalName, FileToken = item.FileToken });
                }
            //}
            return result;
        }
        [NonAction]
        public async Task<AttachmentMemoryStream> DownloadAttaachment(string FileToken)
        {
           var attachment = await _attachmentFileRepository.GetAll().Where(e => e.FileToken == FileToken).FirstOrDefaultAsync();
            if(attachment != null)
            {
                var memory = new MemoryStream();
                var path = $"{attachment.Path}";

                using (var stream = new FileStream(path, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;
                return new AttachmentMemoryStream() { ApplicationType = "application/octet-stream" , FileName= attachment.OriginalName , Memory = memory };

            }
            else
            {
                throw new UserFriendlyException(L("AttachmentNotFoundCode"), L("AttachmentNotFoundCode_Detail"));
            }
        }


            [NonAction]
        public async Task CheckAttachment<T>(int AttachmentTypeId, string ObjectID, AttachmentsViewModelWithEntityDto<T> input, AttachmentUploadType type)
        {
            var attachmentType = await _lookup_attachmentTypeRepository.GetAsync(AttachmentTypeId);
            if (attachmentType == null)
            {
                throw new UserFriendlyException(L("InvalidAttachmentTypeIdCode"), L("InvalidAttachmentTypeIdCode_Detail"));
            }

            if ((input.Attachments != null && input.Attachments.Count > 0) || (input.AttachmentsToDelete !=null && input.AttachmentsToDelete.Count > 0))
            {
                var version = 1;
                var numOfAttached = input.Attachments.Count;

                if (type == AttachmentUploadType.edit)
                {
                    var oldAttachments = await _attachmentFileRepository.GetAll().Where(e => e.ObjectId == ObjectID && e.AttachmentTypeId == AttachmentTypeId).OrderByDescending(e => e.Version).ToListAsync();
                    var anyAttachment = oldAttachments.FirstOrDefault();

                    if (anyAttachment != null)
                    {
                        var lastGroup = oldAttachments.Where(e => e.Version == anyAttachment.Version).ToList();

                        var tokensToDelete = new Dictionary<string, string>();
                        foreach (var item in input.AttachmentsToDelete)
                        {
                            if (!tokensToDelete.ContainsKey(item.FileToken))
                            {
                                tokensToDelete.Add(item.FileToken, item.FileToken);
                            }
                        }
                        version = anyAttachment.Version ;

                        var notDeletedAttaches = lastGroup.Where(e => !tokensToDelete.ContainsKey(e.FileToken)).ToList();
                        var DeletedAttaches = lastGroup.Where(e => tokensToDelete.ContainsKey(e.FileToken)).ToList();
                        if(notDeletedAttaches.Count != lastGroup.Count)
                        {
                            version = anyAttachment.Version + 1;

                            foreach (var att in notDeletedAttaches)
                            {
                                att.Version = version;
                            }
                        }
                        foreach (var item in DeletedAttaches)
                        {
                            await _attachmentFileRepository.DeleteAsync(item);
                        }
                        numOfAttached += notDeletedAttaches.Count;
                    }
                }
                
                if(numOfAttached > attachmentType.MaxAttachments)
                {
                    throw new UserFriendlyException(L("MaxAttachmentsCode"), L("MaxAttachmentsCode_Detail"));

                }
                var listAttachments = new List<UploadFilesInputDto>();
                foreach (var att in input.Attachments)
                {
                    var FileByte = _tempFileCacheManager.GetFile(att.FileToken);
                    if (FileByte == null)
                    {
                        throw new UserFriendlyException("There is no such image file with the token: " + att.FileToken);
                    }
                    CheckFileType(att.FileName, attachmentType);
                    if(attachmentType.MaxSize != 0 && FileByte.Length > attachmentType.MaxSize * 1000)
                    {
                        throw new UserFriendlyException(L("FileSizeNotAllowdCode"), L("FileSizeNotAllowdCode_Detail"));
                    }
                    var UploadFilesInputDto = ObjectMapper.Map<UploadFilesInputDto>(att);
                    UploadFilesInputDto.FileByte = FileByte;
                    UploadFilesInputDto.Version = version;
                    listAttachments.Add(UploadFilesInputDto);
                }
                await AddAttachment(
                    attachmentType, ObjectID ,
                    listAttachments
                    );
            }
            else
            {
                if (attachmentType.IsRequired)
                {
                    throw new UserFriendlyException(L("AttachmentNotFoundCode"), L("AttachmentNotFoundCode_Detail"));

                }
            }
        }

        private void CheckFileType(string fileName, AttachmentType attachmentType)
        {
            var split = fileName.Split('.');
            var last = split.Last();
            if (!string.IsNullOrEmpty(attachmentType.AllowedExtensions) && !string.IsNullOrWhiteSpace(attachmentType.AllowedExtensions))
            {
                if (!attachmentType.AllowedExtensions.Contains(last))
                {
                    throw new UserFriendlyException(L("FileExtensionNotAllowdCode"), L("FileExtensionNotAllowdCode_Detail"));
                }
            }
        }

        private async Task AddAttachment(AttachmentType attachmentType, string ObjectID, List<UploadFilesInputDto> Attachments)
        {
            var allAttachmentEntityType = await _AttachmentEntityTypeRepository.GetAllListAsync();
            
            var attachmentEntityType = allAttachmentEntityType.Where(e => e.Id == attachmentType.EntityTypeId).FirstOrDefault();
            if (attachmentEntityType == null)
            {
                throw new UserFriendlyException(L("InvalidAttachmentEntityTypeIdCode"), L("InvalidAttachmentEntityTypeIdCode_Detail"));
            }
            string path = GetAttachmentPath(ObjectID, attachmentEntityType);
            bool exists = System.IO.Directory.Exists(path);
            if (!exists)
            {
                System.IO.Directory.CreateDirectory(path);
            }
            var allAttached = new List<AttachmentFile>();
            foreach (var file in Attachments)
            {
                var PhysicalName = $"{DateTime.Now.Ticks}_{file.FileName}";
                string attachPath = $"{path}/{PhysicalName}";

                using (var fileStream = new FileStream(attachPath, FileMode.Create))
                {
                    fileStream.Write(file.FileByte, 0, file.FileByte.Length);
                }
                allAttached.Add(new AttachmentFile()
                {
                    AttachmentTypeId = attachmentType.Id,
                    ObjectId = ObjectID,
                    Path = attachPath,
                    PhysicalName = PhysicalName,
                    OriginalName = file.FileName,
                    Size = file.FileByte.Length,
                    Version = file.Version,
                    FileToken = file.FileToken
                }) ;
            }
            foreach (var item in allAttached)
            {
                if (AbpSession.TenantId != null)
                {
                    item.TenantId = (int?)AbpSession.TenantId;
                }


                await _attachmentFileRepository.InsertAsync(item);
            }
        }

        private string GetAttachmentPath(string ObjectIds, AttachmentEntityType attachmentEntityType)
        {
            var splitObjectIds = ObjectIds.Split('.');
            return GetPath(splitObjectIds, splitObjectIds.Length - 1, attachmentEntityType);

        }

        private string GetPath(string[] splitObjectIds, int i, AttachmentEntityType attachmentEntityType)
        {
            if (i < 0)
            {
                throw new UserFriendlyException(L("InvalidObjectIdCode"), L("InvalidObjectIdCode_Detail"));
            }
            if (attachmentEntityType == null)
            {
                throw new UserFriendlyException(L("NotHaveAttachmentEntityTypeCode"), L("NotHaveAttachmentEntityTypeCode_Detail"));
            }
            var ObjectId = splitObjectIds[i];
            if (i == 0 && attachmentEntityType != null)
            {
                return attachmentEntityType.Folder + "/" + ObjectId;
            }
            else
            {
                return  GetPath(splitObjectIds, --i, attachmentEntityType.ParentTypeFk) + attachmentEntityType.Folder + "/" + ObjectId;
            }

        }
    }
}