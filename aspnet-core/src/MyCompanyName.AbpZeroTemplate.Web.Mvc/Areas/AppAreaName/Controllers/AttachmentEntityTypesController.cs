using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyCompanyName.AbpZeroTemplate.Web.Areas.AppAreaName.Models.AttachmentEntityTypes;
using MyCompanyName.AbpZeroTemplate.Web.Controllers;
using MyCompanyName.AbpZeroTemplate.Authorization;
using MyCompanyName.AbpZeroTemplate.Attachments;
using MyCompanyName.AbpZeroTemplate.Attachments.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace MyCompanyName.AbpZeroTemplate.Web.Areas.AppAreaName.Controllers
{
    [Area("AppAreaName")]
    [AbpMvcAuthorize(AppPermissions.Pages_AttachmentEntityTypes)]
    public class AttachmentEntityTypesController : AbpZeroTemplateControllerBase
    {
        private readonly IAttachmentEntityTypesAppService _attachmentEntityTypesAppService;

        public AttachmentEntityTypesController(IAttachmentEntityTypesAppService attachmentEntityTypesAppService)
        {
            _attachmentEntityTypesAppService = attachmentEntityTypesAppService;
        }

        public ActionResult Index()
        {
            var model = new AttachmentEntityTypesViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_AttachmentEntityTypes_Create, AppPermissions.Pages_AttachmentEntityTypes_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(int? id)
			{
				GetAttachmentEntityTypeForEditOutput getAttachmentEntityTypeForEditOutput;

				if (id.HasValue){
					getAttachmentEntityTypeForEditOutput = await _attachmentEntityTypesAppService.GetAttachmentEntityTypeForEdit(new EntityDto { Id = (int) id });
				}
				else {
					getAttachmentEntityTypeForEditOutput = new GetAttachmentEntityTypeForEditOutput{
						AttachmentEntityType = new CreateOrEditAttachmentEntityTypeDto()
					};
				}

				var viewModel = new CreateOrEditAttachmentEntityTypeModalViewModel()
				{
					AttachmentEntityType = getAttachmentEntityTypeForEditOutput.AttachmentEntityType,
					AttachmentEntityTypeArName = getAttachmentEntityTypeForEditOutput.AttachmentEntityTypeArName,                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewAttachmentEntityTypeModal(int id)
        {
			var getAttachmentEntityTypeForViewDto = await _attachmentEntityTypesAppService.GetAttachmentEntityTypeForView(id);

            var model = new AttachmentEntityTypeViewModel()
            {
                AttachmentEntityType = getAttachmentEntityTypeForViewDto.AttachmentEntityType
                , AttachmentEntityTypeArName = getAttachmentEntityTypeForViewDto.AttachmentEntityTypeArName 

            };

            return PartialView("_ViewAttachmentEntityTypeModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_AttachmentEntityTypes_Create, AppPermissions.Pages_AttachmentEntityTypes_Edit)]
        public PartialViewResult AttachmentEntityTypeLookupTableModal(int? id, string displayName)
        {
            var viewModel = new AttachmentEntityTypeAttachmentEntityTypeLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_AttachmentEntityTypeAttachmentEntityTypeLookupTableModal", viewModel);
        }

    }
}