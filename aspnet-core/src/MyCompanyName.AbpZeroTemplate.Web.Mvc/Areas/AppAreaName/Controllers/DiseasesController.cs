using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyCompanyName.AbpZeroTemplate.Web.Areas.AppAreaName.Models.Diseases;
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
    [AbpMvcAuthorize(AppPermissions.Pages_Diseases)]
    public class DiseasesController : AbpZeroTemplateControllerBase
    {
        private readonly IDiseasesAppService _diseasesAppService;
        private readonly IAttachmentFilesAppService _attachmentFilesAppService;
        private readonly IAttachmentTypesAppService _attachmentTypesAppService;

        public DiseasesController(IAttachmentTypesAppService attachmentTypesAppService,IAttachmentFilesAppService attachmentFilesAppService, IDiseasesAppService diseasesAppService)
        {
            _diseasesAppService = diseasesAppService;
            _attachmentFilesAppService = attachmentFilesAppService;
            _attachmentTypesAppService = attachmentTypesAppService;
        }

        public ActionResult Index()
        {
            var model = new DiseasesViewModel
            {
                FilterText = ""
            };

            return View(model);
        }


        [AbpMvcAuthorize(AppPermissions.Pages_Diseases_Create, AppPermissions.Pages_Diseases_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetDiseaseForEditOutput getDiseaseForEditOutput;

            if (id.HasValue)
            {
                getDiseaseForEditOutput = await _diseasesAppService.GetDiseaseForEdit(new EntityDto { Id = (int)id });
                getDiseaseForEditOutput.Disease.Attachments = await _attachmentFilesAppService.GetAttacments(getDiseaseForEditOutput.Disease.PersonId+"."+id , AttachmentTypeConsts.DiagnoseTheDisease);

            }
            else
            {
                getDiseaseForEditOutput = new GetDiseaseForEditOutput
                {
                    Disease = new CreateOrEditDiseaseDto()
                };
            }

            var viewModel = new CreateOrEditDiseaseModalViewModel()
            {
                Disease = getDiseaseForEditOutput.Disease,
                Personname = getDiseaseForEditOutput.Personname,
                DiseasePersonList = await _diseasesAppService.GetAllPersonForTableDropdown(),
                AttachmentType = await _attachmentTypesAppService.GetAttachmentTypeById(AttachmentTypeConsts.DiagnoseTheDisease)

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }


        public async Task<PartialViewResult> ViewDiseaseModal(int id)
        {
            var getDiseaseForViewDto = await _diseasesAppService.GetDiseaseForView(id);

            var model = new DiseaseViewModel()
            {
                Disease = getDiseaseForViewDto.Disease
                ,
                Personname = getDiseaseForViewDto.Personname

            };

            return PartialView("_ViewDiseaseModal", model);
        }
        public async Task<ActionResult> DownloadAttacment(string FileToken)
        {
            var result = await _attachmentFilesAppService.DownloadAttaachment(FileToken);
            return File(result.Memory, result.ApplicationType, result.FileName);
        }
    }
}