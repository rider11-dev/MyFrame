var moduleTreeHelp = {
    urlSearchModules: "",
    txtHelpModuleName: $('#txtHelpModuleName'),
    btnModuleHelpSearch: $('#btnModuleHelpSearch'),
    init: function (options) {
        moduleTreeHelp.urlSearchModules = options.urlSearchModules;
        moduleTreeHelp.initModuleTree(options.singleSelect);

        moduleTreeHelp.btnModuleHelpSearch.click(moduleTreeHelp.searchModules);
    },
    initModuleTree: function (singleSelect) {
        treeviewExt.initTree({
            treeId: 'treeModuleHelp',
            dataId: 'id',
            dataField: 'rows',
            singleSelect: singleSelect,
            dataUrl: moduleTreeHelp.urlSearchModules
        });
    },
    searchModules: function () {
        //查询文本
        var name = moduleTreeHelp.txtHelpModuleName.val();
        //alert(name);
        treeviewExt.search(name);
    }
};