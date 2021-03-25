(function () {
    $(function () {

        var _$attachmentFilesTable = $('#AttachmentFilesTable');
        var _attachmentFilesService = abp.services.app.attachmentFiles;
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.AttachmentFiles.Create'),
            edit: abp.auth.hasPermission('Pages.AttachmentFiles.Edit'),
            'delete': abp.auth.hasPermission('Pages.AttachmentFiles.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
                    viewUrl: abp.appPath + 'AppAreaName/AttachmentFiles/CreateOrEditModal',
                    scriptUrl: abp.appPath + 'view-resources/Areas/AppAreaName/Views/AttachmentFiles/_CreateOrEditModal.js',
                    modalClass: 'CreateOrEditAttachmentFileModal'
                });
                   

		 var _viewAttachmentFileModal = new app.ModalManager({
            viewUrl: abp.appPath + 'AppAreaName/AttachmentFiles/ViewattachmentFileModal',
            modalClass: 'ViewAttachmentFileModal'
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

        var dataTable = _$attachmentFilesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _attachmentFilesService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#AttachmentFilesTableFilter').val(),
					physicalNameFilter: $('#PhysicalNameFilterId').val(),
					originalNameFilter: $('#OriginalNameFilterId').val(),
					minSizeFilter: $('#MinSizeFilterId').val(),
					maxSizeFilter: $('#MaxSizeFilterId').val(),
					objectIdFilter: $('#ObjectIdFilterId').val(),
					pathFilter: $('#PathFilterId').val(),
					attachmentTypeArNameFilter: $('#AttachmentTypeArNameFilterId').val()
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
                                    _viewAttachmentFileModal.open({ id: data.record.attachmentFile.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            iconStyle: 'far fa-edit mr-2',
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.attachmentFile.id });                                
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            iconStyle: 'far fa-trash-alt mr-2',
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteAttachmentFile(data.record.attachmentFile);
                            }
                        }]
                    }
                },
					{
						targets: 2,
						 data: "attachmentFile.physicalName",
						 name: "physicalName"   
					},
					{
						targets: 3,
						 data: "attachmentFile.originalName",
						 name: "originalName"   
					},
					{
						targets: 4,
						 data: "attachmentFile.size",
						 name: "size"   
					},
					{
						targets: 5,
						 data: "attachmentFile.objectId",
						 name: "objectId"   
					},
					{
						targets: 6,
						 data: "attachmentFile.path",
						 name: "path"   
					},
					{
						targets: 7,
						 data: "attachmentTypeArName" ,
						 name: "attachmentTypeFk.arName" 
					}
            ]
        });

        function getAttachmentFiles() {
            dataTable.ajax.reload();
        }

        function deleteAttachmentFile(attachmentFile) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _attachmentFilesService.delete({
                            id: attachmentFile.id
                        }).done(function () {
                            getAttachmentFiles(true);
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

        $('#CreateNewAttachmentFileButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _attachmentFilesService
                .getAttachmentFilesToExcel({
				filter : $('#AttachmentFilesTableFilter').val(),
					physicalNameFilter: $('#PhysicalNameFilterId').val(),
					originalNameFilter: $('#OriginalNameFilterId').val(),
					minSizeFilter: $('#MinSizeFilterId').val(),
					maxSizeFilter: $('#MaxSizeFilterId').val(),
					objectIdFilter: $('#ObjectIdFilterId').val(),
					pathFilter: $('#PathFilterId').val(),
					attachmentTypeArNameFilter: $('#AttachmentTypeArNameFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditAttachmentFileModalSaved', function () {
            getAttachmentFiles();
        });

		$('#GetAttachmentFilesButton').click(function (e) {
            e.preventDefault();
            getAttachmentFiles();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getAttachmentFiles();
		  }
		});
		
		
		
    });
})();
