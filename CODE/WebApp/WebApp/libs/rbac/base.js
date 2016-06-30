﻿var gFunc = {
    isNull: function (value) {
        return typeof (value) == 'undefined' || value == null;
    },
    getRootPath: function () {
        //获取当前网址，如： http://localhost:8083/uimcardprj/share/meun.jsp
        var curWwwPath = window.document.location.href;
        //获取主机地址之后的目录，如： uimcardprj/share/meun.jsp
        var pathName = window.document.location.pathname;
        var pos = curWwwPath.indexOf(pathName);
        //获取主机地址，如： http://localhost:8083
        var localhostPaht = curWwwPath.substring(0, pos);
        //获取带"/"的项目名，如：/uimcardprj
        var projectName = pathName.substring(0, pathName.substr(1).indexOf('/') + 1);
        return (localhostPaht + projectName);
    },
    /*
    初始化列表：使用bootstraptable组件
    参数：
        grid：列表，可以#id,也可以直接传dom元素值或jquery对象
        options：
            url:字符串，请求后台数据的url（相对网站根目录的路径）
            method:字符串，请求方式，get/post
            dataField:字符串，查询结果json对象中存放实际数据集的属性名称
            height:整数值，高度
            uniqueId:字符串，每一行的唯一标识，一般为主键列
            queryParams:函数，如function (params) {
                                    params.username = '查询条件';//添加额外参数
                                    return params;//必须返回params
                                }
            columns:列配置对象数组，[{ field: 'Id', visible: false },{ field: 'Code' }]
    */
    initgrid: function (grid, options) {
        $(grid).bootstrapTable({
            url: gFunc.getRootPath() + options.url,//请求后台的URL（*）
            method: options.method,                      //请求方式（*）
            //toolbar: '#toolbar',                //工具按钮用哪个容器
            striped: true,                      //是否显示行间隔色
            cache: false,                       //是否使用缓存，默认为true，所以一般情况下需要设置一下这个属性（*）
            sortable: false,                     //是否启用排序
            sortOrder: "asc",                   //排序方式
            dataField: options.dataField,//数据源key
            contentType: 'application/json',
            dataType: 'json',
            pagination: true,                   //是否显示分页（*）
            queryParamsType: 'pageSize',//设置为非'limit',查询时，传递pageNumber，pageSize
            sidePagination: "server",           //分页方式：client客户端分页，server服务端分页（*）
            pageNumber: 1,                       //初始化加载第一页，默认第一页
            pageSize: 10,                       //每页的记录行数（*）
            pageList: [10, 25, 50, 100],        //可供选择的每页的行数（*）
            showHeader: true,
            //toolbarAlign: 'left',
            search: false,                       //是否显示表格搜索，此搜索是客户端搜索，不会进服务端
            searchOnEnterKey: true,
            strictSearch: true,
            //showColumns: true,                  //是否显示所有的列
            //showRefresh: true,                  //是否显示刷新按钮
            clickToSelect: true,                //是否启用点击选中行
            height: options.height,              //行高，如果没有设置height属性，表格自动根据记录条数觉得表格高度
            uniqueId: "Id",                     //每一行的唯一标识，一般为主键列
            //showToggle: true,                    //是否显示详细视图和列表视图的切换按钮
            cardView: false,                    //是否显示详细视图
            detailView: false,                   //是否显示父子表
            queryParams: options.queryParams,
            onLoadSuccess: function (data) {
                //console.log(data);
                if (data.code !== 0) {
                    //console.log(data.error);
                    toastr.warning(data.error);
                }
            },
            onLoadError: function (status, res) {
                toastr.error('网络错误:' + status);
            },
            columns: options.columns
        });
    },
};

var gFormatter = {
    trueOrFalse: {
        formatter: function (value) {
            if (value == 0 || value == '0' || value == 'false' || value == 'False' || value == 'FALSE' || value == false) {
                return '否';
            } else if (value == 1 || value == '1' || value == 'true' || value == 'True' || value == 'TRUE' || value == true) {
                return '是';
            } else {
                return '未知';
            }
        },
        parser: function (text) {
            if (text == '是') {
                return true;
            } else if (text == '否') {
                return false;
            } else {
                return null;
            }
        }
    }
};