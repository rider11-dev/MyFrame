var index = {
    mainContainer: $('#main-content'),
    menuLinks: $('.treeview-menu a'),
    loadUrl: function (url) {
        $.get(url, null, function (data) {
            index.mainContainer.html(data);
        });
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
    }
};