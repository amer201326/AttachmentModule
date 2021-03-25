(function () {
    $(function () {

        var _$attachmentEntityTypesTable = $('#AttachmentEntityTypesTable');
        var _attachmentEntityTypesService = abp.services.app.attachmentEntityTypes;
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.AttachmentEntityTypes.Create'),
            edit: abp.auth.hasPermission('Pages.AttachmentEntityTypes.Edit'),
            'delete': abp.auth.hasPermission('Pages.AttachmentEntityTypes.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
                    viewUrl: abp.appPath + 'AppAreaName/AttachmentEntityTypes/CreateOrEditModal',
                    scriptUrl: abp.appPath + 'view-resources/Areas/AppAreaName/Views/AttachmentEntityTypes/_CreateOrEditModal.js',
                    modalClass: 'CreateOrEditAttachmentEntityTypeModal'
                });
                   

		 var _viewAttachmentEntityTypeModal = new app.ModalManager({
            viewUrl: abp.appPath + 'AppAreaName/AttachmentEntityTypes/ViewattachmentEntityTypeModal',
            modalClass: 'ViewAttachmentEntityTypeModal'
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

        var dataTable = _$attachmentEntityTypesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _attachmentEntityTypesService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#AttachmentEntityTypesTableFilter').val(),
					arNameFilter: $('#ArNameFilterId').val(),
					enNameFilter: $('#EnNameFilterId').val(),
					folderFilter: $('#FolderFilterId').val(),
					attachmentEntityTypeArNameFilter: $('#AttachmentEntityTypeArNameFilterId').val()
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
                                    _viewAttachmentEntityTypeModal.open({ id: data.record.attachmentEntityType.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            iconStyle: 'far fa-edit mr-2',
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.attachmentEntityType.id });                                
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            iconStyle: 'far fa-trash-alt mr-2',
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteAttachmentEntityType(data.record.attachmentEntityType);
                            }
                        }]
                    }
                },
					{
						targets: 2,
						 data: "attachmentEntityType.arName",
						 name: "arName"   
					},
					{
						targets: 3,
						 data: "attachmentEntityType.enName",
						 name: "enName"   
					},
					{
						targets: 4,
						 data: "attachmentEntityType.folder",
						 name: "folder"   
					},
					{
						targets: 5,
						 data: "attachmentEntityTypeArName" ,
						 name: "parentTypeFk.arName" 
					}
            ]
        });

        function getAttachmentEntityTypes() {
            dataTable.ajax.reload();
        }

        function deleteAttachmentEntityType(attachmentEntityType) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _attachmentEntityTypesService.delete({
                            id: attachmentEntityType.id
                        }).done(function () {
                            getAttachmentEntityTypes(true);
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

        $('#CreateNewAttachmentEntityTypeButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _attachmentEntityTypesService
                .getAttachmentEntityTypesToExcel({
				filter : $('#AttachmentEntityTypesTableFilter').val(),
					arNameFilter: $('#ArNameFilterId').val(),
					enNameFilter: $('#EnNameFilterId').val(),
					folderFilter: $('#FolderFilterId').val(),
					attachmentEntityTypeArNameFilter: $('#AttachmentEntityTypeArNameFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditAttachmentEntityTypeModalSaved', function () {
            getAttachmentEntityTypes();
        });

		$('#GetAttachmentEntityTypesButton').click(function (e) {
            e.preventDefault();
            getAttachmentEntityTypes();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getAttachmentEntityTypes();
		  }
		});
		
		
		
    });
})();
