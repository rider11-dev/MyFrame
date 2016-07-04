var usermanage = {
    grid: $('#grid'),
    btnAdd: $('#btnAdd'),
    btnEdit: $('#btnEdit'),
    btnDelete: $('#btnDelete'),
    btnSearch: $('#btnSearch'),
    txtSearchUserName: $('#txtSearchUserName'),
    urlAdd: "",
    urlEdit: "",
    urlDelete: "",
    urlSearch: "",
    init: function (options) {
        usermanage.urlAdd = options.urlAdd;
        usermanage.urlEdit = options.urlEdit;
        usermanage.urlDelete = options.urlDelete;
        usermanage.urlSearch = options.urlSearch;

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
            url: usermanage.urlSearch,
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
                    {
                        field: 'Address', title: '地址', align: 'center', valign: 'center', width: 120,
                        cellStyle: function (value, row, index, field) {
                            return {
                                css: { "min-width": "200px" }
                            };
                        }
                    },
                    {
                        field: 'Enabled', title: '是否激活', align: 'center', valign: 'center', width: 80,
                        formatter: gFormatter.trueOrFalse.formatter
                    },
                    {
                        field: 'IsDeleted', title: '是否删除', align: 'center', valign: 'center', width: 80,
                        formatter: gFormatter.trueOrFalse.formatter
                    },
                    { field: 'CreatorName', title: '创建人', align: 'center', valign: 'center', width: 80 },
                    {
                        field: 'CreateTime', title: '创建时间', align: 'center', valign: 'center', width: 100,
                        cellStyle: function (value, row, index, field) {
                            return {
                                css: { "min-width": "140px" }
                            };
                        }
                    },
                    { field: 'LastModifierName', title: '最后修改人', align: 'center', valign: 'center', width: 80 },
                    {
                        field: 'LastModifyTime', title: '最后修改时间', align: 'center', valign: 'center', width: 100,
                        cellStyle: function (value, row, index, field) {
                            return {
                                css: { "min-width": "140px" }
                            };
                        }
                    },
                    {
                        field: 'Remark', title: '备注', align: 'center', valign: 'center', width: 140,
                        cellStyle: function (value, row, index, field) {
                            return {
                                css: { "min-width": "200px" }
                            };
                        }
                    }
            ]
        };
        //调用公共函数，初始化表格
        gFunc.initgrid(usermanage.grid, options);
    },
    funcBtnAdd: function () {
        //alert('hahahx');
        modalForm.show({
            title: '添加用户',
            contentUrl: usermanage.urlAdd,
            contentUrlParams: {},
            submitUrl: usermanage.urlAdd,
            submitSucceedCallback: function () {
                usermanage.grid.bootstrapTable('refresh');
            },
            onLoadCallback: function () {

            }
        });
    },
    funcBtnEdit: function () {
        var checkedRows = usermanage.grid.bootstrapTable('getSelections');
        //console.log(checkedRows.length);
        if (checkedRows.length < 1) {
            gMessager.warning('请选择要修改的数据');
            return;
        }
        if (checkedRows.length > 1) {
            gMessager.warning('请选择一条数据进行修改');
            return;
        }
        var data = checkedRows[0];
        modalForm.show({
            title: '修改用户信息',
            contentUrl: usermanage.urlAdd,
            contentUrlParams: {},
            submitUrl: usermanage.urlEdit,
            onLoadCallback: function () {
                $('#Id').val(data.Id);
                $('#UserName').val(data.UserName);
                $('#UserName').attr({ 'readonly': 'readonly' });//用户名不能修改
                $('#Email').val(data.Email);
                $('#Phone').val(data.Phone);
                $('#Address').val(data.Address);
                if (data.Enabled) {
                    $('#Enabled').iCheck('check');
                }
                if (data.IsDeleted) {
                    $('#IsDeleted').iCheck('check');
                }
                $('#Remark').val(data.Remark);
            },
            submitSucceedCallback: function () {
                usermanage.grid.bootstrapTable('refresh');
            }
        });
    },
    funcBtnDelete: function () {
        var checkedRows = usermanage.grid.bootstrapTable('getSelections');
        //console.log(checkedRows.length);
        if (checkedRows.length < 1) {
            gMessager.warning('请选择要删除的数据');
            return;
        }
        var usrIds = [];
        $(checkedRows).each(function (index, item) {
            //校验
            //入栈
            usrIds.push(item.Id);
        });
        if (usrIds.length < 1) {
            gMessager.warning("没有可删除的数据");
            return;
        }
        if (usrIds.length > 0) {
            WinMsg.confirm({ message: '确定要删除选中的数据吗？' }).on(function (e) {
                if (!e) {
                    return;
                }
                //
                $.ajax({
                    type: 'post',
                    url: usermanage.urlDelete,
                    data: JSON.stringify(usrIds),
                    success: function (result, status, XHR) {
                        if (result.code == 0) {
                            usermanage.grid.bootstrapTable('refresh');
                            gMessager.warning('删除成功');
                        } else {
                            gMessager.warning(result.message);
                        }
                    },
                    error: function (XHR, status, error) {
                        gMessager.error("网络错误：" + status + "\r\n" + error);
                    },
                    complete: function (XHR, status) {
                        //console.log('delete users completed,status:' + status);
                    }
                });
            });
        }
    },
    funcBtnSearch: function () {
        usermanage.grid.bootstrapTable('refresh');
    }
}