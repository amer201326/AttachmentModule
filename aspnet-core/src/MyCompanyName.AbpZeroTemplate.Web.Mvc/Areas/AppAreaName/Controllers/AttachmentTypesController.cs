using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyCompanyName.AbpZeroTemplate.Web.Areas.AppAreaName.Models.AttachmentTypes;
using MyCompanyName.AbpZeroTemplate.Web.Controllers;
using MyCompanyName.AbpZeroTemplate.Authorization;
using MyCompanyName.AbpZeroTemplate.Attachments;
using MyCompanyName.AbpZeroTemplate.Attachments.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace MyCompanyName.AbpZeroTemplate.Web.Areas.AppAreaName.Controllers
{
    [Area("AppAreaName")]
    [AbpMvcAuthorize(AppPermissions.Pages_AttachmentTypes)]
    public class AttachmentTypesController : AbpZeroTemplateControllerBase
    {
        private readonly IAttachmentTypesAppService _attachmentTypesAppService;

        public AttachmentTypesController(IAttachmentTypesAppService attachmentTypesAppService)
        {
            _attachmentTypesAppService = attachmentTypesAppService;
        }

        public ActionResult Index()
        {
            var model = new AttachmentTypesViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_AttachmentTypes_Create, AppPermissions.Pages_AttachmentTypes_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(int? id)
			{
				GetAttachmentTypeForEditOutput getAttachmentTypeForEditOutput;

				if (id.HasValue){
					getAttachmentTypeForEditOutput = await _attachmentTypesAppService.GetAttachmentTypeForEdit(new EntityDto { Id = (int) id });
				}
				else {
					getAttachmentTypeForEditOutput = new GetAttachmentTypeForEditOutput{
						AttachmentType = new CreateOrEditAttachmentTypeDto()
					};
				}

				var viewModel = new CreateOrEditAttachmentTypeModalViewModel()
				{
					AttachmentType = getAttachmentTypeForEditOutput.AttachmentType,
					AttachmentEntityTypeArName = getAttachmentTypeForEditOutput.AttachmentEntityTypeArName,
					AttachmentTypeAttachmentEntityTypeList = await _attachmentTypesAppService.GetAllAttachmentEntityTypeForTableDropdown(),                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewAttachmentTypeModal(int id)
        {
			var getAttachmentTypeForViewDto = await _attachmentTypesAppService.GetAttachmentTypeForView(id);

            var model = new AttachmentTypeViewModel()
            {
                AttachmentType = getAttachmentTypeForViewDto.AttachmentType
                , AttachmentEntityTypeArName = getAttachmentTypeForViewDto.AttachmentEntityTypeArName 

            };

            return PartialView("_ViewAttachmentTypeModal", model);
        }


    }
}