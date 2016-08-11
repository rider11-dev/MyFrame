var authmanage = {
    gridRoles: $('#gridRoles_AuthManage'),
    treeModules: undefined,
    gridOpts: $('#gridOpts_AuthManage'),
    urlSearchRoles: "",
    urlSearchModules: "",
    urlSearchOpts: "",
    urlGetPermission: "",

    txtSearchRoleName: $('#txtSearchRoleName'),
    btnSearchRoles: $('#btnSearchRoles'),

    cacheOpts: [],
    cacheRolePers: {//结构：roleId的值作为属性名称，其对应的模块权限+操作权限数组作为属性值([Object { RoleId=4,  PermissionId=12,  PerType=0,Id=12}, Object { RoleId=4,  PermissionId=23,  PerType=0,Id=13})
        set: function (key, val) {
            this['' + key + ''] = val;
        },
        get: function (key) {
            return this['' + key + ''];
        }
    },

    init: function (options) {
        //设置全局变量
        authmanage.urlSearchRoles = options.urlSearchRoles;
        authmanage.urlSearchModules = options.urlSearchModules;
        authmanage.urlSearchOpts = options.urlSearchOpts;
        authmanage.urlSavePermission = options.urlSavePermission;
        authmanage.urlSaveAllPermission = options.urlSaveAllPermission;

        authmanage.urlGetPermission = options.urlGetPermission;
        //加载所有操作信息
        authmanage.getAllOpts();
        //初始化列表、树
        authmanage.initRolesGrid();
        authmanage.initTreeModules();
        authmanage.initOptsGrid();
        //绑定事件
        authmanage.bindEvents();
    },
    initRolesGrid: function () {
        var options = {
            url: authmanage.urlSearchRoles,
            method: 'get',
            dataField: 'rows',
            height: 500,
            uniqueId: 'Id',
            queryParams: function (params) {
                //添加额外参数
                if (!gFunc.isNull(authmanage.txtSearchRoleName.val())) {
                    params.RoleName = authmanage.txtSearchRoleName.val();
                }
                return params;//必须返回params
            },
            columns: [
                   { field: 'Id', visible: false },
                   {
                       field: 'rownumber', formatter: function (value, row, index) {
                           return index + 1;
                       },
                       width: 40
                   },
                   {
                       field: 'check', radio: true, width: 40
                   },
                   {
                       field: 'RoleName', title: '角色名', align: 'center', valign: 'center', width: 100
                   }
            ]
        };
        //调用公共函数，初始化表格
        gFunc.initgrid(authmanage.gridRoles, options);
        //注册列表事件
        authmanage.gridRoles.on('check.bs.table', function (e, row) {
            var roleId = row.Id;
            if (gFunc.isNull(roleId)) {
                return;
            }
            //从缓存加载角色模块权限，无则通过http获取
            if (authmanage.cacheRolePers.length < 1 || gFunc.isNull(authmanage.cacheRolePers.get(roleId))) {
                authmanage.getRolePermissions(roleId);
            }

            //1、然后设置模块树选中状态
            authmanage.setModulesChecked();

            //2、设置操作选中状态
            authmanage.setOptsChecked();
        });
    },
    initTreeModules: function () {
        authmanage.treeModules = treeHelper.create('#treeModule_AuthManage', {
            dataId: 'id',
            dataField: 'rows',
            dataUrl: authmanage.urlSearchModules
        });
        //选择模块时，加载其对应的操作列表
        authmanage.treeModules.tree.on('nodeSelected', function (event, data) {
            //console.log(data);// Object { id="11",  text="用户管理",  sort=10,  更多...}
            if (gFunc.isNull(data)) {
                return;
            }

            var opts = $.grep(authmanage.cacheOpts, function (item, idx) { return item.ModuleId == data.id });
            //console.log(opts);
            //重新加载列表数据：bootstraptable的sidePagination必须是client，否则load无效
            authmanage.gridOpts.bootstrapTable('load', opts);

            authmanage.setOptsChecked();
        });
    },
    initOptsGrid: function () {
        var options = {
            //url: '',
            //method: 'get',
            //dataField: 'rows',
            height: 500,
            uniqueId: 'Id',
            sidePagination: 'client',//设置客户端分页，否则load方法无效
            //queryParams: function (params) {
            //    //添加额外参数
            //    //1、moduleId
            //    var optIds = authmanage.treeModules.getSelectedData(['id']);
            //    //console.log(moduleId);
            //    if (!gFunc.isNull(optIds) && optIds.length > 0) {
            //        params.moduleId = optIds[0].id;
            //    }
            //    //2、
            //    if (!gFunc.isNull(authmanage.txtSearchOptName.val())) {
            //        params.OptName = authmanage.txtSearchOptName.val();
            //    }
            //    return params;//必须返回params
            //},
            columns: [
                   { field: 'Id', visible: false },
                   {
                       field: 'rownumber', formatter: function (value, row, index) {
                           return index + 1;
                       },
                       width: 40
                   },
                   {
                       field: 'check', checkbox: true, width: 40
                   },
                   { field: 'OptCode', title: '操作编号', align: 'center', valign: 'center', width: 100 },
                   { field: 'OptName', title: '操作名称', align: 'center', valign: 'center', width: 100 },
            ]
        };
        //调用公共函数，初始化表格
        gFunc.initgrid(authmanage.gridOpts, options);
    },
    bindEvents: function () {
        gFuncBusiness.bindEventForRbacButton();
        authmanage.btnSearchRoles.click(authmanage.searchRoles);
    },
    searchRoles: function () {
        authmanage.gridRoles.bootstrapTable('refresh');
    },
    //加载所有操作
    getAllOpts: function () {
        $.ajax({
            type: 'get',
            url: authmanage.urlSearchOpts,
            //data: { },
            success: function (result, status, XHR) {
                if (result.code == 0) {
                    authmanage.cacheOpts = result.rows;
                    //console.log(authmanage.cacheOpts);
                } else {
                    gMessager.warning(result.message);
                }
            },
            error: function (XHR, status, error) {
                gMessager.error("网络错误：" + status + "\r\n" + error);
            },
            complete: function (XHR, status) {
                //console.log('delete users completed,status:' + status);
            }
        });
    },
    //加载角色权限（模块+操作）
    getRolePermissions: function (roleId) {
        if (gFunc.isNull(roleId)) {
            return;
        }
        $.ajax({
            type: 'get',
            url: authmanage.urlGetPermission,
            data: { roleId: roleId },
            async: false,
            success: function (result, status, XHR) {
                if (result.code == 0) {
                    authmanage.cacheRolePers.set(roleId, result.rows);
                    //console.log(authmanage.cacheRolePers.get(roleId));
                } else {
                    gMessager.warning(result.message);
                }
            },
            error: function (XHR, status, error) {
                gMessager.error("网络错误：" + status + "\r\n" + error);
            },
            complete: function (XHR, status) {
                //console.log('delete users completed,status:' + status);
            }
        });
    },
    setModulesChecked: function () {
        var roleRows = authmanage.gridRoles.bootstrapTable('getSelections');
        if (roleRows.length < 1) {
            return;
        }
        var roleId = roleRows[0].Id;
        if (gFunc.isNull(roleId)) {
            return;
        }
        var modules = $.grep(authmanage.cacheRolePers.get(roleId), function (item, idx) { return item.PerType == 0 });
        //console.log(modules);

        var permModuleIds = [];
        if (modules.length > 0) {
            $(modules).each(function (idx, per) {
                permModuleIds.push(per.PermissionId.toString());//这里toString是为了利用bootstrap-treeview的查询匹配（字符串）
            });
        }
        //console.log(permModuleIds);
        authmanage.treeModules.setCheckedNodes(permModuleIds);
    },

    setOptsChecked: function () {
        authmanage.gridOpts.bootstrapTable('uncheckAll');
        var roleRows = authmanage.gridRoles.bootstrapTable('getSelections');
        if (roleRows.length < 1) {
            return;
        }

        //遍历当前操作列表
        var optData = authmanage.gridOpts.bootstrapTable('getData', true);
        var rolePer = authmanage.cacheRolePers.get(roleRows[0].Id);

        if (gFunc.isNull(optData) || optData.length < 1 ||
            gFunc.isNull(authmanage.cacheRolePers) || gFunc.isNull(rolePer) || rolePer.length < 1) {
            return;
        }
        var matchedIds = [];
        $(optData).each(function (idx, item) {
            var match = $.grep(rolePer, function (per, idx) { return per.PermissionId == item.Id && per.PerType == 1 });
            if (match.length > 0) {
                matchedIds.push(item.Id);
            }
        });
        authmanage.gridOpts.bootstrapTable("checkBy", { field: "Id", values: matchedIds })
    },
    saveModulePer: function (options) {
        //保存角色功能权限
        var roleRows = authmanage.gridRoles.bootstrapTable('getSelections');
        if (roleRows.length < 1) {
            gMessager.warning("请选择角色");
            return;
        }
        var idField = 'id';
        var moduleIds = authmanage.treeModules.getCheckedData([idField]);

        var data = { roleId: roleRows[0].Id, perIds: [], perType: 0, moduleId: null };//moduleId: null是为了与后台控制器方法参数匹配
        var idx = 0;
        if (!gFunc.isNull(moduleIds) && moduleIds.length > 0) {
            for (var idx = 0; idx < moduleIds.length; idx++) {
                data["perIds[" + idx + "]"] = moduleIds[idx][idField];
            }
        }
        //console.log(data);

        $.ajax({
            type: 'post',
            url: options.submitUrl,
            data: data,
            success: function (result, status, XHR) {
                if (result.code == 0) {
                    //authmanage.gridRoles.bootstrapTable('refresh');
                    gMessager.success('模块权限设置成功');
                    //更新角色权限缓存
                    authmanage.getRolePermissions(roleRows[0].Id);
                } else {
                    gMessager.warning(result.message);
                }
            },
            error: function (XHR, status, error) {
                gMessager.error("网络错误：" + status + "\r\n" + error);
            },
            complete: function (XHR, status) {
                //console.log('delete users completed,status:' + status);
            }
        });

    },
    saveOptPer: function (options) {
        //保存角色操作权限
        var roleRows = authmanage.gridRoles.bootstrapTable('getSelections');
        if (roleRows.length < 1) {
            gMessager.warning("请选择角色");
            return;
        }
        var moduleIds = authmanage.treeModules.getSelectedData(['id']);
        if (moduleIds.length < 1) {
            gMessager.warning("请选择模块");
            return;
        }

        //操作可以为空（即清空当前模块下所有操作权限）
        opts = authmanage.gridOpts.bootstrapTable('getSelections');

        var data = { roleId: roleRows[0].Id, perIds: [], perType: 1, moduleId: moduleIds[0].id };//perType: 1,操作权限
        var idx = 0;
        if (opts.length > 0) {
            for (var idx = 0; idx < opts.length; idx++) {
                data["perIds[" + idx + "]"] = opts[idx].Id;
            }
        }
        //console.log(data);

        $.ajax({
            type: 'post',
            url: options.submitUrl,
            data: data,
            success: function (result, status, XHR) {
                if (result.code == 0) {
                    //authmanage.gridRoles.bootstrapTable('refresh');
                    gMessager.success('操作权限设置成功');
                    //更新角色权限缓存
                    authmanage.getRolePermissions(roleRows[0].Id);

                } else {
                    gMessager.warning(result.message);
                }
            },
            error: function (XHR, status, error) {
                gMessager.error("网络错误：" + status + "\r\n" + error);
            },
            complete: function (XHR, status) {
                //console.log('delete users completed,status:' + status);
            }
        });
    },
    svaeModuleAllPer: function (options) {
        options.perType = 0;
        authmanage.saveAllPer(options);
    },
    svaeOptAllPer: function (options) {
        options.perType = 1;
        authmanage.saveAllPer(options);
    },
    saveAllPer: function (options) {
        //保存角色所有模块/操作权限
        var roleRows = authmanage.gridRoles.bootstrapTable('getSelections');
        if (roleRows.length < 1) {
            gMessager.warning("请选择角色");
            return;
        }

        $.ajax({
            type: 'post',
            url: options.submitUrl,
            data: { roleId: roleRows[0].Id, perType: options.perType },
            success: function (result, status, XHR) {
                if (result.code == 0) {
                    //authmanage.gridRoles.bootstrapTable('refresh');
                    gMessager.success('权限设置成功');
                    //更新角色权限缓存
                    authmanage.getRolePermissions(roleRows[0].Id);
                    //设置选中
                    authmanage.setModulesChecked();
                    authmanage.setOptsChecked();
                } else {
                    gMessager.warning(result.message);
                }
            },
            error: function (XHR, status, error) {
                gMessager.error("网络错误：" + status + "\r\n" + error);
            },
            complete: function (XHR, status) {
                //console.log('delete users completed,status:' + status);
            }
        });
    }
};