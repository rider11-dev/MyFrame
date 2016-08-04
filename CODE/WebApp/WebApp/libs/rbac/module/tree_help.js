var moduleTreeHelp = {
    urlSearchModules: "",
    txtSearchModuleName: $('#txtSearchModuleName'),
    btnSearchModules: $('#btnSearchModules'),
    init: function (options) {
        moduleTreeHelp.urlSearchModules = options.urlSearchModules;
        moduleTreeHelp.initModuleTree(options.singleSelect);

        moduleTreeHelp.btnSearchModules.click(moduleTreeHelp.searchModules);
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
        var name = moduleTreeHelp.txtSearchModuleName.val();
        var nodes = treeviewExt.tree.treeview('search', [name, {
            ignoreCase: true,     // case insensitive
            exactMatch: false,    // like or equals
            revealResults: true,  // reveal matching nodes
        }]);
    }
};