(function ($) {
    app.modals.AttachmentEntityTypeLookupTableModal = function () {

        var _modalManager;

        var _attachmentEntityTypesService = abp.services.app.attachmentEntityTypes;
        var _$attachmentEntityTypeTable = $('#AttachmentEntityTypeTable');

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };


        var dataTable = _$attachmentEntityTypeTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _attachmentEntityTypesService.getAllAttachmentEntityTypeForLookupTable,
                inputFilter: function () {
                    return {
                        filter: $('#AttachmentEntityTypeTableFilter').val()
                    };
                }
            },
            columnDefs: [
                {
                    targets: 0,
                    data: null,
                    orderable: false,
                    autoWidth: false,
                    defaultContent: "<div class=\"text-center\"><input id='selectbtn' class='btn btn-success' type='button' width='25px' value='" + app.localize('Select') + "' /></div>"
                },
                {
                    autoWidth: false,
                    orderable: false,
                    targets: 1,
                    data: "displayName"
                }
            ]
        });

        $('#AttachmentEntityTypeTable tbody').on('click', '[id*=selectbtn]', function () {
            var data = dataTable.row($(this).parents('tr')).data();
            _modalManager.setResult(data);
            _modalManager.close();
        });

        function getAttachmentEntityType() {
            dataTable.ajax.reload();
        }

        $('#GetAttachmentEntityTypeButton').click(function (e) {
            e.preventDefault();
            getAttachmentEntityType();
        });

        $('#SelectButton').click(function (e) {
            e.preventDefault();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getAttachmentEntityType();
            }
        });

    };
})(jQuery);

