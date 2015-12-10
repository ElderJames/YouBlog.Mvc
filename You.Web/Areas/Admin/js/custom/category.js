var Script = function () {
    var $table = $('#CategoryTable'),
      $remove = $('#remove'),
      selections = [];
    var $CategoryList = new Array();
    var $CategoryTree = new Array();
    var $TableTree = new Array();
    var $DeletedCategory = new Array();
    var Service = {
        getData: function (callback) {
        var dialog = BootstrapDialog.show({
                            message: '正在获取，请稍候...',
                            closable: false
                        });
            $.getJSON('/Admin/Category/List?Type=' + $('#Type').val(), function (data) {dialog.close(); callback(data); });
        },
        update: function (method, callback) {
            var dialog = BootstrapDialog.show({
                message: '正在提交，请稍候...',
                closable: false
            });
            $.ajax({
                url: method == 'Add' ? '/Admin/Category/Add' : '/Admin/Category/Edit/' + $('#CategoryID').val(),
                type: 'POST',
                data: $('#cat-form').serialize(),
                dataType: 'json',
                success: function (data) {
                dialog.close();
                    if (data.result) $('#cat-form')[0].reset();
                    callback(data);
                },
                error: function (result) {
                    console.error("服务器出错");
                }
            });
        },
        Delete: function (id, callback) {
            var dialog = BootstrapDialog.show({
                message: '正在移除，请稍候...',
                closable: false
            });
            $.post("/Admin/Category/Delete", { CategoryID: id }, function (data) {dialog.close(); callback(data); })
        },
        Recovery: function (id, callback) {
            var dialog = BootstrapDialog.show({
                message: '正在恢复，请稍候...',
                closable: false
            });
            $.post("/Admin/Category/Recovery", { CategoryID: id }, function (data) {dialog.close(); callback(data); })
        },
         getTranslation: function (origin, callback) {
            $.get('/Admin/Article/Translate?query=' + origin, function (data) { callback(data); })
        }
    }
    function getChildren(parentId, parentName, level) {
        var $list = new Array();
        $.each($CategoryList, function (index, $item) {
            if ($item.ParentId == parentId) {
                $item.id = $item.CategoryID;
                $item.text = $item.Name;
                $item.parentName = parentName;
                $item.level = level;
                $list.push($item);
                if ($item.State == 1) $DeletedCategory.push($item);
                else $TableTree.push($item);
            }
        });
        return $list.sort(function (a, b) { return a.Order > b.Order });
    }

    function getCategoryTree($nodes) {
        $.each($nodes, function (index, item) {
            var nodes = getChildren(item.id, item.Name, item.level + 1);
            if (nodes.length > 0) {
                item.nodes = nodes;
                getCategoryTree(item.nodes);
            }
        });
    }

    function getCategory(callback) {
        Service.getData(function (data) {
            $TableTree = new Array();
            $DeletedCategory = new Array();
            $CategoryList = data;
            var $list = [{ id: 0, text: "根菜单项" }];
            $list[0].nodes = getChildren(0, '根菜单项', 0);
            getCategoryTree($list[0].nodes);
            callback($list);
        });
    }


    $(function () {
        $table.bootstrapTable({
            ajax: ajaxRequest,
            height: getHeight(),
            columns: [{
                field: 'state',
                checkbox: true,
                // rowspan: 2,
                align: 'center',
                valign: 'middle'
            }, {
                title: '名称',
                field: 'Name',
                sortable: false,
                align: 'center'
            },
             {
                 title: '说明',
                 field: 'Description',
                 sortable: false,
                 align: 'left'
             },
                    {
                        title: '父栏目',
                        field: 'parentName',
                        sortable: false,
                        align: 'center'
                    },

                    {
                        title: '类型',
                        field: 'TypeToString',
                        sortable: false,
                        align: 'left'
                    },
                      {
                          title: '记录单位',
                          field: 'RecordUnit',
                          sortable: false,
                          align: 'left'
                      },
                        {
                            title: '记录名称',
                            field: 'RecordName',
                            sortable: false,
                            align: 'left'
                        },
                    {
                        title: '创建时间',
                        field: 'CreateTime',
                        sortable: false,
                        // footerFormatter: totalNameFormatter,
                        align: 'left'
                    },
                     {
                         title: '状态',
                         field: 'StateToString',
                         sortable: false,
                         // footerFormatter: totalNameFormatter,
                         align: 'center'
                     },
                     {
                         field: 'operate',
                         title: '操作',
                         align: 'center',
                         events: operateEvents,
                         formatter: operateFormatter
                     }
            ]
        });
        // sometimes footer render error.
        setTimeout(function () {
            $table.bootstrapTable('resetView');
        }, 1);
        $table.on('check.bs.table uncheck.bs.table ' +
                'check-all.bs.table uncheck-all.bs.table', function () {
                    $remove.prop('disabled', !$table.bootstrapTable('getSelections').length);

                    // save your data, here just save the current page
                    selections = getIdSelections();
                    // push or splice the selections if you want to save all data selections
                });
        $table.on('expand-row.bs.table', function (e, index, row, $detail) {
            if (index % 2 == 1) {
                $detail.html('Loading from ajax request...');
                $.get('LICENSE', function (res) {
                    $detail.html(res.replace(/\n/g, '<br>'));
                });
            }
        });
        $table.on('all.bs.table', function (e, name, args) {
            console.log(name, args);
        });
        $remove.click(function () {
            var ids = getIdSelections();
            if ($remove.data('method') == 'delete') {
                BootstrapDialog.alert({
                    message: '确认要删除这些菜单项吗？',
                    type: BootstrapDialog.TYPE_DANGER,
                    closable: true, // <-- Default value is false
                    draggable: true,
                    buttonLabel: '确定',
                    callback: function () {
                        Service.Delete(ids, function (data) {
                            if (data.result) {
                                $table.bootstrapTable('refresh');
                                BootstrapDialog.alert({
                                    message: '删除成功',
                                    type: BootstrapDialog.TYPE_SUCCESS,
                                    closable: true, // <-- Default value is false
                                    draggable: true,
                                    buttonLabel: '确定'
                                });
                            }
                            else BootstrapDialog.alert({
                                message: data.error,
                                type: BootstrapDialog.TYPE_DANGER,
                                closable: true, // <-- Default value is false
                                draggable: true,
                                buttonLabel: '确定',
                                callback: function () {
                                    // $('#UserFormModal').modal('show');
                                }
                            });
                        });
                    }
                });
            }
            else {
                BootstrapDialog.alert({
                    message: '确认要恢复这些菜单项吗？',
                    type: BootstrapDialog.TYPE_DANGER,
                    closable: true, // <-- Default value is false
                    draggable: true,
                    buttonLabel: '确定',
                    callback: function () {
                        Service.Recovery(ids, function (data) {
                            if (data.result) {
                                $table.bootstrapTable('refresh');
                                BootstrapDialog.alert({
                                    message: '恢复成功',
                                    type: BootstrapDialog.TYPE_SUCCESS,
                                    closable: true, // <-- Default value is false
                                    draggable: true,
                                    buttonLabel: '确定'
                                });
                            }
                            else BootstrapDialog.alert({
                                message: data.error,
                                type: BootstrapDialog.TYPE_DANGER,
                                closable: true, // <-- Default value is false
                                draggable: true,
                                buttonLabel: '确定',
                                callback: function () {
                                    //  $('#CategoryFormModal').modal('show');
                                }
                            });
                        });
                    }
                });
            }
            //$table.bootstrapTable('remove', {
            //    field: 'id',
            //    values: ids
            //});
            $remove.prop('disabled', true);
        });
        $(window).resize(function () {
            $table.bootstrapTable('resetView', {
                height: getHeight()
            });
        });
    });
    function getIdSelections() {
        return $.map($table.bootstrapTable('getSelections'), function (row) {
            return row.CategoryID;
        });
    }

    function responseHandler(res) {
        $.each(res.rows, function (i, row) {
            row.state = $.inArray(row.id, selections) !== -1;
        });
        return res;
    }

    function detailFormatter(index, row) {
        var html = [];
        $.each(row, function (key, value) {
            html.push('<p><b>' + key + ':</b> ' + value + '</p>');
        });
        return html.join('');
    }
    function operateFormatter(value, row, index) {
        if (row.State != 1)
            return [
                '<a class="btn btn-success btn-xs add" href="javascript:void(0)" title="添加子菜单">',
                '<i class="icon icon icon icon-plus"></i>',
                '</a>  ',
                '<a class="btn btn-primary btn-xs edit" href="javascript:void(0)" title="编辑此菜单">',
                '<i class="icon icon icon-pencil"></i>',
                '</a>  ',
                '<a class="btn btn-danger btn-xs remove" href="javascript:void(0)" title="移除此菜单">',
                '<i class="icon icon-remove"></i>',
                '</a>  '
            ].join('');
        else return [
            '<a class="btn btn-success btn-xs recovery" href="javascript:void(0)" title="恢复">',
            '<i class="icon-undo"></i>',
            '</a>'
        ].join('');
    }

    window.operateEvents = {
        'click .add': function (e, value, row, index) {
            $('#cat-form')[0].reset();
            $('#FormTitle').text('添加子菜单');
            $('#ParentId').val(row.CategoryID);
            $('#Parent').val(row.Name);
            $('#method').val('Add');
            $('#CategoryFormModal').modal('show');

        },
        'click .edit': function (e, value, row, index) {
            $('#cat-form')[0].reset();
            $('#FormTitle').text('修改菜单');
            $('#cat-form').fill(row, { styleElementName: 'none' });
            $('#Parent').val(row.parentName);
            $('#method').val('Edit');
            $('#CategoryID').val(row.CategoryID);
            $('#CategoryFormModal').modal('show');
            // alert('You click edit action, row: ' + JSON.stringify(row));
        },
        'click .remove': function (e, value, row, index) {
            BootstrapDialog.alert({
                message: '确认要删除[' + row.Name + ']吗？',
                type: BootstrapDialog.TYPE_DANGER,
                closable: true, // <-- Default value is false
                draggable: true,
                buttonLabel: '确定',
                callback: function () {
                    Service.Delete([row.CategoryID], function (data) {
                        if (data.result) {
                            $table.bootstrapTable('refresh');
                            BootstrapDialog.alert({
                                message: '删除成功',
                                type: BootstrapDialog.TYPE_SUCCESS,
                                closable: true, // <-- Default value is false
                                draggable: true,
                                buttonLabel: '确定'
                            });
                        }
                        else BootstrapDialog.alert({
                            message: data.error,
                            type: BootstrapDialog.TYPE_DANGER,
                            closable: true, // <-- Default value is false
                            draggable: true,
                            buttonLabel: '确定',
                            callback: function () {
                                // $('#CategoryFormModal').modal('show');
                            }
                        });
                    });
                }
            });
        },
        'click .recovery': function (e, value, row, index) {
            BootstrapDialog.alert({
                message: '确认要恢复[' + row.Name + ']吗？',
                type: BootstrapDialog.TYPE_DANGER,
                closable: true, // <-- Default value is false
                draggable: true,
                buttonLabel: '确定',
                callback: function () {
                    Service.Recovery([row.CategoryID], function (data) {
                        if (data.result) {
                            $table.bootstrapTable('refresh');
                            BootstrapDialog.alert({
                                message: '恢复成功',
                                type: BootstrapDialog.TYPE_SUCCESS,
                                closable: true, // <-- Default value is false
                                draggable: true,
                                buttonLabel: '确定'
                            });
                        }
                        else BootstrapDialog.alert({
                            message: data.error,
                            type: BootstrapDialog.TYPE_DANGER,
                            closable: true, // <-- Default value is false
                            draggable: true,
                            buttonLabel: '确定',
                            callback: function () {
                                //  $('#CategoryFormModal').modal('show');
                            }
                        });
                    });
                }
            });
        }
    };

    function getHeight() {
        return $(window).height() - 120;

    }

    function ajaxRequest(params) {
        // just use setTimeout
        getCategory(function (data) {
            // console.log(data);
            $('#CategoryTree').treeview({
                levels: 99,
                data: data,
                onNodeSelected: function (event, node) {
                    $('#Parent').val(node.text);
                    $('#ParentId').val(node.id);
                    $('#CategoryModal').modal('hide');
                    $('#CategoryFormModal').modal('show');
                }
            });
            // var data2 = [{ "CategoryID": 1, "Name": "用户管理", "Alias": "UserManager", "ParentId": 0, "Description": "用户管理", "CategoryType": 1, "Url": "User/Index", "isAllAction": false, "Controller": "User", "Action": "Index", "Parameter": null, "Order": 1, "CreateTime": "2015-08-05 15:52:23", "State": 0, "CategoryTypeToString": "节点", "StateToString": "正常" }, { "CategoryID": 2, "Name": "角色管理", "Alias": "RoleManager", "ParentId": 0, "Description": null, "CategoryType": 0, "Url": "Role/Index", "isAllAction": false, "Controller": "Role", "Action": "Index", "Parameter": null, "Order": 2, "CreateTime": "2015-08-05 16:34:35", "State": 0, "CategoryTypeToString": "节点", "StateToString": "正常" }, { "CategoryID": 3, "Name": "部门管理", "Alias": "DepartmentManager", "ParentId": 0, "Description": null, "CategoryType": 1, "Url": "Department/Index", "isAllAction": false, "Controller": "Department", "Action": "Index", "Parameter": null, "Order": 3, "CreateTime": "2015-08-05 16:37:29", "State": 0, "CategoryTypeToString": "节点", "StateToString": "正常" }];
            if ($('#State').data('state') == "Deleted") params.success($DeletedCategory);
            else params.success($TableTree);
            // hide loading
            params.complete();
        });
    }
    $('#State a').on('click', function () {
        $('#State').data('state', $(this).data('state'));
        if ($(this).data('state') == "Deleted") {
            $remove.html('恢复菜单').removeClass('btn-danger').addClass('btn-success');
            $remove.data('method', 'recovery');
            $table.bootstrapTable('load', $DeletedCategory);
            $('#State .text').html('已删除');
            //  $('#toolbar .btn-group').html('<button type="button" class="btn btn-sm btn-primary" data-toggle="modal" id="Recovery">恢复菜单</button>');
        }
        else {
            $remove.html('删除菜单').removeClass('btn-success').addClass('btn-danger');
            $remove.data('method', 'delete');
            $table.bootstrapTable('load', $TableTree);
            $('#State .text').html('正常');
        }
    });

    $('#CategoryFormModal .save').on('click', function () {
        Service.update($('#method').val(), function (data) {
            $('#CategoryFormModal').modal('hide');
            $table.bootstrapTable('refresh');
            if (data.result) {
                BootstrapDialog.confirm({
                    message: '提交成功！',
                    type: BootstrapDialog.TYPE_SUCCESS, // <-- Default value is BootstrapDialog.TYPE_PRIMARY
                    closable: true, // <-- Default value is false
                    draggable: true, // <-- Default value is false
                    btnCancelLabel: '回到列表', // <-- Default value is 'Cancel',
                    btnOKLabel: '继续添加', // <-- Default value is 'OK',
                    btnOKClass: 'btn-success', // <-- If you didn't specify it, dialog type will be used,
                    callback: function (result) {
                        // result will be true if button was click, while it will be false if users close the dialog directly.
                        if (result) {
                            $('#CategoryFormModal').modal('show');
                        } else {

                        }
                    }
                });
            } else {
                BootstrapDialog.alert({
                    message: data.error,
                    type: BootstrapDialog.TYPE_DANGER,
                    closable: true, // <-- Default value is false
                    draggable: true,
                    buttonLabel: '确定',
                    callback: function () {
                        $('#CategoryFormModal').modal('show');
                    }
                });
            }
        });
    });

    var typing = false;
    var keyword = '';
    var submited = false;
    var tempval = '';

    $('#title').on('keydown', function () {
        typing = true;
    });

    $('#title').on('keyup blur', function () {
        keyword = $('#title').val();
        typing = false;
        if (keyword == '') $('#subtitle').val('');
        console.log(keyword);
        setTimeout(function () {
            if (typing || keyword.length <= 0 || keyword == tempval) return;
            tempval = keyword;
            console.log('translating "' + keyword + '"')
            Service.getTranslation($('#title').val(), function (data) {
                if (typing) return;
                if (data.result && data.data[0].length > 0 && keyword != '')
                    $('#subtitle').val(data.data[0].toLocaleLowerCase().replace(/\s+/g, "-").replace(/[\ |\~|\`|\!|\@|\#|\$|\%|\^|\&|\*|\(|\)|\_|\+|\=|\||\\|\[|\]|\{|\}|\;|\:|\"|\'|\,|\<|\.|\>|\/|\?]/g, ""));
            });
        }, 500);
    });

    $('#Parent').on('click focus', function () {
        $('#CategoryModal').modal('show');
    });
    $('#CategoryModal').on('show.bs.modal', function () {
        $('#CategoryFormModal').modal('hide');
    });
    $('#CategoryModal').on('hidden.bs.modal', function () {
        $('#CategoryFormModal').modal('show');
    });
    $('#AddCategory').on('click', function () {
        $('#cat-form')[0].reset();
        $('#method').val('Add');
        $('#CategoryFormModal').modal('show');
    }) 

}();