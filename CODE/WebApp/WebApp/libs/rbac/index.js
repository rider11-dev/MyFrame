﻿var index = {
    urlGetAccount: "",
    mainContainer: $('#main-content'),
    menuLinks: $('.treeview-menu a'),
    navMainContent: $('#nav-main-content'),
    tabHeaderContainerMain_Id: 'tab_header_container_main',
    contextMenuTabHome_Id: 'context-menu-tab-home',
    init: function (options) {
        index.urlGetAccount = options.urlGetAccount;
        index.initMenuTree();
        index.getAccount();
        index.initContextMenu();
    },
    initMenuTree: function () {
        for (var idx = 0; idx < index.menuLinks.length; idx++) {
            var menuLink = index.menuLinks[idx];
            $(menuLink).on('click', menuLink, function (event) {
                //event.data即传递的menuLink参数
                //console.log($(event.data));
                var link = $(event.data);
                tabHelper.toggleTab({
                    tabHeaderId: 'tab_page_header_' + link.attr('data-module'),
                    tabContentId: 'tab_page_content_' + link.attr('data-module'),
                    title: link.find('span').text(),
                    tabHeaderContainerId: index.tabHeaderContainerMain_Id,
                    tabContentContainerId: 'tab_content_container_main',
                    url: link.attr('href')
                });

                return false;//阻止超链接跳转
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
            gMessager.success("欢迎回来," + data.info.user);
        }, 'json');
    },
    //初始化tab页右键菜单
    initContextMenu: function () {
        $('#' + index.tabHeaderContainerMain_Id).contextmenu({
            target: '#' + index.contextMenuTabHome_Id
        });
        $('#' + index.contextMenuTabHome_Id)
            .find('li')
            .click(function () {
                var currTabId = $('#' + index.tabHeaderContainerMain_Id).find('.active').attr('id');
                if (gFunc.isNull(currTabId)) {
                    return;
                }
                switch ($(this).attr('data-tag')) {
                    case 'close-this':
                        tabHelper.closeTab(currTabId);
                        break;
                    case 'close-prev':
                        tabHelper.closePrevTabs(currTabId);
                        break;
                    case 'close-next':
                        tabHelper.closeNextTabs(currTabId);
                        break;
                    case 'close-other':
                        tabHelper.closeOtherTabs(currTabId);
                        break;
                    case 'close-all':
                        tabHelper.closeAllTabs(currTabId);
                        break;
                    default:
                        break;
                }
            });
    }
};