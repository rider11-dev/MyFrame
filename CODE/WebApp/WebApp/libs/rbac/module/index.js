var modulemanage = {
    grid: $('#grid'),
    btnAdd: $('#btnAdd'),
    btnEdit: $('#btnEdit'),
    btnDelete: $('#btnDelete'),
    btnSearch: $('#btnSearch'),
    txtSearchCode: $('#txtSearchCode'),
    txtSearchName: $('#txtSearchName'),
    urlAdd: "",
    urlEdit: "",
    urlDelete: "",
    urlSearch: "",
    init: function (options) {
        modulemanage.urlAdd = options.urlAdd;
        modulemanage.urlEdit = options.urlEdit;
        modulemanage.urlDelete = options.urlDelete;
        modulemanage.urlSearch = options.urlSearch;

        modulemanage.initgrid();
        modulemanage.bindingEventArgs();
    },
    bindingEventArgs: function () {
        modulemanage.btnAdd.click(modulemanage.funcBtnAdd);
        modulemanage.btnEdit.click(modulemanage.funcBtnEdit);
        modulemanage.btnDelete.click(modulemanage.funcBtnDelete);
        modulemanage.btnSearch.click(modulemanage.funcBtnSearch);
    },
    initgrid: function () {
        var options = {
            url: modulemanage.urlSearch,
            method: 'get',
            dataField: 'rows',
            height: 460,
            uniqueId: 'Id',
            queryParams: function (params) {
                //添加额外参数
                if (!gFunc.isNull(modulemanage.txtSearchCode.val())) {
                    params.Code = modulemanage.txtSearchCode.val();
                }
                if (!gFunc.isNull(modulemanage.txtSearchName.val())) {
                    params.Name = modulemanage.txtSearchName.val();
                }
                return params;//必须返回params
            },
            columns: [
                    { field: 'Id', visible: false },
                    { field: 'ParentId', visible: false },
                    {
                        field: 'rownumber', formatter: function (value, row, index) {
                            return index + 1;
                        }
                    },
                    { field: 'check', checkbox: true },
                    { field: 'Code', title: '模块编号', align: 'center', valign: 'center', width: 80 },
                    { field: 'Name', title: '模块名称', align: 'center', valign: 'center', width: 100 },
                    {
                        field: 'LinkUrl', title: '链接', align: 'center', valign: 'center', width: 120,
                        cellStyle: function (value, row, index, field) {
                            return {
                                css: { "min-width": "120px" }
                            };
                        }
                    },
                    { field: 'Icon', title: '图标', align: 'center', valign: 'center', width: 80 },
                    {
                        field: 'IsMenu', title: '是否菜单', align: 'center', valign: 'center', width: 80,
                        formatter: gFormatter.trueOrFalse.formatter
                    },
                    {
                        field: 'HasChild', title: '包含子模块', align: 'center', valign: 'center', width: 80,
                        formatter: gFormatter.trueOrFalse.formatter
                    },
                    {
                        field: 'IsSystem', title: '系统模块', align: 'center', valign: 'center', width: 80,
                        formatter: gFormatter.trueOrFalse.formatter
                    },
                    { field: 'ParentName', title: '父级模块', align: 'center', valign: 'center', width: 100 },
                    { field: 'SortOrder', title: '排序号', align: 'center', valign: 'center', width: 100 },
                    {
                        field: 'Enabled', title: '是否激活', align: 'center', valign: 'center', width: 80,
                        formatter: gFormatter.trueOrFalse.formatter
                    },
                    {
                        field: 'IsDeleted', title: '已删除', align: 'center', valign: 'center', width: 80,
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
        gFunc.initgrid(modulemanage.grid, options);
    },
    funcBtnAdd: function () {
        //alert('hahahx');
        modalForm.show({
            title: '添加模块',
            contentUrl: modulemanage.urlAdd,
            contentUrlParams: {},
            submitUrl: modulemanage.urlAdd,
            submitSucceedCallback: function () {
                modulemanage.grid.bootstrapTable('refresh');
            },
            onLoadCallback: function () {

            }
        });
    },
    funcBtnEdit: function () {
        var checkedRows = modulemanage.grid.bootstrapTable('getSelections');
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
            title: '修改模块信息',
            contentUrl: modulemanage.urlAdd,
            contentUrlParams: {},
            submitUrl: modulemanage.urlEdit,
            onLoadCallback: function () {
                $('#Id').val(data.Id);
                $('#Code').val(data.Code);
                $('#UserName').attr({ 'readonly': 'readonly' });//模块编号不能修改
                //$('#Email').val(data.Email);
                //$('#Phone').val(data.Phone);
                //$('#Address').val(data.Address);
                if (data.Enabled) {
                    $('#Enabled').iCheck('check');
                }
                if (data.IsDeleted) {
                    $('#IsDeleted').iCheck('check');
                }
                $('#Remark').val(data.Remark);
            },
            submitSucceedCallback: function () {
                modulemanage.grid.bootstrapTable('refresh');
            }
        });
    },
    funcBtnDelete: function () {
        var checkedRows = modulemanage.grid.bootstrapTable('getSelections');
        //console.log(checkedRows.length);
        if (checkedRows.length < 1) {
            gMessager.warning('请选择要删除的数据');
            return;
        }
        var moduleIds = [];
        $(checkedRows).each(function (index, item) {
            //校验
            //入栈
            moduleIds.push(item.Id);
        });
        if (moduleIds.length < 1) {
            gMessager.warning("没有可删除的数据");
            return;
        }
        if (moduleIds.length > 0) {
            WinMsg.confirm({ message: '确定要删除选中的数据吗？' }).on(function (e) {
                if (!e) {
                    return;
                }
                //
                $.ajax({
                    type: 'post',
                    url: modulemanage.urlDelete,
                    data: JSON.stringify(moduleIds),
                    success: function (result, status, XHR) {
                        if (result.code == 0) {
                            modulemanage.grid.bootstrapTable('refresh');
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
        modulemanage.grid.bootstrapTable('refresh');
    }
}