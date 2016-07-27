var rolesGridHelp = {
    grid: $('#gridHelpRoles'),
    btnHelpSearch: $('#btnHelpSearch'),
    txtHelpRoleName: $('#txtHelpRoleName'),
    urlSearch: "",
    init: function (options) {
        rolesGridHelp.urlSearch = options.urlSearch;
        rolesGridHelp.initgrid();
        rolesGridHelp.btnHelpSearch.click(rolesGridHelp.funcbtnHelpSearch);
    },
    initgrid: function () {
        var options = {
            url: rolesGridHelp.urlSearch,
            method: 'get',
            dataField: 'rows',
            height: 160,
            uniqueId: 'Id',
            queryParams: function (params) {
                //添加额外参数
                if (!gFunc.isNull(rolesGridHelp.txtHelpRoleName.val())) {
                    params.RoleName = rolesGridHelp.txtHelpRoleName.val();
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
        gFunc.initgrid(rolesGridHelp.grid, options);
    },
    funcbtnHelpSearch: function () {
        rolesGridHelp.grid.bootstrapTable('refresh');
    }
}