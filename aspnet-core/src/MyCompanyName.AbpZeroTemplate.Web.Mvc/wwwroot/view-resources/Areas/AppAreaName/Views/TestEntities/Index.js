(function () {
    $(function () {

        var _$testEntitiesTable = $('#TestEntitiesTable');
        var _testEntitiesService = abp.services.app.testEntities;
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.TestEntities.Create'),
            edit: abp.auth.hasPermission('Pages.TestEntities.Edit'),
            'delete': abp.auth.hasPermission('Pages.TestEntities.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
                    viewUrl: abp.appPath + 'AppAreaName/TestEntities/CreateOrEditModal',
                    scriptUrl: abp.appPath + 'view-resources/Areas/AppAreaName/Views/TestEntities/_CreateOrEditModal.js',
                    modalClass: 'CreateOrEditTestEntityModal'
                });
                   

		 var _viewTestEntityModal = new app.ModalManager({
            viewUrl: abp.appPath + 'AppAreaName/TestEntities/ViewtestEntityModal',
            modalClass: 'ViewTestEntityModal'
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

        var dataTable = _$testEntitiesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _testEntitiesService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#TestEntitiesTableFilter').val(),
					arNameFilter: $('#ArNameFilterId').val()
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
                                    _viewTestEntityModal.open({ id: data.record.testEntity.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            iconStyle: 'far fa-edit mr-2',
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.testEntity.id });                                
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            iconStyle: 'far fa-trash-alt mr-2',
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteTestEntity(data.record.testEntity);
                            }
                        }]
                    }
                },
					{
						targets: 2,
						 data: "testEntity.arName",
						 name: "arName"   
					}
            ]
        });

        function getTestEntities() {
            dataTable.ajax.reload();
        }

        function deleteTestEntity(testEntity) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _testEntitiesService.delete({
                            id: testEntity.id
                        }).done(function () {
                            getTestEntities(true);
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

        $('#CreateNewTestEntityButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _testEntitiesService
                .getTestEntitiesToExcel({
				filter : $('#TestEntitiesTableFilter').val(),
					arNameFilter: $('#ArNameFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditTestEntityModalSaved', function () {
            getTestEntities();
        });

		$('#GetTestEntitiesButton').click(function (e) {
            e.preventDefault();
            getTestEntities();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getTestEntities();
		  }
		});
		
		
		
    });
})();
