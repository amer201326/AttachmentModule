(function ($) {
    app.modals.CreateOrEditDiseaseModal = function () {

        var _diseasesService = abp.services.app.diseases;

        var _modalManager;
        var _$diseaseInformationForm = null;

		        var _DiseasepersonLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'AppAreaName/Diseases/PersonLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/AppAreaName/Views/Diseases/_DiseasePersonLookupTableModal.js',
            modalClass: 'PersonLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$diseaseInformationForm = _modalManager.getModal().find('form[name=DiseaseInformationsForm]');
            _$diseaseInformationForm.validate();
        };

		          $('#OpenPersonLookupTableButton').click(function () {

            var disease = _$diseaseInformationForm.serializeFormToObject();

            _DiseasepersonLookupTableModal.open({ id: disease.personId, displayName: disease.personname }, function (data) {
                _$diseaseInformationForm.find('input[name=personname]').val(data.displayName); 
                _$diseaseInformationForm.find('input[name=personId]').val(data.id); 
            });
        });
		
		$('#ClearPersonnameButton').click(function () {
                _$diseaseInformationForm.find('input[name=personname]').val(''); 
                _$diseaseInformationForm.find('input[name=personId]').val(''); 
        });
		


        this.save = function () {
            if (!_$diseaseInformationForm.valid()) {
                return;
            }
            if ($('#Disease_PersonId').prop('required') && $('#Disease_PersonId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Person')));
                return;
            }

            var disease = _$diseaseInformationForm.serializeFormToObject();
            disease.Attachments = DiseaseAttachment;
            disease.AttachmentsToDelete = DiseaseAttachmentToDelete;
			 _modalManager.setBusy(true);
			 _diseasesService.createOrEdit(
				disease
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditDiseaseModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };

        var DiseaseImagesView = $("#DiseaseImagesView");

        var DiseaseAttachment = [];
        var DiseaseAttachmentToDelete = [];
        $("#DiseaseAttachments").change(function () {
            var $fileInput = $(this);
            var files = $fileInput[0].files;
            if (!files.length) {
                return false;
            }
            console.log(files)
            var fd = new FormData();
            fd.append('file', files[0]);

            $.ajax({
                url: '/AppAreaName/AttachmentFiles/UploadFiles',
                type: 'post',
                contentType: false,
                processData: false,
                success: function (data) {
                    console.log(data)
                    if (data.success) {
                        DiseaseAttachment.push(data.result);
                        addImage(data.result)
                        $fileInput.val('');
                    }

                },
                data: fd
            });
        });
        DiseaseImagesView.find("a.deleteAtt").click(function () {
            deleteAttachment(this);
        });

        function addImage(data) {
            var table = `
                <div class="row margin-bottom-10">
                    <div class="col-sm">
                        ${data.fileName}
                    </div>
                    
                    <div class="col-sm">
                        <a fileToken="${data.fileToken}" href="#" class="deleteAtt btn btn-icon btn-danger btn-sm mr-2">
                            <i class="far fa-trash-alt"></i>
                        </a>
                    </div>
                </div>
            `;
            DiseaseImagesView.append(table);
            DiseaseImagesView.find("a.deleteAtt").prop('onclick', null).off("click");
            DiseaseImagesView.find("a.deleteAtt").click(function () {
                deleteAttachment(this);
            });
        }
        function deleteAttachment(element) {
            var button = $(element);
            var token = button.attr("fileToken");
            if (token) {
                var temp = [];
                DiseaseAttachment.forEach(function (element) {
                    if (element.fileToken != token) {
                        temp.push(element)
                    }
                });
                DiseaseAttachment = temp;
                DiseaseAttachmentToDelete.push({ fileToken: token });
            }
            button.parent().parent().remove();

        }
    };
})(jQuery);