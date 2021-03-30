(function ($) {
    app.modals.CreateOrEditKkkkModal = function () {

        var _kkkksService = abp.services.app.kkkks;

        var _modalManager;
        var _$kkkkInformationForm = null;

		        var _KkkkpersonLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'AppAreaName/Kkkks/PersonLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/AppAreaName/Views/Kkkks/_KkkkPersonLookupTableModal.js',
            modalClass: 'PersonLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$kkkkInformationForm = _modalManager.getModal().find('form[name=KkkkInformationsForm]');
            _$kkkkInformationForm.validate();
        };

		          $('#OpenPersonLookupTableButton').click(function () {

            var kkkk = _$kkkkInformationForm.serializeFormToObject();

            _KkkkpersonLookupTableModal.open({ id: kkkk.personId, displayName: kkkk.personname }, function (data) {
                _$kkkkInformationForm.find('input[name=personname]').val(data.displayName); 
                _$kkkkInformationForm.find('input[name=personId]').val(data.id); 
            });
        });
		
		$('#ClearPersonnameButton').click(function () {
                _$kkkkInformationForm.find('input[name=personname]').val(''); 
                _$kkkkInformationForm.find('input[name=personId]').val(''); 
        });
		


        this.save = function () {
            if (!_$kkkkInformationForm.valid()) {
                return;
            }
            if ($('#Kkkk_PersonId').prop('required') && $('#Kkkk_PersonId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Person')));
                return;
            }

            var kkkk = _$kkkkInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _kkkksService.createOrEdit(
				kkkk
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditKkkkModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);