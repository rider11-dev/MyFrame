/*——————————————————全局扩展——————————————————*/
//日期格式化扩展
Date.prototype.Format = function (fmt) {
    var o = {
        "M+": this.getMonth() + 1,                 //月份
        "d+": this.getDate(),                    //日
        "h+": this.getHours(),                   //小时
        "m+": this.getMinutes(),                 //分
        "s+": this.getSeconds(),                 //秒
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度
        "S": this.getMilliseconds()             //毫秒
    };
    if (/(y+)/.test(fmt))
        fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt))
            fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
};

/*——————————————————全局函数——————————————————*/
var gFunc = {
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
            url:字符串，请求后台数据的url（绝对路径）
            method:字符串，请求方式，get/post
            dataField:字符串，查询结果json对象中存放实际数据集的属性名称
            singleSelect:是否单选
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
            url: options.url,//请求后台的URL（*）
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
            sidePagination: gFunc.isNull(options.sidePagination) ? 'server' : options.sidePagination,//分页方式：client客户端分页，server服务端分页（*）
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
            singleSelect: options.singleSelect,//是否单选
            height: options.height,              //行高，如果没有设置height属性，表格自动根据记录条数觉得表格高度
            uniqueId: "Id",                     //每一行的唯一标识，一般为主键列
            //showToggle: true,                    //是否显示详细视图和列表视图的切换按钮
            cardView: false,                    //是否显示详细视图
            detailView: false,                   //是否显示父子表
            maintainSelected: false,
            queryParams: options.queryParams,
            onLoadSuccess: function (data) {
                //console.log(data);
                if (data.code !== 0) {
                    //console.log(data.error);
                    gMessager.warning(data.message);
                }
            },
            onLoadError: function (status, res) {
                gMessager.error('网络错误:' + status);
            },
            columns: options.columns
        });
    },
    setCheckBoxStyle: function (target) {
        if (gFunc.isNull(target)) {
            return;
        }
        $(target).iCheck({
            checkboxClass: 'icheckbox_square-blue',
            radioClass: 'iradio_square-blue',
            increaseArea: '20%' // optional
        });
    },
};

/*——————————————————全局格式化器——————————————————*/
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
    },
    datetime: {
        //参数格式"/Date(1415169703000)/"
        formatter: function (value) {
            if (gFunc.isNull(value)) {
                return null;
            }
            var formatdate = eval(value.replace(/\/Date\((\d+)\)\//gi, "new Date($1)"));
            return formatdate.Format('yyyy-MM-dd');
        }
    }
};

/*——————————————————全局对象——————————————————*/
var gMessager = {
    error: function (message, title) {
        toastr.clear();
        toastr.error(message, title);
    },
    info: function (message, title) {
        toastr.clear();
        toastr.info(message, title);
    },
    success: function (message, title) {
        toastr.clear();
        toastr.success(message, title);
    },
    warning: function (message, title) {
        toastr.clear();
        toastr.warning(message, title);
    }
};

/*——————————————————表单全局函数——————————————————*/
var gFormFunc = {
    // 将Form序列化为JSON对象
    serializeToJson: function (form) {
        if (gFunc.isNull(form)) {
            return null;
        }

        var o = {};
        var a = $(form).serializeArray();
        $.each(a, function () {
            if (o[this.name]) {
                if (!o[this.name].push) {
                    o[this.name] = [o[this.name]];
                }
                o[this.name].push(this.value || '');
            } else {
                o[this.name] = this.value || '';
            }
        });
        return o;
    },
};