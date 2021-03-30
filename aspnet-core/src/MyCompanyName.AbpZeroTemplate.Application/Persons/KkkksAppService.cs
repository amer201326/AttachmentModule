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

namespace MyCompanyName.AbpZeroTemplate.Persons
{
	[AbpAuthorize(AppPermissions.Pages_Kkkks)]
    public class KkkksAppService : AbpZeroTemplateAppServiceBase, IKkkksAppService
    {
		 private readonly IRepository<Kkkk> _kkkkRepository;
		 private readonly IKkkksExcelExporter _kkkksExcelExporter;
		 private readonly IRepository<Person,int> _lookup_personRepository;
		 

		  public KkkksAppService(IRepository<Kkkk> kkkkRepository, IKkkksExcelExporter kkkksExcelExporter , IRepository<Person, int> lookup_personRepository) 
		  {
			_kkkkRepository = kkkkRepository;
			_kkkksExcelExporter = kkkksExcelExporter;
			_lookup_personRepository = lookup_personRepository;
		
		  }

		 public async Task<PagedResultDto<GetKkkkForViewDto>> GetAll(GetAllKkkksInput input)
         {
			
			var filteredKkkks = _kkkkRepository.GetAll()
						.Include( e => e.PersonFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.name.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.nameFilter),  e => e.name == input.nameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.PersonnameFilter), e => e.PersonFk != null && e.PersonFk.name == input.PersonnameFilter);

			var pagedAndFilteredKkkks = filteredKkkks
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var kkkks = from o in pagedAndFilteredKkkks
                         join o1 in _lookup_personRepository.GetAll() on o.PersonId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetKkkkForViewDto() {
							Kkkk = new KkkkDto
							{
                                name = o.name,
                                Id = o.Id
							},
                         	Personname = s1 == null || s1.name == null ? "" : s1.name.ToString()
						};

            var totalCount = await filteredKkkks.CountAsync();

            return new PagedResultDto<GetKkkkForViewDto>(
                totalCount,
                await kkkks.ToListAsync()
            );
         }
		 
		 public async Task<GetKkkkForViewDto> GetKkkkForView(int id)
         {
            var kkkk = await _kkkkRepository.GetAsync(id);

            var output = new GetKkkkForViewDto { Kkkk = ObjectMapper.Map<KkkkDto>(kkkk) };

		    if (output.Kkkk.PersonId != null)
            {
                var _lookupPerson = await _lookup_personRepository.FirstOrDefaultAsync((int)output.Kkkk.PersonId);
                output.Personname = _lookupPerson?.name?.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Kkkks_Edit)]
		 public async Task<GetKkkkForEditOutput> GetKkkkForEdit(EntityDto input)
         {
            var kkkk = await _kkkkRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetKkkkForEditOutput {Kkkk = ObjectMapper.Map<CreateOrEditKkkkDto>(kkkk)};

		    if (output.Kkkk.PersonId != null)
            {
                var _lookupPerson = await _lookup_personRepository.FirstOrDefaultAsync((int)output.Kkkk.PersonId);
                output.Personname = _lookupPerson?.name?.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditKkkkDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Kkkks_Create)]
		 protected virtual async Task Create(CreateOrEditKkkkDto input)
         {
            var kkkk = ObjectMapper.Map<Kkkk>(input);

			
			if (AbpSession.TenantId != null)
			{
				kkkk.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _kkkkRepository.InsertAsync(kkkk);
         }

		 [AbpAuthorize(AppPermissions.Pages_Kkkks_Edit)]
		 protected virtual async Task Update(CreateOrEditKkkkDto input)
         {
            var kkkk = await _kkkkRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, kkkk);
         }

		 [AbpAuthorize(AppPermissions.Pages_Kkkks_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _kkkkRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetKkkksToExcel(GetAllKkkksForExcelInput input)
         {
			
			var filteredKkkks = _kkkkRepository.GetAll()
						.Include( e => e.PersonFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.name.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.nameFilter),  e => e.name == input.nameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.PersonnameFilter), e => e.PersonFk != null && e.PersonFk.name == input.PersonnameFilter);

			var query = (from o in filteredKkkks
                         join o1 in _lookup_personRepository.GetAll() on o.PersonId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetKkkkForViewDto() { 
							Kkkk = new KkkkDto
							{
                                name = o.name,
                                Id = o.Id
							},
                         	Personname = s1 == null || s1.name == null ? "" : s1.name.ToString()
						 });


            var kkkkListDtos = await query.ToListAsync();

            return _kkkksExcelExporter.ExportToFile(kkkkListDtos);
         }


			[AbpAuthorize(AppPermissions.Pages_Kkkks)]
			public async Task<List<KkkkPersonLookupTableDto>> GetAllPersonForTableDropdown()
			{
				return await _lookup_personRepository.GetAll()
					.Select(person => new KkkkPersonLookupTableDto
					{
						Id = person.Id,
						DisplayName = person == null || person.name == null ? "" : person.name.ToString()
					}).ToListAsync();
			}
							
    }
}