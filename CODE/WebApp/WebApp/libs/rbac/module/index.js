var modulemanage = {
    form: $('#form-add-module'),
    grid: $('#grid'),
    btnSearch: $('#btnSearch'),
    txtSearchCode: $('#txtSearchCode'),
    txtSearchName: $('#txtSearchName'),
    btnHelpParentModule: $('#btnHelpParentModule'),
    urlAdd: "",
    urlEdit: "",
    urlDelete: "",
    urlSearch: "",
    urlModuleTreeHelp: "",
    init: function (options) {
        modulemanage.urlAdd = options.urlAdd;
        modulemanage.urlSearch = options.urlSearch;
        modulemanage.urlModuleTreeHelp = options.urlModuleTreeHelp;

        modulemanage.initgrid();
        modulemanage.bindingEventArgs();
    },
    bindingEventArgs: function () {
        gFuncBusiness.bindEventForRbacButton();

        modulemanage.btnSearch.click(modulemanage.funcBtnSearch);

        modulemanage.btnHelpParentModule.click(function () {
            console.log('modulemanage.btnHelpParentModule.click enter');
            modalForm.show({
                title: '模块帮助',
                contentUrl: modulemanage.urlModuleTreeHelp,
                contentUrlParams: {},
                submitUrl: "",
                submitSucceedCallback: function () {

                },
                onLoadCallback: function () {

                }
            });
        });
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
                        field: 'IsSystem', title: '系统模块', align: 'center', valign: 'center', width: 80,
                        formatter: gFormatter.trueOrFalse.formatter
                    },
                    { field: 'ParentName', title: '父级模块', align: 'center', valign: 'center', width: 100 },
                    { field: 'SortOrder', title: '排序号', align: 'center', valign: 'center', width: 100 },
                    {
                        field: 'Enabled', title: '是否激活', align: 'center', valign: 'center', width: 80,
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
    funcBtnAdd: function (options) {
        //alert('hahahx');
        modalForm.show({
            title: options.tag,
            contentUrl: modulemanage.urlAdd,
            contentUrlParams: {},
            submitUrl: options.submitUrl,
            submitSucceedCallback: function () {
                modulemanage.grid.bootstrapTable('refresh');
            },
            onLoadCallback: function () {

            }
        });
    },
    funcBtnEdit: function (options) {
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
            title: options.tag,
            contentUrl: modulemanage.urlAdd,
            contentUrlParams: {},
            submitUrl: options.submitUrl,
            onLoadCallback: function () {
                //console.log(data);
                $('#Id').val(data.Id);
                $('#Code').val(data.Code);
                $('#Code').attr({ 'readonly': 'readonly' });//模块编号不能修改
                $('#Name').val(data.Name);
                $('#LinkUrl').val(data.LinkUrl);
                $('#Icon').val(data.Icon);
                $('#SortOrder').val(data.SortOrder);
                $('#ParentId').val(data.ParentId);
                $('#ParentName').val(data.ParentName);
                if (data.Enabled) {
                    $('#Enabled').iCheck('check');
                }
                if (data.IsSystem) {
                    $('#IsSystem').iCheck('check');
                }
                $('#Remark').val(data.Remark);
                //console.log('LinkUrl:' + $('#Id').val());
            },
            submitSucceedCallback: function () {
                modulemanage.grid.bootstrapTable('refresh');
            }
        });
    },
    funcBtnDelete: function (options) {
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
                    url: options.submitUrl,
                    data: JSON.stringify(moduleIds),
                    success: function (result, status, XHR) {
                        if (result.code == 0) {
                            modulemanage.grid.bootstrapTable('refresh');
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
        modulemanage.grid.bootstrapTable('refresh');
    }
}