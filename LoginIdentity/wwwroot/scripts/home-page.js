var ConsignmentsList = function (scope, options) {
    var $scope = $(scope);

    var $filterForm = null;
    var $pageField = null;
    var $sortWayField = null;
    var $sortColumnField = null;

    var $datatable = null;

    this.init = function () {
        this.initForm();
        this.initDatatable();
    }

    this.initForm = function () {
        var _this = this;
        $filterForm = $(options.filterForm, _this.$scope);

        $pageField = $('input[name="Page"]', $filterForm);
        $sortWayField = $('input[name="OrderWay"]', $filterForm);
        $sortColumnField = $('input[name="SortColumn"]', $filterForm);

        new RewriteFilterForm($filterForm, function (data) {
            options = $.extend({}, options, data);
            debugger;
            $datatable.options.toolbar.items.pagination = $.extend({}
                , $datatable.options.toolbar.items.pagination
                , {
                    totalItems: options.totalItems,
                    totalPages: options.totalPages,
                    currentPage: options.currentPage,
                    pageSizeSelect: false
                });

            $datatable.dataSet = $datatable.originalDataSet = $datatable.dataMapCallback(options.items);

            $datatable.dataRender();

        }).init();
    }

    this.initDatatable = function () {
        var $table = $(options.table, $scope);
        if (!$table.length) return;

        $datatable = $table.KTDatatable({
            data: {
                type: 'local',
                source: options.items,
                serverPaging: true,
                serverFiltering: true,
                serverSorting: true,
                pageSize: options.pageSize,
            },
            rows: {
                afterTemplate: function (row, data, index) {

                    row.addClass('data-id-' + data.id);
                }
            },
            layout: {
                icons: {
                    sort: {
                        asc: 'fa fa-caret-up',
                        desc: 'fa fa-caret-down'
                    },
                    pagination: {
                        next: 'icon angle-right',
                        prev: 'icon angle-left',
                        first: 'icon double-left',
                        last: 'icon double-right',
                        more: 'la la-ellipsis-h'
                    }
                }
            },
            skipDataRenderOnClick: true,
            pagination: true,
            pagingCallback: function (ctx, meta) {
                debugger;
                $pageField.val(meta.page);
                $filterForm.submit();
            },
            toolbar: {
                items: {
                    pagination: {
                        totalItems: options.totalItems,
                        totalPages: options.totalPages,
                        currentPage: options.currentPage,
                        pageSizeSelect: false
                    }
                }
            },
            columns: [{
                field: 'id',
                title: 'Id',
                template: function (row) {
                    return row.id;
                }
            }, {
                field: 'storeName',
                title: 'Store Name',
                template: function (row) {
                    return row.storeName;
                }
            }]
        }).on('datatable-on-sort', function (ctx, meta) {
            $sortColumnField.val(meta.field);
            $sortWayField.val(meta.sort);
            $filterForm.submit();
        }).on('datatable-on-layout-updated', function () {
            var $page = $('.datatable-pager');
            
        })

        $datatable.setDataSourceParam('sort', {
            field: options.sortColumn,
            sort: options.orderWay
        });
        debugger;
        setTimeout(function () {
            var $pageSize = $('.datatable-pager-size');
            $pageSize.hide();
        }, 200);

        
        
    }
}