(function () {
    $(function () {

        var _$attachmentTypesTable = $('#AttachmentTypesTable');
        var _attachmentTypesService = abp.services.app.attachmentTypes;
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.AttachmentTypes.Create'),
            edit: abp.auth.hasPermission('Pages.AttachmentTypes.Edit'),
            'delete': abp.auth.hasPermission('Pages.AttachmentTypes.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
                    viewUrl: abp.appPath + 'AppAreaName/AttachmentTypes/CreateOrEditModal',
                    scriptUrl: abp.appPath + 'view-resources/Areas/AppAreaName/Views/AttachmentTypes/_CreateOrEditModal.js',
                    modalClass: 'CreateOrEditAttachmentTypeModal'
                });
                   

		 var _viewAttachmentTypeModal = new app.ModalManager({
            viewUrl: abp.appPath + 'AppAreaName/AttachmentTypes/ViewattachmentTypeModal',
            modalClass: 'ViewAttachmentTypeModal'
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

        var dataTable = _$attachmentTypesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _attachmentTypesService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#AttachmentTypesTableFilter').val(),
					arNameFilter: $('#ArNameFilterId').val(),
					enNameFilter: $('#EnNameFilterId').val(),
					minMaxSizeFilter: $('#MinMaxSizeFilterId').val(),
					maxMaxSizeFilter: $('#MaxMaxSizeFilterId').val(),
					allowedExtensionsFilter: $('#AllowedExtensionsFilterId').val(),
					minMaxAttachmentsFilter: $('#MinMaxAttachmentsFilterId').val(),
					maxMaxAttachmentsFilter: $('#MaxMaxAttachmentsFilterId').val(),
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
                                    _viewAttachmentTypeModal.open({ id: data.record.attachmentType.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            iconStyle: 'far fa-edit mr-2',
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.attachmentType.id });                                
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            iconStyle: 'far fa-trash-alt mr-2',
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteAttachmentType(data.record.attachmentType);
                            }
                        }]
                    }
                },
					{
						targets: 2,
						 data: "attachmentType.arName",
						 name: "arName"   
					},
					{
						targets: 3,
						 data: "attachmentType.enName",
						 name: "enName"   
					},
					{
						targets: 4,
						 data: "attachmentType.maxSize",
						 name: "maxSize"   
					},
					{
						targets: 5,
						 data: "attachmentType.allowedExtensions",
						 name: "allowedExtensions"   
					},
					{
						targets: 6,
						 data: "attachmentType.maxAttachments",
						 name: "maxAttachments"   
					},
					{
						targets: 7,
						 data: "attachmentEntityTypeArName" ,
						 name: "entityTypeFk.arName" 
					}
            ]
        });

        function getAttachmentTypes() {
            dataTable.ajax.reload();
        }

        function deleteAttachmentType(attachmentType) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _attachmentTypesService.delete({
                            id: attachmentType.id
                        }).done(function () {
                            getAttachmentTypes(true);
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

        $('#CreateNewAttachmentTypeButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _attachmentTypesService
                .getAttachmentTypesToExcel({
				filter : $('#AttachmentTypesTableFilter').val(),
					arNameFilter: $('#ArNameFilterId').val(),
					enNameFilter: $('#EnNameFilterId').val(),
					minMaxSizeFilter: $('#MinMaxSizeFilterId').val(),
					maxMaxSizeFilter: $('#MaxMaxSizeFilterId').val(),
					allowedExtensionsFilter: $('#AllowedExtensionsFilterId').val(),
					minMaxAttachmentsFilter: $('#MinMaxAttachmentsFilterId').val(),
					maxMaxAttachmentsFilter: $('#MaxMaxAttachmentsFilterId').val(),
					attachmentEntityTypeArNameFilter: $('#AttachmentEntityTypeArNameFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditAttachmentTypeModalSaved', function () {
            getAttachmentTypes();
        });

		$('#GetAttachmentTypesButton').click(function (e) {
            e.preventDefault();
            getAttachmentTypes();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getAttachmentTypes();
		  }
		});
		
		
		
    });
})();
