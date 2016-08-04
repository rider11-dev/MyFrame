var optmanage = {
    gridOpt: $('#gridOpt'),
    btnAddOpt: $('#btnAddOpt'),
    btnEditOpt: $('#btnEditOpt'),
    btnDeleteOpt: $('#btnDeleteOpt'),
    btnSearchOpt: $('#btnSearchOpt'),
    txtSearchOptName: $('#txtSearchOptName'),
    btnSearchModules: $('#btnSearchModules'),
    txtSearchModuleName: $('#txtSearchModuleName'),
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
                var moduleIds = treeviewExt.getSelectedData(['id']);
                //console.log(moduleId);
                if (!gFunc.isNull(moduleIds) && moduleIds.length > 0) {
                    params.moduleId = moduleIds[0].id;
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
        optmanage.btnAddOpt.click(optmanage.funcBtnAddOpt);
        optmanage.btnEditOpt.click(optmanage.funcBtnEditOpt);
        optmanage.btnDeleteOpt.click(optmanage.funcBtnDeleteOpt);
        optmanage.btnSearchOpt.click(optmanage.funcBtnSearchOpt);
        optmanage.btnSearchModules.click(optmanage.funcBtnSearchModules);
    },
    funcBtnAddOpt: function () {

    },
    funcBtnEditOpt: function () {

    },
    funcBtnDeleteOpt: function () {

    },
    funcBtnSearchOpt: function () {
        var moduleId = treeviewExt.getSelectedData(['id']);
        optmanage.gridOpt.bootstrapTable('refresh', { url: optmanage.urlSearchOpt });
    },
    funcBtnSearchModules: function () {
        //查询文本
        var name = optmanage.txtSearchModuleName.val();

        var nodes = treeviewExt.tree.treeview('search', [name, {
            ignoreCase: true,     // case insensitive
            exactMatch: false,    // like or equals
            revealResults: true,  // reveal matching nodes
        }]);

    }
};