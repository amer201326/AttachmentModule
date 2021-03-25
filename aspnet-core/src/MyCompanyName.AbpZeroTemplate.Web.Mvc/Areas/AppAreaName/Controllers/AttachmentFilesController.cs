using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyCompanyName.AbpZeroTemplate.Web.Areas.AppAreaName.Models.AttachmentFiles;
using MyCompanyName.AbpZeroTemplate.Web.Controllers;
using MyCompanyName.AbpZeroTemplate.Authorization;
using MyCompanyName.AbpZeroTemplate.Attachments;
using MyCompanyName.AbpZeroTemplate.Attachments.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace MyCompanyName.AbpZeroTemplate.Web.Areas.AppAreaName.Controllers
{
    [Area("AppAreaName")]
    [AbpMvcAuthorize(AppPermissions.Pages_AttachmentFiles)]
    public class AttachmentFilesController : AbpZeroTemplateControllerBase
    {
        private readonly IAttachmentFilesAppService _attachmentFilesAppService;

        public AttachmentFilesController(IAttachmentFilesAppService attachmentFilesAppService)
        {
            _attachmentFilesAppService = attachmentFilesAppService;
        }

        public ActionResult Index()
        {
            var model = new AttachmentFilesViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_AttachmentFiles_Create, AppPermissions.Pages_AttachmentFiles_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(long? id)
			{
				GetAttachmentFileForEditOutput getAttachmentFileForEditOutput;

				if (id.HasValue){
					getAttachmentFileForEditOutput = await _attachmentFilesAppService.GetAttachmentFileForEdit(new EntityDto<long> { Id = (long) id });
				}
				else {
					getAttachmentFileForEditOutput = new GetAttachmentFileForEditOutput{
						AttachmentFile = new CreateOrEditAttachmentFileDto()
					};
				}

				var viewModel = new CreateOrEditAttachmentFileModalViewModel()
				{
					AttachmentFile = getAttachmentFileForEditOutput.AttachmentFile,
					AttachmentTypeArName = getAttachmentFileForEditOutput.AttachmentTypeArName,
					AttachmentFileAttachmentTypeList = await _attachmentFilesAppService.GetAllAttachmentTypeForTableDropdown(),                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewAttachmentFileModal(long id)
        {
			var getAttachmentFileForViewDto = await _attachmentFilesAppService.GetAttachmentFileForView(id);

            var model = new AttachmentFileViewModel()
            {
                AttachmentFile = getAttachmentFileForViewDto.AttachmentFile
                , AttachmentTypeArName = getAttachmentFileForViewDto.AttachmentTypeArName 

            };

            return PartialView("_ViewAttachmentFileModal", model);
        }


    }
}