var changepwd = {
    btnSubmit: $('#btnChangePwd_Submit'),
    editForm: $('#editForm_ChangePwd'),
    init: function (options) {
        //changepwd.btnSubmit.click(changepwd.submit);
        $('#btnChangePwd_GetVerifyCode').click(function () {
            var email = $('#VerifyCodeEmail', changepwd.editForm).val();
            $.ajax({
                type: 'post',
                url: options.urlGetVerifyCode,
                data: { email: email },
                async: false,
                dataType: 'json',
                success: function (result) {
                    if (result && result.code == 0) {
                        //console.log('changepwd,ajax succeed');
                        gMessager.success(result.message);
                    } else {
                        //console.log('changepwd,ajax fail');
                        gMessager.warning(result.message);
                    }
                },
                error: function () {
                    gMessager.error("网络错误");
                }
            });
        });

        //这里将form的请求改为ajax方式
        changepwd.editForm.ajaxForm({
            type: 'post',
            url: changepwd.editForm.attr('action'),
            //data: {},//data这里是额外参数
            async: false,
            dataType: 'json',
            success: function (result) {
                if (result && result.code == 0) {
                    //console.log('changepwd,ajax succeed');
                    gMessager.success('密码修改成功');
                    changepwd.editForm.resetForm();
                } else {
                    //console.log('changepwd,ajax fail');
                    gMessager.warning(result.message);
                }
            },
            error: function () {
                //console.log('changepwd,ajax error');
                gMessager.error("网络错误");
            }
        });
    }
};