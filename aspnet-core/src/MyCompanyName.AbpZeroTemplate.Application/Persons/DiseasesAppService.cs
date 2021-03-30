using MyCompanyName.AbpZeroTemplate.Persons;
					using System.Collections.Generic;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using MyCompanyName.AbpZeroTemplate.Persons.Exporting;
using MyCompanyName.AbpZeroTemplate.Persons.Dtos;
using MyCompanyName.AbpZeroTemplate.Dto;
using Abp.Application.Services.Dto;
using MyCompanyName.AbpZeroTemplate.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using MyCompanyName.AbpZeroTemplate.Attachments;

namespace MyCompanyName.AbpZeroTemplate.Persons
{
	[AbpAuthorize(AppPermissions.Pages_Diseases)]
    public class DiseasesAppService : AbpZeroTemplateAppServiceBase, IDiseasesAppService
    {
		 private readonly IRepository<Disease> _diseaseRepository;
		 private readonly IDiseasesExcelExporter _diseasesExcelExporter;
		 private readonly IRepository<Person,int> _lookup_personRepository;
		 private readonly IAttachmentFilesAppService _attachmentFilesAppService;
		 

		  public DiseasesAppService(IAttachmentFilesAppService attachmentFilesAppService,IRepository<Disease> diseaseRepository, IDiseasesExcelExporter diseasesExcelExporter , IRepository<Person, int> lookup_personRepository) 
		  {
			_diseaseRepository = diseaseRepository;
			_diseasesExcelExporter = diseasesExcelExporter;
			_lookup_personRepository = lookup_personRepository;
			_attachmentFilesAppService = attachmentFilesAppService;
		
		  }

		 public async Task<PagedResultDto<GetDiseaseForViewDto>> GetAll(GetAllDiseasesInput input)
         {
			
			var filteredDiseases = _diseaseRepository.GetAll()
						.Include( e => e.PersonFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.name.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.nameFilter),  e => e.name == input.nameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.PersonnameFilter), e => e.PersonFk != null && e.PersonFk.name == input.PersonnameFilter);

			var pagedAndFilteredDiseases = filteredDiseases
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var diseases = from o in pagedAndFilteredDiseases
                         join o1 in _lookup_personRepository.GetAll() on o.PersonId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetDiseaseForViewDto() {
							Disease = new DiseaseDto
							{
                                name = o.name,
                                Id = o.Id
							},
                         	Personname = s1 == null || s1.name == null ? "" : s1.name.ToString()
						};

            var totalCount = await filteredDiseases.CountAsync();

            return new PagedResultDto<GetDiseaseForViewDto>(
                totalCount,
                await diseases.ToListAsync()
            );
         }
		 
		 public async Task<GetDiseaseForViewDto> GetDiseaseForView(int id)
         {
            var disease = await _diseaseRepository.GetAsync(id);

            var output = new GetDiseaseForViewDto { Disease = ObjectMapper.Map<DiseaseDto>(disease) };

		    if (output.Disease.PersonId != null)
            {
                var _lookupPerson = await _lookup_personRepository.FirstOrDefaultAsync((int)output.Disease.PersonId);
                output.Personname = _lookupPerson?.name?.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Diseases_Edit)]
		 public async Task<GetDiseaseForEditOutput> GetDiseaseForEdit(EntityDto input)
         {
            var disease = await _diseaseRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetDiseaseForEditOutput {Disease = ObjectMapper.Map<CreateOrEditDiseaseDto>(disease)};

		    if (output.Disease.PersonId != null)
            {
                var _lookupPerson = await _lookup_personRepository.FirstOrDefaultAsync((int)output.Disease.PersonId);
                output.Personname = _lookupPerson?.name?.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditDiseaseDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Diseases_Create)]
		 protected virtual async Task Create(CreateOrEditDiseaseDto input)
         {
            var disease = ObjectMapper.Map<Disease>(input);

			
			if (AbpSession.TenantId != null)
			{
				disease.TenantId = (int?) AbpSession.TenantId;
			}
		

            var id = await _diseaseRepository.InsertAndGetIdAsync(disease);
			await _attachmentFilesAppService.CheckAttachment(AttachmentTypeConsts.DiagnoseTheDisease, input.PersonId + "." +id, input, AttachmentUploadType.add);


		}

		[AbpAuthorize(AppPermissions.Pages_Diseases_Edit)]
		 protected virtual async Task Update(CreateOrEditDiseaseDto input)
         {
            var disease = await _diseaseRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, disease);
			await _attachmentFilesAppService.CheckAttachment(AttachmentTypeConsts.DiagnoseTheDisease, input.PersonId+"."+ input.Id, input, AttachmentUploadType.edit);

		}

		[AbpAuthorize(AppPermissions.Pages_Diseases_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _diseaseRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetDiseasesToExcel(GetAllDiseasesForExcelInput input)
         {
			
			var filteredDiseases = _diseaseRepository.GetAll()
						.Include( e => e.PersonFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.name.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.nameFilter),  e => e.name == input.nameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.PersonnameFilter), e => e.PersonFk != null && e.PersonFk.name == input.PersonnameFilter);

			var query = (from o in filteredDiseases
                         join o1 in _lookup_personRepository.GetAll() on o.PersonId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetDiseaseForViewDto() { 
							Disease = new DiseaseDto
							{
                                name = o.name,
                                Id = o.Id
							},
                         	Personname = s1 == null || s1.name == null ? "" : s1.name.ToString()
						 });


            var diseaseListDtos = await query.ToListAsync();

            return _diseasesExcelExporter.ExportToFile(diseaseListDtos);
         }


			[AbpAuthorize(AppPermissions.Pages_Diseases)]
			public async Task<List<DiseasePersonLookupTableDto>> GetAllPersonForTableDropdown()
			{
				return await _lookup_personRepository.GetAll()
					.Select(person => new DiseasePersonLookupTableDto
					{
						Id = person.Id,
						DisplayName = person == null || person.name == null ? "" : person.name.ToString()
					}).ToListAsync();
			}
							
    }
}