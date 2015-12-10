var Script = function () {
    var $table = $('#UserTable'),
    $remove = $('#remove'),
    selections = [];
    var $UserList = new Array();

    var Service = {
        getData: function (callback) {
            var dialog = BootstrapDialog.show({
                message: '正在获取，请稍候...',
                closable: false
            });
            $.getJSON('/Admin/User/List', function (data) {dialog.close(); callback(data); });
        },
        update: function (method, callback) {
            var dialog = BootstrapDialog.show({
                message: '正在提交，请稍候...',
                closable: false
            });
            $.ajax({
                url: method == 'Add' ? '/Admin/User/Add' : '/Admin/User/Edit/' + $('#UserID').val(),
                type: 'POST',
                data: $('#user-form').serialize(),
                dataType: 'json',
                success: function (data) {
                    dialog.close();
                    if (data.result) $('#user-form')[0].reset();
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
            }); $.post("/Admin/User/Delete", { Id: id }, function (data) {dialog.close(); callback(data); })
        },
        Recovery: function (id, callback) {
            var dialog = BootstrapDialog.show({
                message: '正在恢复，请稍候...',
                closable: false
            });
            $.post("/Admin/User/Recovery", { Id: id }, function (data) {dialog.close(); callback(data); })
        }
    }
    function getUser(callback) {
        Service.getData(function (data) {
            $UserList = new Array();
            $RoleList = new Array();
            $UserList = data;
            callback($UserList);
        });
    }
       
    
    function Filter(data,state)
    {
        if (state == -1) return data;
        var $list=new Array();
        $.each(data, function (index, item) {
            if (item.State == state) $list.push(item);
        });
        return $list;
    }
    $(function () {
        $table.bootstrapTable({
            //ajax: ajaxRequest,
            height: getHeight(),
            columns: [{
                field: 'state',
                checkbox: true,
                rowspan: 2,
                align: 'center',
                valign: 'middle'
            }, {
                title: '用户名',
                field: 'UserName',
                sortable: false,
                align: 'center'
            },{
                title: '邮箱',
                field: 'Email',
                sortable: false,
                align: 'left'
            },{
                title: '真实姓名',
                field: 'RealName',
                sortable: false,
                align: 'center'
            }, {
                title: '注册时间',
                field: 'RegisterOn',
                sortable: false,
                // footerFormatter: totalNameFormatter,
                align: 'left'
            }, {
                title: '最后登录时间',
                field: 'LoginTime',
                sortable: false,
                // footerFormatter: totalNameFormatter,
                align: 'left'
            }, {
                title: '状态',
                field: 'StateToString',
                sortable: false,
                // footerFormatter: totalNameFormatter,
                align: 'center'
             },{
                field: 'operate',
                title: '操作',
                align: 'center',
                events: operateEvents,
                formatter: operateFormatter
            }]
        });//table

        // sometimes footer render error.
        setTimeout(function () {
            $table.bootstrapTable('resetView');
        }, 1);
        $table.on('load-success.bs.table', function (e,data) {
            console.log('load-success:',data);
        })
        $table.on('check.bs.table uncheck.bs.table ' +
                'check-all.bs.table uncheck-all.bs.table', function () {
                    $remove.prop('disabled', !$table.bootstrapTable('getSelections').length);

                    // save your data, here just save the current page
                    selections = getIdSelections();
                    // push or splice the selections if you want to save all data selections
                });
        $table.on('expand-row.bs.table', function (e, index, row, $detail) {
            //if (index % 2 == 1) {
            //    $detail.html('Loading from ajax request...');
            //    $.get('LICENSE', function (res) {
            //        $detail.html(res.replace(/\n/g, '<br>'));
            //    });
            //}
        });
        $table.on('all.bs.table', function (e, name, args) {
            //console.log(name, args);
        });
        $remove.click(function () {
            var ids = getIdSelections();
            BootstrapDialog.alert({
                message: '确认要删除这些用户吗？',
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
            return row.Id;
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
                '<a class="btn btn-primary btn-xs edit" href="javascript:void(0)" title="编辑">',
                '<i class="icon icon icon-pencil"></i>',
                '</a>  ',
                '<a class="btn btn-danger btn-xs remove" href="javascript:void(0)" title="移除">',
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
        'click .edit': function (e, value, row, index) {
            $('#user-form')[0].reset();
            $('#FormTitle').text('修改用户资料');
            $('#user-form').fill(row, { styleElementName: 'none' });
            $("#RoleId").trigger("change");
            $("#DepartmentId").trigger("change");
            $("#JobId").trigger("change");
            $('#method').val('Edit');
            $('#UserID').val(row.UserID);
            $('#UserFormModal').modal('show');
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
                    Service.Delete([row.Id], function (data) {
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
        },
        'click .recovery': function (e, value, row, index) {
            BootstrapDialog.alert({
                message: '确认要恢复[' + row.UserName + ']吗？',
                type: BootstrapDialog.TYPE_DANGER,
                closable: true, // <-- Default value is false
                draggable: true,
                buttonLabel: '确定',
                callback: function () {
                    Service.Recovery([row.Id], function (data) {
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
                                //  $('#UserFormModal').modal('show');
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
        getUser(function (data) {
            params.success(Filter(data, $('#State').data('state')));
            // hide loading
            params.complete();
        });
    }
    $('#State a').on('click', function () {
        $('#State').data('state', $(this).data('state'));
        $table.bootstrapTable('refresh');
        //if ($('#State').val() == 1) {
        //    $table.bootstrapTable('load', $DeletedUser);
        //    //  $('#toolbar .btn-group').html('<button type="button" class="btn btn-sm btn-primary" data-toggle="modal" id="Recovery">恢复菜单</button>');
        //}
        //else $table.bootstrapTable('load', $TableTree);
    });

    $('#UserFormModal .save').on('click', function () {
     $('#UserFormModal').modal('hide');
        Service.update($('#method').val(), function (data) {
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
                            $("#UserID").val('');
                            $("method").val('Add');
                            $("#RoleId").trigger("change");
                            $("#DepartmentId").trigger("change");
                            $("#JobId").trigger("change");
                            $('#UserFormModal').modal('show');
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
                        $('#UserFormModal').modal('show');
                    }
                });
            }
        });
    });
    $('#Parent').on('click focus', function () {
        $('#UserModal').modal('show');
    });
    $('#UserModal').on('show.bs.modal', function () {
        $('#UserFormModal').modal('hide');
    });
    $('#UserModal').on('hidden.bs.modal', function () {
        $('#UserFormModal').modal('show');
    });
    $('#AddUser').on('click', function () {
        $('#user-form')[0].reset();
        $('#method').val('Add');
        $('#UserFormModal').modal('show');
    })


}();