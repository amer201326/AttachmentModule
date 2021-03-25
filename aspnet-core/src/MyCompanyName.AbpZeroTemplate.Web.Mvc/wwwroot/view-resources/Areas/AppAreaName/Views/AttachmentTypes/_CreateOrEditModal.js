(function ($) {
    app.modals.CreateOrEditAttachmentTypeModal = function () {

        var _attachmentTypesService = abp.services.app.attachmentTypes;

        var _modalManager;
        var _$attachmentTypeInformationForm = null;

		        var _AttachmentTypeattachmentEntityTypeLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'AppAreaName/AttachmentTypes/AttachmentEntityTypeLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/AppAreaName/Views/AttachmentTypes/_AttachmentTypeAttachmentEntityTypeLookupTableModal.js',
            modalClass: 'AttachmentEntityTypeLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$attachmentTypeInformationForm = _modalManager.getModal().find('form[name=AttachmentTypeInformationsForm]');
            _$attachmentTypeInformationForm.validate();
        };

		          $('#OpenAttachmentEntityTypeLookupTableButton').click(function () {

            var attachmentType = _$attachmentTypeInformationForm.serializeFormToObject();

            _AttachmentTypeattachmentEntityTypeLookupTableModal.open({ id: attachmentType.entityTypeId, displayName: attachmentType.attachmentEntityTypeArName }, function (data) {
                _$attachmentTypeInformationForm.find('input[name=attachmentEntityTypeArName]').val(data.displayName); 
                _$attachmentTypeInformationForm.find('input[name=entityTypeId]').val(data.id); 
            });
        });
		
		$('#ClearAttachmentEntityTypeArNameButton').click(function () {
                _$attachmentTypeInformationForm.find('input[name=attachmentEntityTypeArName]').val(''); 
                _$attachmentTypeInformationForm.find('input[name=entityTypeId]').val(''); 
        });
		


        this.save = function () {
            if (!_$attachmentTypeInformationForm.valid()) {
                return;
            }
            if ($('#AttachmentType_EntityTypeId').prop('required') && $('#AttachmentType_EntityTypeId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('AttachmentEntityType')));
                return;
            }

            var attachmentType = _$attachmentTypeInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _attachmentTypesService.createOrEdit(
				attachmentType
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditAttachmentTypeModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);