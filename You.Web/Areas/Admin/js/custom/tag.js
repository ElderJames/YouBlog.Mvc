var Script = function () {
    var $table = $('#TagTable'),
   $remove = $('#remove'),
   selections = [];
    var $TagList = new Array();
    var $TagTree = new Array();
    var $TableTree = new Array();
    var $DeletedTag = new Array();
    var Service = {
        getData: function (params, callback) {
            var dialog = BootstrapDialog.show({
                message: '正在获取，请稍候...',
                closable: false
            });
            $.get('/Admin/Tag/All', params, function (data) {dialog.close(); callback(data); });
        },
        update: function (method, callback) {
            var dialog = BootstrapDialog.show({
                message: '正在提交，请稍候...',
                closable: false
            });
            $.ajax({
                url: method == 'Add' ? '/Admin/Tag/Add' : '/Admin/Tag/Edit/' + $('#TagID').val(),
                type: 'POST',
                data: $('#Tag-form').serialize(),
                dataType: 'json',
                success: function (data) {
                    dialog.close();
                    if (data.result) $('#Tag-form')[0].reset();
                    callback(data);
                },
                error: function (result) {
                    console.error("服务器出错");
                }
            });
        },
        Delete: function (id, callback) {
            var dialog = BootstrapDialog.show({
                message: '正在删除，请稍候...',
                closable: false
            });
            $.post("/Admin/Tag/Delete", { TagID: id }, function (data) { dialog.close(); callback(data); })
        },
        Recovery: function (id, callback) {
            var dialog = BootstrapDialog.show({
                message: '正在恢复，请稍候...',
                closable: false
            });
            $.post("/Admin/Tag/Recovery", { TagID: id }, function (data) { dialog.close(); callback(data); })
        },
        getTranslation: function (origin, callback) {
        $.get('/Admin/Article/Translate?query=' + origin, function (data) { callback(data); })
    }
    }


    function getTag(callback) {
        Service.getData(function (data) {
            callback(data);
        });
    }


    $(function () {
        $table.bootstrapTable({
            ajax: ajaxRequest,
            height: getHeight(),
            detailFormatter: detailFormatter,
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
                 title: '别名',
                 field: 'SubTitle',
                 sortable: false,
                 align: 'center'
             },
                    {
                        title: '创建时间',
                        field: 'CreateTime',
                        sortable: false,
                        align: 'left'
                    },
                     {
                         title: '状态',
                         field: 'StateToString',
                         sortable: false,
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

            $detail.html(row.MenuName.join(','));
        });
        $table.on('all.bs.table', function (e, name, args) {
            console.log(name, args);
        });
        $remove.click(function () {
            var ids = getIdSelections();
            if ($remove.data('method') == 'delete') {
                BootstrapDialog.alert({
                    message: '确认要删除这些职位吗？',
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
                    message: '确认要恢复这些标签吗？',
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
                                    //  $('#MenuFormModal').modal('show');
                                }
                            });
                        });
                    }
                });
            }
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
            return row.TagID;
        });
    }

    function responseHandler(res) {
        $.each(res.rows, function (i, row) {
            row.state = $.inArray(row.id, selections) !== -1;
        });
        return res;
    }
    function MenusFormatter(value, row, index) {
        console.log(row.MenuName);
        return row.MenuName.join(',');
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
        'click .edit': function (e, value, row, index) {
            $('#Tag-form')[0].reset();
            $('#FormTitle').text('修改标签');
            $('#Tag-form').fill(row, { styleElementName: 'none' });
            $('#Parent').val(row.parentName);
            $('#method').val('Edit');
            $('#TagID').val(row.TagID);
            $('#TagFormModal').modal('show');
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
                    console.log(row.TagID);
                    Service.Delete([row.TagID], function (data) {
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
                    Service.Recovery([row.TagID], function (data) {
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
                                //  $('#MenuFormModal').modal('show');
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
        params.data.state = $('#State').data('state');
        Service.getData(params.data, function (data) {
            console.log(data);
            params.success(data);
            params.complete();
        });
    }

    $('#State a').on('click', function () {
        $('#State').data('state', $(this).data('state'));
        $table.bootstrapTable('refresh');
        if ($('#State').val() == -3) {
            $remove.html('恢复标签').removeClass('btn-danger').addClass('btn-success');
            $remove.data('method', 'recovery');
            $('#State .text').html('已删除');
        }
        else {
            $remove.html('删除标签').removeClass('btn-success').addClass('btn-danger');
            $remove.data('method', 'delete');
            $('#State .text').html('正常');
        }
    });

    $('#TagFormModal .save').on('click', function () {
        console.log($('#method').val());
        Service.update($('#method').val(), function (data) {
            $('#TagFormModal').modal('hide');
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
                            $('#TagFormModal').modal('show');
                        } else {
                            $(this).modal('hide');
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
                        $('#TagFormModal').modal('show');
                    }
                });
            }
        });
    });

    $('#Parent').on('click focus', function () {
        $('#TagModal').modal('show');
    });
    $('#TagModal').on('show.bs.modal', function () {
        $('#TagFormModal').modal('hide');
    });
    $('#TagModal').on('hidden.bs.modal', function () {
        $('#TagFormModal').modal('show');
    });
    $('#AddTag').on('click', function () {
        $('#Tag-form')[0].reset();
        $('#method').val('Add');
        $('#TagFormModal').modal('show');
    })

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

}();