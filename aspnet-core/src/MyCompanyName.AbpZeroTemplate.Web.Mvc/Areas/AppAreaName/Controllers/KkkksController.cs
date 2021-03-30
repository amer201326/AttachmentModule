using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyCompanyName.AbpZeroTemplate.Web.Areas.AppAreaName.Models.Kkkks;
using MyCompanyName.AbpZeroTemplate.Web.Controllers;
using MyCompanyName.AbpZeroTemplate.Authorization;
using MyCompanyName.AbpZeroTemplate.Persons;
using MyCompanyName.AbpZeroTemplate.Persons.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;
using MyCompanyName.AbpZeroTemplate.Attachments;

namespace MyCompanyName.AbpZeroTemplate.Web.Areas.AppAreaName.Controllers
{
    [Area("AppAreaName")]
    [AbpMvcAuthorize(AppPermissions.Pages_Kkkks)]
    public class KkkksController : AbpZeroTemplateControllerBase
    {
        private readonly IKkkksAppService _kkkksAppService;
        private readonly IAttachmentFilesAppService _attachmentFilesAppService;
        private readonly IAttachmentTypesAppService _attachmentTypesAppService;

        public KkkksController(IAttachmentTypesAppService attachmentTypesAppService, IAttachmentFilesAppService attachmentFilesAppService, IKkkksAppService kkkksAppService)
        {
            _kkkksAppService = kkkksAppService;
            _attachmentFilesAppService = attachmentFilesAppService;
            _attachmentTypesAppService = attachmentTypesAppService;
        }

        public ActionResult Index()
        {
            var model = new KkkksViewModel
            {
                FilterText = ""
            };

            return View(model);
        }


        [AbpMvcAuthorize(AppPermissions.Pages_Kkkks_Create, AppPermissions.Pages_Kkkks_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetKkkkForEditOutput getKkkkForEditOutput;

            if (id.HasValue)
            {
                getKkkkForEditOutput = await _kkkksAppService.GetKkkkForEdit(new EntityDto { Id = (int)id });
                getKkkkForEditOutput.Kkkk.Attachments = await _attachmentFilesAppService.GetAttacments(id + "", AttachmentTypeConsts.IDCardImageID);

            }
            else
            {
                getKkkkForEditOutput = new GetKkkkForEditOutput
                {
                    Kkkk = new CreateOrEditKkkkDto()
                };
            }

            var viewModel = new CreateOrEditKkkkModalViewModel()
            {
                Kkkk = getKkkkForEditOutput.Kkkk,
                Personname = getKkkkForEditOutput.Personname,
                KkkkPersonList = await _kkkksAppService.GetAllPersonForTableDropdown(),
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }


        public async Task<PartialViewResult> ViewKkkkModal(int id)
        {
            var getKkkkForViewDto = await _kkkksAppService.GetKkkkForView(id);

            var model = new KkkkViewModel()
            {
                Kkkk = getKkkkForViewDto.Kkkk
                ,
                Personname = getKkkkForViewDto.Personname

            };

            return PartialView("_ViewKkkkModal", model);
        }


    }
}