(function () {
    $(function () {

        var _$diseasesTable = $('#DiseasesTable');
        var _diseasesService = abp.services.app.diseases;
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Diseases.Create'),
            edit: abp.auth.hasPermission('Pages.Diseases.Edit'),
            'delete': abp.auth.hasPermission('Pages.Diseases.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
                    viewUrl: abp.appPath + 'AppAreaName/Diseases/CreateOrEditModal',
                    scriptUrl: abp.appPath + 'view-resources/Areas/AppAreaName/Views/Diseases/_CreateOrEditModal.js',
                    modalClass: 'CreateOrEditDiseaseModal'
                });
                   

		 var _viewDiseaseModal = new app.ModalManager({
            viewUrl: abp.appPath + 'AppAreaName/Diseases/ViewdiseaseModal',
            modalClass: 'ViewDiseaseModal'
        });

		
		

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z"); 
        }
        
        var getMaxDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT23:59:59Z"); 
        }

        var dataTable = _$diseasesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _diseasesService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#DiseasesTableFilter').val(),
					nameFilter: $('#nameFilterId').val(),
					personnameFilter: $('#PersonnameFilterId').val()
                    };
                }
            },
            columnDefs: [
                {
                    className: 'control responsive',
                    orderable: false,
                    render: function () {
                        return '';
                    },
                    targets: 0
                },
                {
                    width: 120,
                    targets: 1,
                    data: null,
                    orderable: false,
                    autoWidth: false,
                    defaultContent: '',
                    rowAction: {
                        cssClass: 'btn btn-brand dropdown-toggle',
                        text: '<i class="fa fa-cog"></i> ' + app.localize('Actions') + ' <span class="caret"></span>',
                        items: [
						{
                                text: app.localize('View'),
                                iconStyle: 'far fa-eye mr-2',
                                action: function (data) {
                                    _viewDiseaseModal.open({ id: data.record.disease.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            iconStyle: 'far fa-edit mr-2',
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.disease.id });                                
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            iconStyle: 'far fa-trash-alt mr-2',
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteDisease(data.record.disease);
                            }
                        }]
                    }
                },
					{
						targets: 2,
						 data: "disease.name",
						 name: "name"   
					},
					{
						targets: 3,
						 data: "personname" ,
						 name: "personFk.name" 
					}
            ]
        });

        function getDiseases() {
            dataTable.ajax.reload();
        }

        function deleteDisease(disease) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _diseasesService.delete({
                            id: disease.id
                        }).done(function () {
                            getDiseases(true);
                            abp.notify.success(app.localize('SuccessfullyDeleted'));
                        });
                    }
                }
            );
        }

		$('#ShowAdvancedFiltersSpan').click(function () {
            $('#ShowAdvancedFiltersSpan').hide();
            $('#HideAdvancedFiltersSpan').show();
            $('#AdvacedAuditFiltersArea').slideDown();
        });

        $('#HideAdvancedFiltersSpan').click(function () {
            $('#HideAdvancedFiltersSpan').hide();
            $('#ShowAdvancedFiltersSpan').show();
            $('#AdvacedAuditFiltersArea').slideUp();
        });

        $('#CreateNewDiseaseButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _diseasesService
                .getDiseasesToExcel({
				filter : $('#DiseasesTableFilter').val(),
					nameFilter: $('#nameFilterId').val(),
					personnameFilter: $('#PersonnameFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditDiseaseModalSaved', function () {
            getDiseases();
        });

		$('#GetDiseasesButton').click(function (e) {
            e.preventDefault();
            getDiseases();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getDiseases();
		  }
		});
		
		
		
    });
})();
