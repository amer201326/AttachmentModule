using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyCompanyName.AbpZeroTemplate.Web.Areas.AppAreaName.Models.Persons;
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
    [AbpMvcAuthorize(AppPermissions.Pages_Persons)]
    public class PersonsController : AbpZeroTemplateControllerBase
    {
        private readonly IPersonsAppService _personsAppService;
        private readonly IAttachmentFilesAppService _attachmentFilesAppService;
        private readonly IAttachmentTypesAppService _attachmentTypesAppService;

        public PersonsController(IPersonsAppService personsAppService, IAttachmentFilesAppService attachmentFilesAppService, IAttachmentTypesAppService attachmentTypesAppService)
        {
            _personsAppService = personsAppService;
            _attachmentFilesAppService = attachmentFilesAppService;
            _attachmentTypesAppService = attachmentTypesAppService;
        }

        public ActionResult Index()
        {
            var model = new PersonsViewModel
            {
                FilterText = ""
            };

            return View(model);
        }


        [AbpMvcAuthorize(AppPermissions.Pages_Persons_Create, AppPermissions.Pages_Persons_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetPersonForEditOutput getPersonForEditOutput;

            if (id.HasValue)
            {
                getPersonForEditOutput = await _personsAppService.GetPersonForEdit(new EntityDto { Id = (int)id });
                getPersonForEditOutput.Person.Attachments = await _attachmentFilesAppService.GetAttacments(id + "", AttachmentTypeConsts.IDCardImageID);

                }
            else
            {
                getPersonForEditOutput = new GetPersonForEditOutput
                {
                    Person = new CreateOrEditPersonDto()
                };
            }

            var viewModel = new CreateOrEditPersonModalViewModel()
            {
                Person = getPersonForEditOutput.Person,
                AttachmentType = await _attachmentTypesAppService.GetAttachmentTypeById(AttachmentTypeConsts.IDCardImageID)
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }


        public async Task<PartialViewResult> ViewPersonModal(int id)
        {
            var getPersonForViewDto = await _personsAppService.GetPersonForView(id);

            var model = new PersonViewModel()
            {
                Person = getPersonForViewDto.Person
            };

            return PartialView("_ViewPersonModal", model);
        }
        public async Task<ActionResult> DownloadAttacment(string FileToken)
        {
            var result = await _attachmentFilesAppService.DownloadAttaachment(FileToken);
            return File(result.Memory, result.ApplicationType, result.FileName);
        }

    }
}