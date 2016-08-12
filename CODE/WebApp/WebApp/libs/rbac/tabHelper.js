var tabHelper = {
    /*
    切换tab页：有则聚焦，无则添加
    options:
        tabHeaderId,tab头元素id
        tabContentId,tab内容元素id
        title,tab标题
        tabHeaderContainerId,tab头容器id
        tabContentContainerId，tab内容元素容器id,
        url,tab页内容url
    */
    toggleTab: function (options) {
        if ($('#' + options.tabHeaderId).length > 0) {
            tabHelper.focusTab(options.tabHeaderId);
            return;
        }

        var html = '<li id="' + options.tabHeaderId + '" data-closeable="true">';
        html += '<a href="#' + options.tabContentId + '" data-toggle="tab">' + options.title + '</a>';
        html += '<i class="close-tab glyphicon glyphicon-remove" style="display: none;"></i>';
        html += '</li>';
        $('#' + options.tabHeaderContainerId).append(html);
        html = '<div class="tab-pane tab_page_content" id="' + options.tabContentId + '"></div>';
        $('#' + options.tabContentContainerId).append(html);

        //加载内容
        tabHelper.fillTabContent('#' + options.tabContentId, options.url);//

        //选中tab
        tabHelper.focusTab(options.tabHeaderId);

        //注册事件
        $('#' + options.tabHeaderId)
            .mouseenter(function () {
                $(this).find('.close-tab').show();
            })
            .mouseleave(function () {
                $(this).find('.close-tab').hide();
            })
            .mousedown(function () {
                tabHelper.focusTab(this.id);
            });
        $('#' + options.tabHeaderId).find('.close-tab').click(function () {
            tabHelper.closeTab($(this).parent().attr('id'));
        });
    },
    closeTab: function (tabHeaderId) {
        if ($('#' + tabHeaderId).length < 1 || $('#' + tabHeaderId).attr('data-closeable') == 'false') {
            return;
        }

        var tabContentId = $('#' + tabHeaderId).find('a').attr('href');
        //如果关闭的是当前激活的TAB，激活他的后一个TAB；如果后一个tab不存在，则激活前一个tab
        if ($('#' + tabHeaderId).hasClass('active') == true) {
            var nextTab = $('#' + tabHeaderId).next();
            if (nextTab.length > 0) {
                tabHelper.focusTab(nextTab.attr('id'));
            } else {
                tabHelper.focusTab($('#' + tabHeaderId).prev().attr('id'));
            }
        }
        //关闭当前
        $('#' + tabHeaderId).remove();
        $(tabContentId).remove();

    },
    closePrevTabs: function (tabHeaderId) {
        $('#' + tabHeaderId).prevAll().each(function () {
            tabHelper.closeTab(this.id);
        });
    },
    closeNextTabs: function (tabHeaderId) {
        $('#' + tabHeaderId).nextAll().each(function () {
            tabHelper.closeTab(this.id);
        });
    },
    closeOtherTabs: function (tabHeaderId) {
        $('#' + tabHeaderId).siblings().each(function () {
            tabHelper.closeTab(this.id);
        });
    },
    closeAllTabs: function (tabHeaderId) {
        tabHelper.closeOtherTabs(tabHeaderId);
        tabHelper.closeTab(tabHeaderId);
    },
    focusTab: function (tabHeaderId) {
        if ($('#' + tabHeaderId).length < 1 || $('#' + tabHeaderId).hasClass('active') == true) {
            //alert('already active');
            return;
        }
        var tabContentId = $('#' + tabHeaderId).find('a').attr('href');
        //alert(tabContentId);
        //设置tab选中
        $('#' + tabHeaderId).siblings('li').removeClass('active');
        $(tabContentId).siblings('.tab-pane').removeClass('active');

        $('#' + tabHeaderId).addClass('active');
        $(tabContentId).addClass('active');
    },
    fillTabContent: function (target, url) {
        //alert(url);
        if (gFunc.isNull(url)) {
            return;
        }

        $.get(url, null, function (data) {
            $(target).html(data);
        });
    },
};