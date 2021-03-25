(function ($) {
    app.modals.CreateOrEditAttachmentEntityTypeModal = function () {

        var _attachmentEntityTypesService = abp.services.app.attachmentEntityTypes;

        var _modalManager;
        var _$attachmentEntityTypeInformationForm = null;

		        var _AttachmentEntityTypeattachmentEntityTypeLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'AppAreaName/AttachmentEntityTypes/AttachmentEntityTypeLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/AppAreaName/Views/AttachmentEntityTypes/_AttachmentEntityTypeAttachmentEntityTypeLookupTableModal.js',
            modalClass: 'AttachmentEntityTypeLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$attachmentEntityTypeInformationForm = _modalManager.getModal().find('form[name=AttachmentEntityTypeInformationsForm]');
            _$attachmentEntityTypeInformationForm.validate();
        };

		          $('#OpenAttachmentEntityTypeLookupTableButton').click(function () {

            var attachmentEntityType = _$attachmentEntityTypeInformationForm.serializeFormToObject();

            _AttachmentEntityTypeattachmentEntityTypeLookupTableModal.open({ id: attachmentEntityType.parentTypeId, displayName: attachmentEntityType.attachmentEntityTypeArName }, function (data) {
                _$attachmentEntityTypeInformationForm.find('input[name=attachmentEntityTypeArName]').val(data.displayName); 
                _$attachmentEntityTypeInformationForm.find('input[name=parentTypeId]').val(data.id); 
            });
        });
		
		$('#ClearAttachmentEntityTypeArNameButton').click(function () {
                _$attachmentEntityTypeInformationForm.find('input[name=attachmentEntityTypeArName]').val(''); 
                _$attachmentEntityTypeInformationForm.find('input[name=parentTypeId]').val(''); 
        });
		


        this.save = function () {
            if (!_$attachmentEntityTypeInformationForm.valid()) {
                return;
            }
            if ($('#AttachmentEntityType_ParentTypeId').prop('required') && $('#AttachmentEntityType_ParentTypeId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('AttachmentEntityType')));
                return;
            }

            var attachmentEntityType = _$attachmentEntityTypeInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _attachmentEntityTypesService.createOrEdit(
				attachmentEntityType
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditAttachmentEntityTypeModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);