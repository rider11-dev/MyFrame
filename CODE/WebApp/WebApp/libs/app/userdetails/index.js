var idxUsrDetails = {
    editForm: $('#editForm_UserDetails'),
    init: function (options) {
        //出生日期
        $('#date-birthday', idxUsrDetails.editForm).datetimepicker({
            format: 'YYYY-MM-DD',
            showTodayButton: true,
            showClear: true,
            showClose: true,
            widgetPositioning: {
                horizontal: 'left',
                vertical: 'bottom'
            }
        });

        //这里将form的请求改为ajax方式
        idxUsrDetails.editForm.ajaxForm({
            type: 'post',
            url: idxUsrDetails.editForm.attr('action'),
            async: false,
            dataType: 'json',
            success: function (result) {
                if (result && result.code == 0) {
                    gMessager.success('保存成功');
                } else {
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