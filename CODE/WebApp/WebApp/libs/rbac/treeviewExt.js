var treeviewExt = {
    tree: undefined,
    dataId: '',
    dataUrl: '',
    dataField: '',
    getTreeData: function () {
        var treeData;
        $.ajax({
            url: treeviewExt.dataUrl,
            type: 'get',
            async: false,//?ͬ��
            dataType: 'json',
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert('����tree����Դʧ�ܣ�' + textStatus + ',' + errorThrown);
            },
            success: function (data, textStatus, jqXHR) {
                if (data.code !== 0) {
                    alert('������Դ����ʧ�ܣ�' + data.message);
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
        treeviewExt.dataField = options.dataField;

        treeviewExt.tree.treeview({
            data: treeviewExt.getTreeData(),
            showIcon: false,
            showCheckbox: true,
            onNodeChecked: function (event, node) {
                // console.log(node);
                //�����ӽڵ�
                treeviewExt.checkSubNodes(node, true);
                //���ø��ڵ�
                treeviewExt.checkParent(node);
            },
            onNodeUnchecked: function (event, node) {
                //�����ӽڵ�
                treeviewExt.checkSubNodes(node, false);
                //���ø��ڵ�
                treeviewExt.checkParent(node);
            }
        });
    },
    //ѡ���ӽڵ�
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
    //ѡ�и��ڵ�
    checkParent: function (node) {
        var parent = treeviewExt.tree.treeview('getParent', node);
        // console.log(parent);
        if (parent == undefined || parent.state == undefined) {
            return;
        }

        //1����ǰ�ڵ㱻ѡ�У����ϼ��ڵ�ѡ��
        if (node.state.checked == true) {
            if (parent.state.checked == false) {
                treeviewExt.tree.treeview('checkNode', [parent.nodeId, { silent: true }]);
                //�ݹ������ϼ��ڵ�
                treeviewExt.checkParent(parent);
            }
            return;
        }

        //node.state.checked == false
        //2�������ֵܽڵ㶼δѡ�У����ϼ��ڵ�δѡ��
        var siblings = treeviewExt.tree.treeview('getSiblings', node);
        var checkedSiblingsExists = false;//�Ƿ����ѡ�е��ֵܽڵ�
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
                //�ݹ������ϼ��ڵ�
                treeviewExt.checkParent(parent);
            }
        }
    },
    //ȫѡ
    checkAll: function () {
        treeviewExt.tree.treeview('checkAll', { silent: true });
        var node = treeviewExt.tree.treeview('getNode', 1);
        console.log(node);

    },
    //ȫ��ѡ
    uncheckAll: function () {
        treeviewExt.tree.treeview('uncheckAll', { silent: true });
    },
    //ȫ��չ��
    expandAll: function () {
        treeviewExt.tree.treeview('expandAll', { silent: true });
    },
    //ȫ���۵�
    collapseAll: function () {
        treeviewExt.tree.treeview('collapseAll', { silent: true });
    },
    //��ȡѡ�нڵ�����id����
    getCheckedDataIds: function () {
        var sel = treeviewExt.tree.treeview('getChecked');
        // console.log(sel.length);
        var selIds = [];
        if (sel && sel.length > 0) {
            $(sel).each(function (idx, item) {
                selIds.push(item[treeviewExt.dataId]);
            });
        }
        return selIds;
    },
    //����ָ���ڵ�ѡ��
    setCheckedNodes: function (dataIds) {
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
                        //����ѡ��ʱ���������κ��¼������Զ�ѡ���ϼ����¼��ڵ㣺Ϊ��չʾ����ʵ����
                        treeviewExt.tree.treeview('checkNode', [node.nodeId, { silent: true }]);
                        // treeviewExt.checkParent(node);
                    });
                }
            });
        }
    },
};
