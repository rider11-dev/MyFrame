var index = {
    urlGetAccount: "",
    mainContainer: $('#main-content'),
    menuLinks: $('.treeview-menu a'),
    loadUrl: function (url) {
        $.get(url, null, function (data) {
            index.mainContainer.html(data);
        });
    },
    init: function (options) {
        index.urlGetAccount = options.urlGetAccount;
        index.initMenuTree();
        index.getAccount();
    },
    initMenuTree: function () {
        for (var idx = 0; idx < index.menuLinks.length; idx++) {
            var menuLink = index.menuLinks[idx];
            $(menuLink).on('click', menuLink, function (event) {
                //console.log('click begin');
                //event.data即传递的menuLink参数
                var url = $(event.data).attr('href');
                //console.log(url);
                if (url && url !== '#') {
                    //console.log( url);
                    index.loadUrl(url);
                }
                return false;//组织超链接跳转
            });
        }
    },
    getAccount: function () {
        $.get(index.urlGetAccount, null, function (data, status, xhr) {
            if (gFunc.isNull(data.info)) {
                return;
            }
            //console.log(data);
            $(".lblUserName").text(data.info.user);
            $(".lblRoleName").text(data.info.role);
        }, 'json');
    }
};