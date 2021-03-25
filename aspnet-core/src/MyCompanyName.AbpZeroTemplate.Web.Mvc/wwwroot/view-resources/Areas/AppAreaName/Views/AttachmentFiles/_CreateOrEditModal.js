(function ($) {
    app.modals.CreateOrEditAttachmentFileModal = function () {

        var _attachmentFilesService = abp.services.app.attachmentFiles;

        var _modalManager;
        var _$attachmentFileInformationForm = null;

		        var _AttachmentFileattachmentTypeLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'AppAreaName/AttachmentFiles/AttachmentTypeLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/AppAreaName/Views/AttachmentFiles/_AttachmentFileAttachmentTypeLookupTableModal.js',
            modalClass: 'AttachmentTypeLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$attachmentFileInformationForm = _modalManager.getModal().find('form[name=AttachmentFileInformationsForm]');
            _$attachmentFileInformationForm.validate();
        };

		          $('#OpenAttachmentTypeLookupTableButton').click(function () {

            var attachmentFile = _$attachmentFileInformationForm.serializeFormToObject();

            _AttachmentFileattachmentTypeLookupTableModal.open({ id: attachmentFile.attachmentTypeId, displayName: attachmentFile.attachmentTypeArName }, function (data) {
                _$attachmentFileInformationForm.find('input[name=attachmentTypeArName]').val(data.displayName); 
                _$attachmentFileInformationForm.find('input[name=attachmentTypeId]').val(data.id); 
            });
        });
		
		$('#ClearAttachmentTypeArNameButton').click(function () {
                _$attachmentFileInformationForm.find('input[name=attachmentTypeArName]').val(''); 
                _$attachmentFileInformationForm.find('input[name=attachmentTypeId]').val(''); 
        });
		


        this.save = function () {
            if (!_$attachmentFileInformationForm.valid()) {
                return;
            }
            if ($('#AttachmentFile_AttachmentTypeId').prop('required') && $('#AttachmentFile_AttachmentTypeId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('AttachmentType')));
                return;
            }

            var attachmentFile = _$attachmentFileInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _attachmentFilesService.createOrEdit(
				attachmentFile
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditAttachmentFileModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);