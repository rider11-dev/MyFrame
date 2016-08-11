var moduleTreeHelp = {
    urlSearchModules: "",
    txtHelpModuleName: $('#txtModuleName_Help'),
    btnModuleHelpSearch: $('#btnModuleSearch_Help'),
    treeModule: undefined,
    init: function (options) {
        moduleTreeHelp.urlSearchModules = options.urlSearchModules;
        moduleTreeHelp.initModuleTree(options.singleSelect);

        moduleTreeHelp.btnModuleHelpSearch.click(moduleTreeHelp.searchModules);
    },
    initModuleTree: function (singleSelect) {
        moduleTreeHelp.treeModule = treeHelper.create('#treeModule_Help', {
            dataId: 'id',
            dataField: 'rows',
            dataUrl: moduleTreeHelp.urlSearchModules,
            singleSelect: true,
            funcUrlParams: function () {
                return {};
            }
        });
    },
    searchModules: function () {
        //查询文本
        var name = moduleTreeHelp.txtHelpModuleName.val();
        //alert(name);
        moduleTreeHelp.treeModule.search(name);
    }
};