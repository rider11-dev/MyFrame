//模态帮助相关
var modalFormHelp = {
    container: $('#help-modal-container'),
    content: $('#help-modal-content'),
    btnModalHelpOK: $('#btnModalHelpOK'),
    /*
    弹出
    options属性说明：
        title：标题
        urlHelp：帮助链接
        urlParams：获取内容时传递的参数
        onSubmitCallback:确定提交的回调函数，需要返回true或false来通知模态帮助是否关闭（隐藏）
        onLoadCallback:窗口加载后的回调函数，初始化数据等
    */
    show: function (options) {
        $('.modal-title', modalFormHelp.container).html(options.title);

        modalFormHelp.btnModalHelpOK.click(function () {
            if (options.onSubmitCallback && $.isFunction(options.onSubmitCallback)) {
                var rst = options.onSubmitCallback();
                //console.log(rst);
                if (!rst) {
                    return;
                }
            }
            modalFormHelp.container.modal('hide');
        });

        $.ajax({
            type: "get",
            url: options.urlHelp,
            data: options.urlParams,
            beforeSend: function (XHR) {
                //console.log('modalFormHelp.show.ajax.beforeSend');

            },
            success: function (result, status, XHR) {
                //console.log('modalFormHelp.show.ajax.success');
                //console.log(result);
                modalFormHelp.content.html(result);
                modalFormHelp.container.modal('show');
                //初始化回调函数
                if (options.onLoadCallback && $.isFunction(options.onLoadCallback)) {
                    options.onLoadCallback();
                };
            },
            error: function (XHR, status, error) {
                gMessager.error("网络错误：" + status + "\r\n" + error);
            },
            complete: function (XHR, status) {

            }
        });
    }
};