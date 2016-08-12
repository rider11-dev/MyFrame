var usermanage = {
    grid: $('#grid_UsrManage'),
    btnSearch: $('#btnSearch_UsrManage'),
    txtSearchUserName: $('#txtSearch_UsrManage'),
    urlAdd: "",
    urlSearch: "",
    urlHelpRoles: "",
    init: function (options) {
        usermanage.urlAdd = options.urlAdd;
        usermanage.urlSearch = options.urlSearch;
        usermanage.urlHelpRoles = options.urlHelpRoles;

        usermanage.initgrid();
        usermanage.bindingEventArgs();
    },
    bindingEventArgs: function () {
        gFuncBusiness.bindEventForRbacButton();
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
                    //{
                    //    field: 'rownumber', formatter: function (value, row, index) {
                    //        return index + 1;
                    //    }
                    //},
                    { field: 'check', checkbox: true },
                    { field: 'UserName', title: '用户名', align: 'center', valign: 'center', width: 80 },
                    { field: 'Email', title: '邮箱', align: 'center', valign: 'center', width: 100 },
                    {
                        field: 'Enabled', title: '是否激活', align: 'center', valign: 'center', width: 80,
                        formatter: gFormatter.trueOrFalse.formatter
                    },
                    {
                        field: 'Roles', title: '角色', align: 'center', valign: 'center', width: 100,
                        cellStyle: function (value, row, index, field) {
                            return {
                                css: { "min-width": "140px" }
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
        gFunc.initgrid(usermanage.grid, options);
    },
    funcBtnAdd: function (options) {
        modalForm.show({
            title: options.tag,
            contentUrl: usermanage.urlAdd,
            contentUrlParams: {},
            submitUrl: options.submitUrl,
            submitSucceedCallback: function () {
                usermanage.grid.bootstrapTable('refresh');
            },
            onLoadCallback: function () {

            }
        });
    },
    funcBtnEdit: function (options) {
        //console.log('funcBtnEdit:' + options.submitUrl);
        var checkedRows = usermanage.grid.bootstrapTable('getSelections');
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
            title: options.tag,
            contentUrl: usermanage.urlAdd,
            contentUrlParams: {},
            submitUrl: options.submitUrl,
            onLoadCallback: function () {
                $('#Id').val(data.Id);
                $('#UserName').val(data.UserName);
                $('#UserName').attr({ 'readonly': 'readonly' });//用户名不能修改
                $('#Email').val(data.Email);
                if (data.Enabled) {
                    $('#Enabled').iCheck('check');
                }
            },
            submitSucceedCallback: function () {
                usermanage.grid.bootstrapTable('refresh');
            }
        });
    },
    funcBtnDelete: function (options) {
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
                    url: options.submitUrl,
                    data: JSON.stringify(usrIds),
                    success: function (result, status, XHR) {
                        if (result.code == 0) {
                            usermanage.grid.bootstrapTable('refresh');
                            gMessager.success('删除成功');
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
    },
    funcBtnSetRoles: function (options) {
        //
        var checkedRows = usermanage.grid.bootstrapTable('getSelections');
        //console.log(checkedRows.length);
        if (checkedRows.length <= 0) {
            gMessager.warning('请选择用户');
            return;
        }
        var usrIds = [];
        $(checkedRows).each(function (index, item) {
            usrIds.push(item.Id);
        });
        modalForm.show({
            title: options.tag,
            contentUrl: usermanage.urlHelpRoles,
            submitUrl: options.submitUrl,
            submitSucceedCallback: function () {
                usermanage.grid.bootstrapTable('refresh');
            },
            onLoadCallback: function () {

            },
            funcGetSubmitParams: function () {
                var data = { cancel: false };
                var rows = rolesGridHelp.grid.bootstrapTable('getSelections');
                if (rows.length < 1) {
                    gMessager.warning("请选择角色");
                    data.cancel = true;
                    return data;
                }
                var roleIds = [];
                $(rows).each(function (index, item) {
                    roleIds.push(item.Id);
                });
                for (var i = 0; i < usrIds.length; i++) {
                    data["usrIds[" + i + "]"] = usrIds[i];
                }
                for (var i = 0; i < roleIds.length; i++) {
                    data["roleIds[" + i + "]"] = roleIds[i];
                }
                return data;
            }
        });
    },
    funcBtnClearRoles: function (options) {
        var checkedRows = usermanage.grid.bootstrapTable('getSelections');
        //console.log(checkedRows.length);
        if (checkedRows.length <= 0) {
            gMessager.warning('请选择用户');
            return;
        }
        var usrIds = [];
        $(checkedRows).each(function (index, item) {
            usrIds.push(item.Id);
        });
        WinMsg.confirm({ message: '确定要清除选中用户的角色吗？' }).on(function (e) {
            if (!e) {
                return;
            }
            //
            $.ajax({
                type: 'post',
                url: options.submitUrl,
                data: JSON.stringify(usrIds),
                success: function (result, status, XHR) {
                    if (result.code == 0) {
                        usermanage.grid.bootstrapTable('refresh');
                        gMessager.info('清除成功');
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
}