

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
using Abp.Domain.Uow;
using MyCompanyName.AbpZeroTemplate.Attachments;

namespace MyCompanyName.AbpZeroTemplate.Persons
{
	[AbpAuthorize(AppPermissions.Pages_Persons)]
    public class PersonsAppService : AbpZeroTemplateAppServiceBase, IPersonsAppService
    {
		 private readonly IRepository<Person> _personRepository;
		 private readonly IPersonsExcelExporter _personsExcelExporter;
		private readonly IUnitOfWorkManager _unitOfWorkManager;
		private readonly IAttachmentFilesAppService _AttachmentFilesAppService;


		public PersonsAppService(IAttachmentFilesAppService AttachmentFilesAppService,IUnitOfWorkManager unitOfWorkManager,IRepository<Person> personRepository, IPersonsExcelExporter personsExcelExporter ) 
		  {
			_personRepository = personRepository;
			_personsExcelExporter = personsExcelExporter;
			_unitOfWorkManager = unitOfWorkManager;
			_AttachmentFilesAppService = AttachmentFilesAppService;
			
		  }

		 public async Task<PagedResultDto<GetPersonForViewDto>> GetAll(GetAllPersonsInput input)
         {
			
			var filteredPersons = _personRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.name.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.nameFilter),  e => e.name == input.nameFilter);

			var pagedAndFilteredPersons = filteredPersons
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var persons = from o in pagedAndFilteredPersons
                         select new GetPersonForViewDto() {
							Person = new PersonDto
							{
                                name = o.name,
                                Id = o.Id
							}
						};

            var totalCount = await filteredPersons.CountAsync();

            return new PagedResultDto<GetPersonForViewDto>(
                totalCount,
                await persons.ToListAsync()
            );
         }
		 
		 public async Task<GetPersonForViewDto> GetPersonForView(int id)
         {
            var person = await _personRepository.GetAsync(id);

            var output = new GetPersonForViewDto { Person = ObjectMapper.Map<PersonDto>(person) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Persons_Edit)]
		 public async Task<GetPersonForEditOutput> GetPersonForEdit(EntityDto input)
         {
            var person = await _personRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetPersonForEditOutput {Person = ObjectMapper.Map<CreateOrEditPersonDto>(person)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditPersonDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
			
			
		}

		 [AbpAuthorize(AppPermissions.Pages_Persons_Create)]
		 protected virtual async Task Create(CreateOrEditPersonDto input)
         {
            var person = ObjectMapper.Map<Person>(input);

			
			if (AbpSession.TenantId != null)
			{
				person.TenantId = (int?) AbpSession.TenantId;
			}

			
			await _personRepository.InsertAsync(person);
			await _unitOfWorkManager.Current.SaveChangesAsync();
			
			await _AttachmentFilesAppService.CheckAttachment(AttachmentTypeConsts.IDCardImageID, person.Id + "", input, AttachmentUploadType.add);
		}

		 [AbpAuthorize(AppPermissions.Pages_Persons_Edit)]
		 protected virtual async Task Update(CreateOrEditPersonDto input)
         {
            var person = await _personRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, person);
			await _AttachmentFilesAppService.CheckAttachment(AttachmentTypeConsts.IDCardImageID, person.Id + "", input, AttachmentUploadType.edit);
		}

		 [AbpAuthorize(AppPermissions.Pages_Persons_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _personRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetPersonsToExcel(GetAllPersonsForExcelInput input)
         {
			
			var filteredPersons = _personRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.name.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.nameFilter),  e => e.name == input.nameFilter);

			var query = (from o in filteredPersons
                         select new GetPersonForViewDto() { 
							Person = new PersonDto
							{
                                name = o.name,
                                Id = o.Id
							}
						 });


            var personListDtos = await query.ToListAsync();

            return _personsExcelExporter.ExportToFile(personListDtos);
         }


    }
}