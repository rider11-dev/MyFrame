//模态窗口相关
var modalForm = {
    container: $('#modal-container'),
    content: $('#modal-content'),
    title: $('#title'),
    btnSave: $('#btnSaveOfModalForm'),
    onLoadCallback: function () { },
    onSubmitSucceedCallback: function () { },
    submitUrl: '',
    /*
    弹出
    options属性说明：
        title：标题
        contentUrl：内容链接
        contentUrlParams：获取内容时传递的参数
        submitUrl:确定提交的url
        onLoadCallback:窗口加载后回调函数，初始化数据等
        submitSucceedCallback:点击确定提交，成功后的回调函数
    */
    show: function (options) {
        //表单初始化
        $('.modal-title', modalForm.container).html(options.title);
        $('#modal-content', modalForm.container).attr('action', options.contentUrl);
        modalForm.onSubmitSucceedCallback = options.submitSucceedCallback;
        modalForm.onLoadCallback = options.onLoadCallback;
        modalForm.submitUrl = options.submitUrl;
        $.ajax({
            type: "get",
            url: options.contentUrl,
            data: options.contentUrlParams,
            beforeSend: function (XHR) {
                //console.log('modalForm.show.ajax.beforeSend');

            },
            success: function (result, status, XHR) {
                //console.log('modalForm.show.ajax.success');
                //console.log(result);
                modalForm.content.html(result);
                modalForm.container.modal('show');
                //初始化回调函数
                if ($.isFunction(modalForm.onLoadCallback)) {
                    modalForm.onLoadCallback();
                };
                modalForm.registerVal();
            },
            error: function (XHR, status, error) {
                gMessager.error("网络错误：" + status + "\r\n" + error);
            },
            complete: function (XHR, status) {

            }
        });
        modalForm.btnSave.unbind('click');
        modalForm.btnSave.click(modalForm.save);
    },
    /*******注册验证脚本，通过Ajax返回的页面原有MVC属性验证将失效，需要重新注册验证脚本*********/
    registerVal: function () {
        modalForm.content.removeData('validator');
        modalForm.content.removeData('unobtrusiveValidation');
        $.validator.unobtrusive.parse('#modal-content');
    },
    save: function () {
        //var actionUrl = modalForm.content.attr('action');
        if (!modalForm.content.valid()) {
            return;
        }
        $.ajax({
            type: 'post',
            url: modalForm.submitUrl,
            data: modalForm.content.serialize(),
            success: function (result, status, XHR) {
                if (result.code === 0) {
                    gMessager.success(result.message);
                    modalForm.container.modal('hide');
                    if ($.isFunction(modalForm.onSubmitSucceedCallback)) {
                        modalForm.onSubmitSucceedCallback();
                    }
                } else {
                    gMessager.warning(result.message);
                }
            },
            error: function (XHR, status, error) {
                gMessager.error('网络错误，' + error + '，请重新提交！');
            }
        });
    }
}