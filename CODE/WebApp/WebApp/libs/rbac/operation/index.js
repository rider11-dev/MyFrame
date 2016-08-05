var optmanage = {
    gridOpt: $('#gridOpt'),
    btnAddOpt: $('#btnAddOpt'),
    btnEditOpt: $('#btnEditOpt'),
    btnDeleteOpt: $('#btnDeleteOpt'),
    btnSearchOpt: $('#btnSearchOpt'),
    txtSearchOptName: $('#txtSearchOptName'),
    btnExpandModules: $('#btnExpandModules'),
    btnSearchModules: $('#btnSearchModules'),
    txtSearchModuleName: $('#txtSearchModuleName'),

    strFormAddOpt: '#form-add-opt',
    strCardModuleId: '#ModuleId',
    strCardModuleName: '#ModuleName',

    urlAddOpt: "",
    urlEditOpt: "",
    urlDeleteOpt: "",
    urlSearchOpt: "",
    urlSearchModule: "",
    init: function (options) {
        optmanage.urlAddOpt = options.urlAddOpt;
        optmanage.urlEditOpt = options.urlEditOpt;
        optmanage.urlDeleteOpt = options.urlDeleteOpt;
        optmanage.urlSearchOpt = options.urlSearchOpt;
        optmanage.urlSearchModule = options.urlSearchModule;

        optmanage.initModuleTree();
        optmanage.initGridOpt();
        optmanage.bindEvent();
    },
    initModuleTree: function () {
        //console.log('initModuleTree');
        treeviewExt.initTree({
            treeId: 'treeModule',
            dataId: 'id',
            dataField: 'rows',
            dataUrl: optmanage.urlSearchModule,
            singleSelect: true,
            funcUrlParams: function () {
                return {};
            }
        });
    },
    initGridOpt: function () {
        var options = {
            url: '',// 设置为空，禁止一开始就加载，因为没有指定moduleId
            method: 'get',
            dataField: 'rows',
            height: 400,
            uniqueId: 'Id',
            queryParams: function (params) {
                //添加额外参数
                //1、moduleId
                var optIds = treeviewExt.getSelectedData(['id']);
                //console.log(moduleId);
                if (!gFunc.isNull(optIds) && optIds.length > 0) {
                    params.moduleId = optIds[0].id;
                }
                //2、
                if (!gFunc.isNull(optmanage.txtSearchOptName.val())) {
                    params.OptName = optmanage.txtSearchOptName.val();
                }
                return params;//必须返回params
            },
            columns: [
                { field: 'Id', visible: false },
                { field: 'ModuleId', visible: false },
                {
                    field: 'rownumber', formatter: function (value, row, index) {
                        return index + 1;
                    }
                },
                { field: 'check', checkbox: true },
                { field: 'OptCode', title: '操作编号', align: 'center', valign: 'center', width: 80 },
                { field: 'OptName', title: '操作名称', align: 'center', valign: 'center', width: 80 },
                { field: 'ModuleName', title: '所属模块', align: 'center', valign: 'center', width: 80 },
                { field: 'Icon', title: '图标', align: 'center', valign: 'center', width: 80 },
                { field: 'CssClass', title: 'Css类', align: 'center', valign: 'center', width: 100 },
                { field: 'CssStyle', title: 'Css样式', align: 'center', valign: 'center', width: 100 },
                {
                    field: 'SubmitUrl', title: '操作链接', align: 'center', valign: 'center', width: 120,
                    cellStyle: function (value, row, index, field) {
                        return {
                            css: { "min-width": "200px" }
                        };
                    }
                },
                { field: 'SortOrder', title: '排序号', align: 'center', valign: 'center', width: 80 },
                {
                    field: 'Enabled', title: '是否激活', align: 'center', valign: 'center', width: 80,
                    formatter: gFormatter.trueOrFalse.formatter
                },
                {
                    field: 'Remark', title: '备注', align: 'center', valign: 'center', width: 140,
                    cellStyle: function (value, row, index, field) {
                        return {
                            css: { "min-width": "200px" }
                        };
                    }
                }
            ],
        };
        //调用公共函数，初始化表格
        gFunc.initgrid(optmanage.gridOpt, options);
    },
    bindEvent: function () {
        optmanage.btnAddOpt.click(optmanage.funcAddOpt);
        optmanage.btnEditOpt.click(optmanage.funcEditOpt);
        optmanage.btnDeleteOpt.click(optmanage.funcDeleteOpt);
        optmanage.btnSearchOpt.click(optmanage.funcSearchOpt);
        optmanage.btnSearchModules.click(optmanage.funcSearchModules);
        optmanage.btnExpandModules.click(optmanage.funcExpandModules);

        treeviewExt.tree.on('nodeSelected', function (event, data) {
            //console.log(data);
            if (gFunc.isNull(data)) {
                return;
            }
            optmanage.funcSearchOpt();
        });
    },
    funcAddOpt: function () {
        var node = treeviewExt.getSelectedData(['id', 'text']);
        if (gFunc.isNull(node) || node.length < 1) {
            gMessager.warning('请选择模块');
            return;
        }
        modalForm.show({
            title: '添加操作',
            contentUrl: optmanage.urlAddOpt,
            contentUrlParams: {},
            submitUrl: optmanage.urlAddOpt,
            submitSucceedCallback: function () {
                optmanage.funcSearchOpt();
            },
            onLoadCallback: function () {
                $(optmanage.strCardModuleId, optmanage.strFormAddOpt).val(node[0]['id']);
                $(optmanage.strCardModuleName, optmanage.strFormAddOpt).val(node[0]['text']);
            }
        });
    },
    funcEditOpt: function () {
        var checkedRows = optmanage.gridOpt.bootstrapTable('getSelections');
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
            title: '修改操作信息',
            contentUrl: optmanage.urlAddOpt,
            contentUrlParams: {},
            submitUrl: optmanage.urlEditOpt,
            onLoadCallback: function () {
                console.log(data);
                $('#Id').val(data.Id);
                $('#OptCode', optmanage.strFormAddOpt).val(data.OptCode);
                $('#OptCode', optmanage.strFormAddOpt).attr({ 'readonly': 'readonly' });//操作编号不能修改
                $('#OptName', optmanage.strFormAddOpt).val(data.OptName);
                $('#SubmitUrl', optmanage.strFormAddOpt).val(data.SubmitUrl);
                $('#Icon', optmanage.strFormAddOpt).val(data.Icon);
                $('#CssClass', optmanage.strFormAddOpt).val(data.CssClass);
                $('#CssStyle', optmanage.strFormAddOpt).val(data.CssStyle);
                $(optmanage.strCardModuleId, optmanage.strFormAddOpt).val(data.ModuleId);
                $(optmanage.strCardModuleName, optmanage.strFormAddOpt).val(data.ModuleName);
                $('#SortOrder', optmanage.strFormAddOpt).val(data.SortOrder);
                if (data.Enabled) {
                    $('#Enabled', optmanage.strFormAddOpt).iCheck('check');
                }
                $('#Remark', optmanage.strFormAddOpt).val(data.Remark);
            },
            submitSucceedCallback: function () {
                optmanage.funcSearchOpt();
            }
        });
    },
    funcDeleteOpt: function () {
        var checkedRows = optmanage.gridOpt.bootstrapTable('getSelections');
        //console.log(checkedRows.length);
        if (checkedRows.length < 1) {
            gMessager.warning('请选择要删除的数据');
            return;
        }
        var optIds = [];
        $(checkedRows).each(function (index, item) {
            //校验
            //入栈
            optIds.push(item.Id);
        });
        if (optIds.length < 1) {
            gMessager.warning("没有可删除的数据");
            return;
        }
        if (optIds.length > 0) {
            WinMsg.confirm({ message: '确定要删除选中的数据吗？' }).on(function (e) {
                if (!e) {
                    return;
                }
                //
                $.ajax({
                    type: 'post',
                    url: optmanage.urlDeleteOpt,
                    data: JSON.stringify(optIds),
                    success: function (result, status, XHR) {
                        if (result.code == 0) {
                            optmanage.funcSearchOpt();
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
    funcSearchOpt: function () {
        optmanage.gridOpt.bootstrapTable('refresh', { url: optmanage.urlSearchOpt });
    },
    funcSearchModules: function () {
        //查询文本
        var name = optmanage.txtSearchModuleName.val();
        treeviewExt.search(name);
    },
    funcExpandModules: function () {
        var treeStatus = optmanage.btnExpandModules.attr('data-status');
        //console.log(treeStatus);
        if (treeStatus == 'expanded') {
            treeviewExt.collapseAll();
            optmanage.btnExpandModules.attr('data-status', 'collapsed');
        } else {
            treeviewExt.expandAll();
            optmanage.btnExpandModules.attr('data-status', 'expanded');
        }
    }
};