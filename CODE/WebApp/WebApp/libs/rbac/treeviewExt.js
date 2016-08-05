var treeviewExt = {
    tree: undefined,
    dataId: '',
    dataUrl: '',
    dataField: '',
    funcUrlParams: null,
    singleSelect: false,
    getTreeData: function () {
        var queryParams = {};
        if (!gFunc.isNull(treeviewExt.funcUrlParams) && $.isFunction(treeviewExt.funcUrlParams)) {
            queryParams = treeviewExt.funcUrlParams();
        }
        var treeData;
        $.ajax({
            url: treeviewExt.dataUrl,
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
    },
    initTree: function (options) {
        treeviewExt.tree = $('#' + options.treeId);
        treeviewExt.dataId = options.dataId;
        treeviewExt.dataUrl = options.dataUrl;
        treeviewExt.funcUrlParams = options.funcUrlParams;
        treeviewExt.dataField = options.dataField;
        treeviewExt.singleSelect = options.singleSelect;

        treeviewExt.tree.treeview({
            data: treeviewExt.getTreeData(),
            showIcon: false,
            showCheckbox: !treeviewExt.singleSelect,
            onNodeChecked: function (event, node) {
                if (treeviewExt.singleSelect) {
                    return;
                }
                // console.log(node);
                //设置子节点
                treeviewExt.checkSubNodes(node, true);
                //设置父节点
                treeviewExt.checkParent(node);
            },
            onNodeUnchecked: function (event, node) {
                if (treeviewExt.singleSelect) {
                    return;
                }
                //设置子节点
                treeviewExt.checkSubNodes(node, false);
                //设置父节点
                treeviewExt.checkParent(node);
            }
        });
    },
    //选中子节点
    checkSubNodes: function (node, check) {
        if (node.nodes && node.nodes.length > 0) {
            $(node.nodes).each(function (idx, subNode) {
                if (check) {
                    treeviewExt.tree.treeview('checkNode', [subNode.nodeId, { silent: true }]);
                } else {
                    treeviewExt.tree.treeview('uncheckNode', [subNode.nodeId, { silent: true }]);
                }
                treeviewExt.checkSubNodes(subNode, check);
            });
        }
    },
    //选中父节点
    checkParent: function (node) {
        var parent = treeviewExt.tree.treeview('getParent', node);
        // console.log(parent);
        if (parent == undefined || parent.state == undefined) {
            return;
        }

        //1、当前节点被选中，则上级节点选中
        if (node.state.checked == true) {
            if (parent.state.checked == false) {
                treeviewExt.tree.treeview('checkNode', [parent.nodeId, { silent: true }]);
                //递归设置上级节点
                treeviewExt.checkParent(parent);
            }
            return;
        }

        //node.state.checked == false
        //2、所有兄弟节点都未选中，则上级节点未选中
        var siblings = treeviewExt.tree.treeview('getSiblings', node);
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
                treeviewExt.tree.treeview('uncheckNode', [parent.nodeId, { silent: true }]);
                //递归设置上级节点
                treeviewExt.checkParent(parent);
            }
        }
    },
    //全选
    checkAll: function () {
        if (treeviewExt.singleSelect) {
            return;
        }
        treeviewExt.tree.treeview('checkAll', { silent: true });
        var node = treeviewExt.tree.treeview('getNode', 1);
        console.log(node);

    },
    //全不选
    uncheckAll: function () {
        if (treeviewExt.singleSelect) {
            return;
        }
        treeviewExt.tree.treeview('uncheckAll', { silent: true });
    },
    //全部展开
    expandAll: function () {
        treeviewExt.tree.treeview('expandAll', { silent: true });
    },
    //全部折叠
    collapseAll: function () {
        treeviewExt.tree.treeview('collapseAll', { silent: true });
    },
    //获取选中节点数据指定字段数据，空则获取全部数据
    getSelectedData: function (fields) {
        var func_get_data = function (node) {
            if (gFunc.isNull(fields)) {
                return node;//fields空则获取全部数据
            }
            var _data = {};
            $(fields).each(function (_idx, _field) {
                _data[_field] = node[_field];
            });
            return _data;
        };

        var sel;
        if (treeviewExt.singleSelect) {
            sel = treeviewExt.tree.treeview('getSelected');
        } else {
            sel = treeviewExt.tree.treeview('getChecked');
        }
        var data = [];
        if (sel && sel.length > 0) {
            $(sel).each(function (idx, item) {
                data.push(func_get_data(item));
            });
        }
        return data;
    },
    //设置指定节点选中
    setCheckedNodes: function (dataIds) {
        if (treeviewExt.singleSelect) {
            return;
        }
        if (dataIds && dataIds.length > 0) {
            $(dataIds).each(function (idx, item) {
                var nodes = treeviewExt.tree.treeview('searchByAttribute', [item, {
                    attribute: treeviewExt.dataId,
                    ignoreCase: true,     // case insensitive
                    exactMatch: true,    // like or equals
                }]);
                // console.log(nodes);
                if (nodes && nodes.length > 0) {
                    $(nodes).each(function (idx, node) {
                        //设置选中时，不触发任何事件，不自动选中上级和下级节点：为了展示最真实数据
                        treeviewExt.tree.treeview('checkNode', [node.nodeId, { silent: true }]);
                        // treeviewExt.checkParent(node);
                    });
                }
            });
        }
    },
    search: function (txt) {
        var nodes = treeviewExt.tree.treeview('search', [txt, {
            ignoreCase: true,     // case insensitive
            exactMatch: false,    // like or equals
            revealResults: true,  // reveal matching nodes
        }]);
    }
};
