(function ($) {
    app.modals.CreateOrEditTestEntityModal = function () {

        var _testEntitiesService = abp.services.app.testEntities;

        var _modalManager;
        var _$testEntityInformationForm = null;

		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$testEntityInformationForm = _modalManager.getModal().find('form[name=TestEntityInformationsForm]');
            _$testEntityInformationForm.validate();
        };

		  

        this.save = function () {
            if (!_$testEntityInformationForm.valid()) {
                return;
            }

            var testEntity = _$testEntityInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _testEntitiesService.createOrEdit(
				testEntity
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditTestEntityModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);