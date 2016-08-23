var avatar = {
    imgSrcFilePath: '',
    cImage: undefined,
    imgHeadSrc: $('#img-headSrc'),
    /*
    options:
        urlUpload:源图文件上传url
        urlCutAvatar:头像裁剪url
    */
    init: function (options) {
        //1、初始化文件上传组件
        var uFile = new FileUpload();
        //console.log(uFile);
        uFile.init({
            element: '#input_uploadFile',
            uploadUrl: options.urlUpload,
            uploadSuccess: function (data) {
                //console.log(data.filepath);
                //console.log(data.folder);

                avatar.imgSrcFilePath = data.filepath;
                var imgSrc = data.filepath + '?' + Math.random();//抵消缓存
                avatar.imgHeadSrc.attr('src', imgSrc);
                $('#src-container img').attr('src', imgSrc);
                $('#img-headPreview').attr('src', imgSrc);

                avatar.cImage = new ImageCrop();
                avatar.cImage.init({
                    prevContainer: '#preview-container',
                    imgSrc: avatar.imgHeadSrc
                });
            }
        });

        $('#btn_uploadFile').click(function () {
            $("#modal_UploadFile").modal();
        });

        //2、初始化图片裁剪组件
        if (avatar.imgHeadSrc.attr('src')) {
            avatar.imgSrcFilePath = avatar.imgHeadSrc.attr('src');
            avatar.cImage = new ImageCrop();
            avatar.cImage.init({
                prevContainer: '#preview-container',
                imgSrc: avatar.imgHeadSrc
            });
        }


        //3、上传裁剪参数
        $('#btn_uploadHead').click(function () {
            //console.log(avatar.cImage.cutInfo);
            //console.log(avatar.imgSrcFilePath);
            if (!avatar.cImage.cutInfo.w || avatar.cImage.cutInfo.w == 0) {
                gMessager.warning('请选择裁剪区域');
                return;
            }
            $.ajax({
                url: options.urlCutAvatar,
                type: 'post',
                async: false,
                data: {
                    x: avatar.cImage.cutInfo.x,
                    y: avatar.cImage.cutInfo.y,
                    w: avatar.cImage.cutInfo.w,
                    h: avatar.cImage.cutInfo.h,
                    srcClientWidth: avatar.imgHeadSrc.width(),
                    srcClientHeight: avatar.imgHeadSrc.height(),
                    imgSrcFilePath: avatar.imgSrcFilePath
                },
                dataType: 'json',
                success: function (data, textStatus, jqXHR) {
                    // data 可能是 xmlDoc, jsonObj, html, text, 等等...
                    //console.log(data);
                    if (data.code === 0) {
                        gMessager.success('头像裁剪成功');
                    } else {
                        gMessager.warning(data.message);
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    // 通常 textStatus 和 errorThrown 之中
                    // 只有一个会包含信息
                    gMessager.error('网络错误：' + textStatus);
                }
            });
        });
    }
}