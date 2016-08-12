var usersGridHelp = {
    grid: $('#gridHelpUsers'),
    btnUserHelpSearch: $('#btnUserHelpSearch'),
    txtHelpUserName: $('#txtHelpUserName'),
    urlSearch: "",
    init: function (options) {
        usersGridHelp.urlSearch = options.urlSearch;
        usersGridHelp.initgrid();
        usersGridHelp.btnUserHelpSearch.click(usersGridHelp.funcbtnUserHelpSearch);
    },
    initgrid: function () {
        var options = {
            url: usersGridHelp.urlSearch,
            method: 'get',
            dataField: 'rows',
            uniqueId: 'Id',
            queryParams: function (params) {
                //添加额外参数
                if (!gFunc.isNull(usersGridHelp.txtHelpUserName.val())) {
                    params.UserName = usersGridHelp.txtHelpUserName.val();
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
                   { field: 'UserName', title: '用户名', align: 'center', valign: 'center', width: 80 }
            ]
        };
        //调用公共函数，初始化表格
        gFunc.initgrid(usersGridHelp.grid, options);
    },
    funcbtnUserHelpSearch: function () {
        usersGridHelp.grid.bootstrapTable('refresh');
    }
};