var roles = {
    grid: $('#grid'),
    btnAdd: $('#btnAdd'),
    btnEdit: $('#btnEdit'),
    btnDelete: $('#btnDelete'),
    btnSearch: $('#btnSearch'),
    txtSearchRoleName: $('#txtSearchRoleName'),
    urlAdd: "",
    urlEdit: "",
    urlDelete: "",
    urlSearch: "",
    init: function (options) {
        roles.urlAdd = options.urlAdd;
        roles.urlEdit = options.urlEdit;
        roles.urlDelete = options.urlDelete;
        roles.urlSearch = options.urlSearch;

        roles.initgrid();
        roles.bindingEventArgs();
    },
    bindingEventArgs: function () {
        roles.btnAdd.click(roles.funcBtnAdd);
        roles.btnEdit.click(roles.funcBtnEdit);
        roles.btnDelete.click(roles.funcBtnDelete);
        roles.btnSearch.click(roles.funcBtnSearch);
    },
    initgrid: function () {
        var options = {
            url: roles.urlSearch,
            method: 'get',
            dataField: 'rows',
            height: 400,
            uniqueId: 'Id',
            queryParams: function (params) {
                //添加额外参数
                if (!gFunc.isNull(roles.txtSearchRoleName.val())) {
                    params.RoleName = roles.txtSearchRoleName.val();
                }
                return params;//必须返回params
            },
            columns: [
                    { field: 'Id', visible: false },
                    {
                        field: 'rownumber', formatter: function (value, row, index) {
                            return index + 1;
                        },
                        width: 40
                    },
                    { field: 'check', checkbox: true, width: 40 },
                    { field: 'RoleName', title: '角色名', align: 'center', valign: 'center', width: 80 },
                    { field: 'SortOrder', title: '排序号', align: 'center', valign: 'center', width: 100 },
                    {
                        field: 'Enabled', title: '是否激活', align: 'center', valign: 'center', width: 80,
                        formatter: gFormatter.trueOrFalse.formatter
                    },
                    {
                        field: 'IsDeleted', title: '是否删除', align: 'center', valign: 'center', width: 80,
                        formatter: gFormatter.trueOrFalse.formatter
                    },
                    {
                        field: 'Remark', title: '备注', align: 'center', valign: 'center', width: 140,
                        cellStyle: function (value, row, index, field) {
                            return {
                                css: { "min-width": "200px" }
                            };
                        }
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
                    }
            ]
        };
        //调用公共函数，初始化表格
        gFunc.initgrid(roles.grid, options);
    },
    funcBtnAdd: function () {
        //alert('hahahx');
        modalForm.show({
            title: '添加角色',
            contentUrl: roles.urlAdd,
            contentUrlParams: {},
            submitUrl: roles.urlAdd,
            submitSucceedCallback: function () {
                roles.grid.bootstrapTable('refresh');
            },
            onLoadCallback: function () {

            }
        });
    },
    funcBtnEdit: function () {
        var checkedRows = roles.grid.bootstrapTable('getSelections');
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
            title: '修改角色信息',
            contentUrl: roles.urlAdd,
            contentUrlParams: {},
            submitUrl: roles.urlEdit,
            onLoadCallback: function () {
                $('#Id').val(data.Id);
                $('#RoleName').val(data.RoleName);
                $('#RoleName').attr({ 'readonly': 'readonly' });//角色名不能修改
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
                roles.grid.bootstrapTable('refresh');
            }
        });
    },
    funcBtnDelete: function () {
        var checkedRows = roles.grid.bootstrapTable('getSelections');
        //console.log(checkedRows.length);
        if (checkedRows.length < 1) {
            gMessager.warning('请选择要删除的数据');
            return;
        }
        var roleIds = [];
        $(checkedRows).each(function (index, item) {
            //校验
            //入栈
            roleIds.push(item.Id);
        });
        if (roleIds.length < 1) {
            gMessager.warning("没有可删除的数据");
            return;
        }
        if (roleIds.length > 0) {
            WinMsg.confirm({ message: '确定要删除选中的数据吗？' }).on(function (e) {
                if (!e) {
                    return;
                }
                //
                $.ajax({
                    type: 'post',
                    url: roles.urlDelete,
                    data: JSON.stringify(roleIds),
                    success: function (result, status, XHR) {
                        if (result.code == 0) {
                            roles.grid.bootstrapTable('refresh');
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
        roles.grid.bootstrapTable('refresh');
    }
};