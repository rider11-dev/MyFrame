var usermanage = {
    grid: $('#grid'),
    btnAdd: $('#btnAdd'),
    btnEdit: $('#btnEdit'),
    btnDelete: $('#btnDelete'),
    btnSearch: $('#btnSearch'),
    txtSearchUserName: $('#txtSearchUserName'),
    init: function () {
        usermanage.initgrid();
        usermanage.bindingEventArgs();
    },
    bindingEventArgs: function () {
        usermanage.btnAdd.click(usermanage.funcBtnAdd);
        usermanage.btnEdit.click(usermanage.funcBtnEdit);
        usermanage.btnDelete.click(usermanage.funcBtnDelete);
        usermanage.btnSearch.click(usermanage.funcBtnSearch);
    },
    initgrid: function () {
        var options = {
            url: '/RBAC/User/GetUsersByPage',
            method: 'get',
            dataField: 'rows',
            height: 400,
            uniqueId: 'Id',
            queryParams: function (params) {
                //添加额外参数
                if (!gFunc.isNull(usermanage.txtSearchUserName.val())) {
                    params.UserName = usermanage.txtSearchUserName.val();
                }
                return params;//必须返回params
            },
            columns: [
                 { field: 'Id', visible: false },
                    {
                        field: 'rownumber', formatter: function (value, row, index) {
                            return index + 1;
                        }
                    },
                    { field: 'check', checkbox: true },
                    { field: 'UserName', title: '用户名', align: 'center', valign: 'center', width: 80 },
                    { field: 'Email', title: '邮箱', align: 'center', valign: 'center', width: 100 },
                    { field: 'Phone', title: '电话', align: 'center', valign: 'center', width: 100 },
                    { field: 'Address', title: '地址', align: 'center', valign: 'center', width: 120 },
                    {
                        field: 'Enabled', title: '是否激活', align: 'center', valign: 'center', width: 80,
                        formatter: gFormatter.trueOrFalse.formatter
                    },
                    {
                        field: 'IsDeleted', title: '是否删除', align: 'center', valign: 'center', width: 80,
                        formatter: gFormatter.trueOrFalse.formatter
                    },
                    { field: 'Creator', title: '创建人', align: 'center', valign: 'center', width: 80 },
                    { field: 'CreateTime', title: '创建时间', align: 'center', valign: 'center', width: 100 },
                    { field: 'LastModifier', title: '最后修改人', align: 'center', valign: 'center', width: 80 },
                    { field: 'LastModifyTime', title: '最后修改时间', align: 'center', valign: 'center', width: 100 },
                    { field: 'Remark', title: '备注', align: 'center', valign: 'center', width: 140 }
            ]
        };
        //调用公共函数，初始化表格
        gFunc.initgrid(usermanage.grid, options);
    },
    funcBtnAdd: function () {
        var checkedRows = usermanage.grid.bootstrapTable('getSelections');
        //console.log(checkedRows.length);
    },
    funcBtnEdit: function () {
        var checkedRows = usermanage.grid.bootstrapTable('getSelections');
        //console.log(checkedRows.length);
        if (checkedRows.length < 1) {
            toastr.warning('请选择要修改的数据');
            return;
        }
        if (checkedRows.length > 1) {
            toastr.warning('请选择一条数据进行修改');
            return;
        }
    },
    funcBtnDelete: function () {
        var checkedRows = usermanage.grid.bootstrapTable('getSelections');
        //console.log(checkedRows.length);
        if (checkedRows.length < 1) {
            toastr.warning('请选择要删除的数据');
            return;
        }

    },
    funcBtnSearch: function () {
        usermanage.grid.bootstrapTable('refresh');
    }
}