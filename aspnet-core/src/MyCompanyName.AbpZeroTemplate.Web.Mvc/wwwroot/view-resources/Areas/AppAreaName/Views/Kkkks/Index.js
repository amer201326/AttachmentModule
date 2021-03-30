    $(function () {

        var _$kkkksTable = $('#KkkksTable');
        var _kkkksService = abp.services.app.kkkks;
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Kkkks.Create'),
            edit: abp.auth.hasPermission('Pages.Kkkks.Edit'),
            'delete': abp.auth.hasPermission('Pages.Kkkks.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
                    viewUrl: abp.appPath + 'AppAreaName/Kkkks/CreateOrEditModal',
                    scriptUrl: abp.appPath + 'view-resources/Areas/AppAreaName/Views/Kkkks/_CreateOrEditModal.js',
                    modalClass: 'CreateOrEditKkkkModal'
                });
                   

		 var _viewKkkkModal = new app.ModalManager({
            viewUrl: abp.appPath + 'AppAreaName/Kkkks/ViewkkkkModal',
            modalClass: 'ViewKkkkModal'
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

        var dataTable = _$kkkksTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _kkkksService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#KkkksTableFilter').val(),
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
                                    _viewKkkkModal.open({ id: data.record.kkkk.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            iconStyle: 'far fa-edit mr-2',
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.kkkk.id });                                
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            iconStyle: 'far fa-trash-alt mr-2',
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteKkkk(data.record.kkkk);
                            }
                        }]
                    }
                },
					{
						targets: 2,
						 data: "kkkk.name",
						 name: "name"   
					},
					{
						targets: 3,
						 data: "personname" ,
						 name: "personFk.name" 
					}
            ]
        });

        function getKkkks() {
            dataTable.ajax.reload();
        }

        function deleteKkkk(kkkk) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _kkkksService.delete({
                            id: kkkk.id
                        }).done(function () {
                            getKkkks(true);
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

        $('#CreateNewKkkkButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _kkkksService
                .getKkkksToExcel({
				filter : $('#KkkksTableFilter').val(),
					nameFilter: $('#nameFilterId').val(),
					personnameFilter: $('#PersonnameFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditKkkkModalSaved', function () {
            getKkkks();
        });

		$('#GetKkkksButton').click(function (e) {
            e.preventDefault();
            getKkkks();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getKkkks();
		  }
		});
		
		
		
    });
})();
