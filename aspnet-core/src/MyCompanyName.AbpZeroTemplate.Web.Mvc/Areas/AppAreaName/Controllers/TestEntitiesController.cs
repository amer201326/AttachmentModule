using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyCompanyName.AbpZeroTemplate.Web.Areas.AppAreaName.Models.TestEntities;
using MyCompanyName.AbpZeroTemplate.Web.Controllers;
using MyCompanyName.AbpZeroTemplate.Authorization;
using MyCompanyName.AbpZeroTemplate.Test;
using MyCompanyName.AbpZeroTemplate.Test.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace MyCompanyName.AbpZeroTemplate.Web.Areas.AppAreaName.Controllers
{
    [Area("AppAreaName")]
    [AbpMvcAuthorize(AppPermissions.Pages_TestEntities)]
    public class TestEntitiesController : AbpZeroTemplateControllerBase
    {
        private readonly ITestEntitiesAppService _testEntitiesAppService;

        public TestEntitiesController(ITestEntitiesAppService testEntitiesAppService)
        {
            _testEntitiesAppService = testEntitiesAppService;
        }

        public ActionResult Index()
        {
            var model = new TestEntitiesViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_TestEntities_Create, AppPermissions.Pages_TestEntities_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(int? id)
			{
				GetTestEntityForEditOutput getTestEntityForEditOutput;

				if (id.HasValue){
					getTestEntityForEditOutput = await _testEntitiesAppService.GetTestEntityForEdit(new EntityDto { Id = (int) id });
				}
				else {
					getTestEntityForEditOutput = new GetTestEntityForEditOutput{
						TestEntity = new CreateOrEditTestEntityDto()
					};
				}

				var viewModel = new CreateOrEditTestEntityModalViewModel()
				{
					TestEntity = getTestEntityForEditOutput.TestEntity,                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewTestEntityModal(int id)
        {
			var getTestEntityForViewDto = await _testEntitiesAppService.GetTestEntityForView(id);

            var model = new TestEntityViewModel()
            {
                TestEntity = getTestEntityForViewDto.TestEntity
            };

            return PartialView("_ViewTestEntityModal", model);
        }


    }
}