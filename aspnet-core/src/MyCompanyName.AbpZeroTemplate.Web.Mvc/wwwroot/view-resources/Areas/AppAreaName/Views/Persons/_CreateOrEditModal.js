(function ($) {
    app.modals.CreateOrEditPersonModal = function () {

        var _personsService = abp.services.app.persons;

        var _modalManager;
        var _$personInformationForm = null;
        var personImagesView = $("#personImagesView");
        

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$personInformationForm = _modalManager.getModal().find('form[name=PersonInformationsForm]');
            _$personInformationForm.validate();
        };

		  

        this.save = function () {
            if (!_$personInformationForm.valid()) {
                return;
            }

            var person = _$personInformationForm.serializeFormToObject();
            person.Attachments = personAttachment;
            person.AttachmentsToDelete = personAttachmentToDelete;
			 _modalManager.setBusy(true);
			 _personsService.createOrEdit(
				person
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditPersonModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };

        var personAttachment = [];
        var personAttachmentToDelete = [];
        $("#image").change(function () {
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
                        personAttachment.push(data.result);
                        addImage(data.result)
                        $fileInput.val(''); 
                    }

                },
                data: fd
            });
        });
        personImagesView.find("a.deleteAtt").click(function () {
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
            personImagesView.append(table);
            personImagesView.find("a.deleteAtt").prop('onclick', null).off("click"); 
            personImagesView.find("a.deleteAtt").click(function () {
                deleteAttachment(this);
            });
        }
        function deleteAttachment(element) {
            var button = $(element);
            var token = button.attr("fileToken");
            if (token) {
                var temp = [];
                personAttachment.forEach(function (element) {
                    if (element.fileToken != token) {
                        temp.push(element)
                    }
                });
                personAttachment = temp;
                personAttachmentToDelete.push({ fileToken:token });
            }
            button.parent().parent().remove();
            
        }
    };
})(jQuery);