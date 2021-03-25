

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using MyCompanyName.AbpZeroTemplate.Test.Exporting;
using MyCompanyName.AbpZeroTemplate.Test.Dtos;
using MyCompanyName.AbpZeroTemplate.Dto;
using Abp.Application.Services.Dto;
using MyCompanyName.AbpZeroTemplate.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace MyCompanyName.AbpZeroTemplate.Test
{
	[AbpAuthorize(AppPermissions.Pages_TestEntities)]
    public class TestEntitiesAppService : AbpZeroTemplateAppServiceBase, ITestEntitiesAppService
    {
		 private readonly IRepository<TestEntity> _testEntityRepository;
		 private readonly ITestEntitiesExcelExporter _testEntitiesExcelExporter;
		 

		  public TestEntitiesAppService(IRepository<TestEntity> testEntityRepository, ITestEntitiesExcelExporter testEntitiesExcelExporter ) 
		  {
			_testEntityRepository = testEntityRepository;
			_testEntitiesExcelExporter = testEntitiesExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetTestEntityForViewDto>> GetAll(GetAllTestEntitiesInput input)
         {
			
			var filteredTestEntities = _testEntityRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.ArName.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.ArNameFilter),  e => e.ArName == input.ArNameFilter);

			var pagedAndFilteredTestEntities = filteredTestEntities
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var testEntities = from o in pagedAndFilteredTestEntities
                         select new GetTestEntityForViewDto() {
							TestEntity = new TestEntityDto
							{
                                ArName = o.ArName,
                                Id = o.Id
							}
						};

            var totalCount = await filteredTestEntities.CountAsync();

            return new PagedResultDto<GetTestEntityForViewDto>(
                totalCount,
                await testEntities.ToListAsync()
            );
         }
		 
		 public async Task<GetTestEntityForViewDto> GetTestEntityForView(int id)
         {
            var testEntity = await _testEntityRepository.GetAsync(id);

            var output = new GetTestEntityForViewDto { TestEntity = ObjectMapper.Map<TestEntityDto>(testEntity) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_TestEntities_Edit)]
		 public async Task<GetTestEntityForEditOutput> GetTestEntityForEdit(EntityDto input)
         {
            var testEntity = await _testEntityRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetTestEntityForEditOutput {TestEntity = ObjectMapper.Map<CreateOrEditTestEntityDto>(testEntity)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditTestEntityDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_TestEntities_Create)]
		 protected virtual async Task Create(CreateOrEditTestEntityDto input)
         {
            var testEntity = ObjectMapper.Map<TestEntity>(input);

			
			if (AbpSession.TenantId != null)
			{
				testEntity.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _testEntityRepository.InsertAsync(testEntity);
         }

		 [AbpAuthorize(AppPermissions.Pages_TestEntities_Edit)]
		 protected virtual async Task Update(CreateOrEditTestEntityDto input)
         {
            var testEntity = await _testEntityRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, testEntity);
         }

		 [AbpAuthorize(AppPermissions.Pages_TestEntities_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _testEntityRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetTestEntitiesToExcel(GetAllTestEntitiesForExcelInput input)
         {
			
			var filteredTestEntities = _testEntityRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.ArName.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.ArNameFilter),  e => e.ArName == input.ArNameFilter);

			var query = (from o in filteredTestEntities
                         select new GetTestEntityForViewDto() { 
							TestEntity = new TestEntityDto
							{
                                ArName = o.ArName,
                                Id = o.Id
							}
						 });


            var testEntityListDtos = await query.ToListAsync();

            return _testEntitiesExcelExporter.ExportToFile(testEntityListDtos);
         }


    }
}