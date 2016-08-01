﻿var authmanage = {
    gridRoles: $('#gridRoles'),
    gridModules: $('#gridModules'),
    urlSearchRoles: "",
    urlSearchModules: "",
    urlSavePermission: "",
    urlGetPermission: "",
    currentPermissions: null,
    txtSearchRoleName: $('#txtSearchRoleName'),
    btnSearchRoles: $('#btnSearchRoles'),
    txtSearchModuleName: $('#txtSearchModuleName'),
    btnSearchModules: $('#btnSearchModules'),
    btnSaveModulePer: $('#btnSaveModulePer'),
    init: function (options) {
        authmanage.urlSearchRoles = options.urlSearchRoles;
        authmanage.urlSearchModules = options.urlSearchModules;
        authmanage.urlSavePermission = options.urlSavePermission;
        authmanage.urlGetPermission = options.urlGetPermission;

        authmanage.initRolesGrid();
        authmanage.initModulesTree();

        authmanage.btnSearchRoles.click(authmanage.searchRoles);
        authmanage.btnSearchModules.click(authmanage.searchModules);
        authmanage.btnSaveModulePer.click(authmanage.saveModulePer);
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
                   { field: 'check', radio: true, width: 40 },
                   { field: 'RoleName', title: '角色名', align: 'center', valign: 'center', width: 80 },
                   {
                       field: 'Remark', title: '备注', align: 'center', valign: 'center', width: 140,
                       cellStyle: function (value, row, index, field) {
                           return {
                               css: { "min-width": "200px" }
                           };
                       }
                   }
            ]
        };
        //调用公共函数，初始化表格
        gFunc.initgrid(authmanage.gridRoles, options);
        //注册列表事件
        authmanage.gridRoles.on('check.bs.table', function (e, row) {
            treeviewExt.uncheckAll();
            var roleId = row.Id;
            if (gFunc.isNull(roleId)) {
                return;
            }
            //加载角色权限
            $.ajax({
                type: 'get',
                url: authmanage.urlGetPermission,
                data: { roleId: roleId },
                success: function (result, status, XHR) {
                    if (result.code == 0) {
                        //gMessager.success('角色权限获取成功');
                        //下面设置右侧列表选中
                        var permissions = result.rows;
                        if (gFunc.isNull(permissions) || permissions.length < 1) {
                            return;
                        }
                        var permModuleIds = [];
                        $(permissions).each(function (idx, perm) {
                            permModuleIds.push(perm.PermissionId.toString());//这里toString是为了利用bootstrap-treeview的查询匹配（字符串）
                        });
                        //console.log(permModuleIds);
                        treeviewExt.setCheckedNodes(permModuleIds);
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
        });
    },
    initModulesTree: function () {
        treeviewExt.initTree({
            treeId: 'treeModule',
            dataId: 'id',
            dataField: 'rows',
            dataUrl: authmanage.urlSearchModules
        });
    },
    searchRoles: function () {
        authmanage.gridRoles.bootstrapTable('refresh');
    },
    searchModules: function () {
        //查询文本
        var name = authmanage.txtSearchModuleName.val();

        var nodes = treeviewExt.tree.treeview('search', [name, {
            ignoreCase: true,     // case insensitive
            exactMatch: false,    // like or equals
            revealResults: true,  // reveal matching nodes
        }]);
    },
    saveModulePer: function () {
        //保存角色功能权限
        var roleRows = authmanage.gridRoles.bootstrapTable('getSelections');
        if (roleRows.length < 1) {
            gMessager.warning("请选择角色");
            return;
        }
        var moduleIds = treeviewExt.getCheckedDataIds();
        if (moduleIds.length < 1) {
            gMessager.warning("请选择模块");
            return;
        }
        var data = { roleId: roleRows[0].Id, perType: 0 };
        var idx = 0;
        for (var idx = 0; idx < moduleIds.length; idx++) {
            data["perIds[" + idx + "]"] = moduleIds[idx];
        }
        $.ajax({
            type: 'post',
            url: authmanage.urlSavePermission,
            data: data,
            success: function (result, status, XHR) {
                if (result.code == 0) {
                    //authmanage.gridRoles.bootstrapTable('refresh');
                    gMessager.success('模块权限设置成功');
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