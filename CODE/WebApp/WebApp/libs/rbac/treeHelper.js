//树控件
var treeHelper = {
    /*
    tree:树标志，可以是dom、css选择器或jquery对象
    options:{
        dataId:数据id
        dataUrl:数据源url
        funcUrlParams:用来获取查询数据源时传递的参数的函数
        dataField:包含树数据的json对象名称
        singleSelect:是否单选
        manualInit:是否手动初始化
    }
    */
    create: function (tree, options) {
        var obj = {};
        //初始化
        obj.init = function () {
            obj.tree = $(tree);
            obj.dataId = options.dataId;
            obj.dataUrl = options.dataUrl;
            obj.funcUrlParams = options.funcUrlParams;
            obj.dataField = options.dataField;
            obj.singleSelect = options.singleSelect;
            obj.manualInit = options.manualInit;

            obj.tree.treeview({
                data: obj.getTreeData(),
                showIcon: false,
                showCheckbox: !obj.singleSelect,
                onNodeChecked: function (event, node) {
                    if (obj.singleSelect) {
                        return;
                    }
                    // console.log(node);
                    //设置子节点
                    obj.checkSubNodes(node, true);
                    //设置父节点
                    obj.checkParent(node);
                },
                onNodeUnchecked: function (event, node) {
                    if (obj.singleSelect) {
                        return;
                    }
                    //设置子节点
                    obj.checkSubNodes(node, false);
                    //设置父节点
                    obj.checkParent(node);
                }
            });
        };

        //获取树数据
        obj.getTreeData = function () {
            var queryParams = {};
            if (!gFunc.isNull(obj.funcUrlParams) && $.isFunction(obj.funcUrlParams)) {
                queryParams = obj.funcUrlParams();
            }
            var treeData;
            $.ajax({
                url: obj.dataUrl,
                type: 'get',
                async: false,//?同步
                dataType: 'json',
                data: queryParams,
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert('加载tree数据源失败！' + textStatus + ',' + errorThrown);
                },
                success: function (data, textStatus, jqXHR) {
                    if (data.code !== 0) {
                        alert('加载树源数据失败！' + data.message);
                        return;
                    }
                    treeData = data.rows;
                }
            });
            return treeData;
        };

        //选中子节点
        obj.checkSubNodes = function (node, check) {
            if (node.nodes && node.nodes.length > 0) {
                $(node.nodes).each(function (idx, subNode) {
                    if (check) {
                        obj.tree.treeview('checkNode', [subNode.nodeId, { silent: true }]);
                    } else {
                        obj.tree.treeview('uncheckNode', [subNode.nodeId, { silent: true }]);
                    }
                    obj.checkSubNodes(subNode, check);
                });
            }
        };

        //选中父节点
        obj.checkParent = function (node) {
            var parent = obj.tree.treeview('getParent', node);
            // console.log(parent);
            if (parent == undefined || parent.state == undefined) {
                return;
            }

            //1、当前节点被选中，则上级节点选中
            if (node.state.checked == true) {
                if (parent.state.checked == false) {
                    obj.tree.treeview('checkNode', [parent.nodeId, { silent: true }]);
                    //递归设置上级节点
                    obj.checkParent(parent);
                }
                return;
            }

            //node.state.checked == false
            //2、所有兄弟节点都未选中，则上级节点未选中
            var siblings = obj.tree.treeview('getSiblings', node);
            var checkedSiblingsExists = false;//是否存在选中的兄弟节点
            if (siblings && siblings.length > 0) {
                if ($.grep(siblings, function (sib, idx) {
                    return sib.state.checked == true;
                }).length > 0) {
                    //
                    checkedSiblingsExists = true;
                }
            }
            if (checkedSiblingsExists == false) {
                if (parent.state.checked == true) {
                    obj.tree.treeview('uncheckNode', [parent.nodeId, { silent: true }]);
                    //递归设置上级节点
                    obj.checkParent(parent);
                }
            }
        };

        //全选
        obj.checkAll = function () {
            if (obj.singleSelect) {
                return;
            }
            obj.tree.treeview('checkAll', { silent: true });
            var node = obj.tree.treeview('getNode', 1);
            //console.log(node);

        };
        //全不选
        obj.uncheckAll = function () {
            if (obj.singleSelect) {
                return;
            }
            obj.tree.treeview('uncheckAll', { silent: true });
        };
        //全部展开
        obj.expandAll = function () {
            obj.tree.treeview('expandAll', { silent: true });
        };
        //全部折叠
        obj.collapseAll = function () {
            obj.tree.treeview('collapseAll', { silent: true });
        };

        //获取选中节点指定字段数据
        obj.getSelectedData = function (fields) {
            return obj._getNodesData(obj.tree.treeview('getSelected'), fields);
        };
        //获取Checked状态节点指定字段数据
        obj.getCheckedData = function (fields) {
            return obj._getNodesData(obj.tree.treeview('getChecked'), fields);
        };
        //获取指定节点指定字段数据，空则获取指定节点全部字段数据
        obj._getNodesData = function (nodes, fields) {
            if (gFunc.isNull(nodes) || nodes.length < 1) {
                return null;
            }
            var func_get_node_data = function (node) {
                if (gFunc.isNull(fields)) {
                    return node;//fields空则获取全部数据
                }
                var _data = {};
                $(fields).each(function (_idx, _field) {
                    _data[_field] = node[_field];
                });
                return _data;
            };

            var data = [];
            if (nodes && nodes.length > 0) {
                $(nodes).each(function (idx, item) {
                    data.push(func_get_node_data(item));
                });
            }
            return data;
        };

        //设置指定节点选中
        obj.setCheckedNodes = function (dataIds) {
            if (obj.singleSelect) {
                return;
            }
            obj.uncheckAll();
            if (dataIds && dataIds.length > 0) {
                $(dataIds).each(function (idx, item) {
                    //调用自定义扩展方法searchByAttribute获取指定节点
                    var nodes = obj.tree.treeview('searchByAttribute', [item, {
                        attribute: obj.dataId,
                        ignoreCase: true,     // case insensitive
                        exactMatch: true,    // like or equals
                    }]);
                    // console.log(nodes);
                    if (nodes && nodes.length > 0) {
                        $(nodes).each(function (idx, node) {
                            //设置选中时，不触发任何事件，不自动选中上级和下级节点：为了展示最真实数据
                            obj.tree.treeview('checkNode', [node.nodeId, { silent: true }]);
                            // obj.checkParent(node);
                        });
                    }
                });
            }
        };
        //筛选
        obj.search = function (txt) {
            var nodes = obj.tree.treeview('search', [txt, {
                ignoreCase: true,     // case insensitive
                exactMatch: false,    // like or equals
                revealResults: true,  // reveal matching nodes
            }]);
        };

        //初始化
        if (!obj.manualInit) {//是否手动初始化
            obj.init();
        }

        return obj;
    }
};