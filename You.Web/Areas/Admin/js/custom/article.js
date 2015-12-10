var Script = function () {
    var $table = $('#ArticleTable'),
   $remove = $('#remove'),
   selections = [];
    var $ArticleList = new Array();
    var $ArticleTree = new Array();
    var $TableTree = new Array();
    var $DeletedArticle = new Array();
    var Service = {
        getData: function (params, callback) {
            var dialog = BootstrapDialog.show({
                message: '正在获取，请稍候...',
                closable: false
            });
            $.get('/Admin/Article/List',params, function (data) {dialog.close(); callback(data); });
        },
        update: function (method, callback) {
            var dialog = BootstrapDialog.show({
                message: '正在提交，请稍候...',
                closable: false
            });
            $.ajax({
                url: method == 'Add' ? '/Admin/Article/Add' : '/Admin/Article/Edit/' + $('#ModelID').val(),
                type: 'POST',
                data: $('#Article-form').serialize(),
                dataType: 'json',
                success: function (data) {
                dialog.close();
                    if (data.result) $('#Article-form')[0].reset();
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
            $.post("/Admin/Article/Delete", { ModelID: id }, function (data) { dialog.close();callback(data); });
        },
        Recovery: function (id, callback) {
            var dialog = BootstrapDialog.show({
                message: '正在恢复，请稍候...',
                closable: false
            });
            $.post("/Admin/Article/Recovery", { ModelID: id }, function (data) {dialog.close(); callback(data); });
        },
        RealDelete: function (id, callback) {
            var dialog = BootstrapDialog.show({
                message: '正在彻底删除，请稍候...',
                closable: false
            });
            $.post("/Admin/Article/RealDelete", { ModelID: id }, function (data) {dialog.close(); callback(data); });
        }
    }

    function getChildren(parentId, parentName, level) {
        var $list = new Array();
        $.each($ArticleList, function (index, $item) {
            if ($item.ParentId == parentId) {
                $item.id = $item.ModelID;
                $item.text = $item.Name;
                $item.parentName = parentName;
                $item.level = level;
                $list.push($item);
                $TableTree.push($item)
            }
        });
        return $list.sort(function (a, b) { return a.Order > b.Order });
    }

    function getArticleTree($nodes) {
        $.each($nodes, function (index, item) {
            var nodes = getChildren(item.id, item.Name, item.level + 1);
            if (nodes.length > 0) {
                item.nodes = nodes;
                getArticleTree(item.nodes);
            }
        });
    }

    function getArticle(params,callback) {
        Service.getData(params,function (data) {
            $DeletedArticle = new Array();
            $TableTree = new Array();
            $.each(data, function (index, item) {
                if (item.State == -3) $DeletedArticle.push(item);
                else $TableTree.push(item);
            });
            callback(data);
        });
    }


    $(function () {
        $table.bootstrapTable({
            ajax: ajaxRequest,
            //detailFormatter: detailFormatter,
            height: getHeight(),
            columns: [{
                field: 'state',
                checkbox: true,
                // rowspan: 2,
                align: 'center',
                valign: 'middle'
            }, {
                title: '标题',
                field: 'Title',
                sortable: false,
                align: 'center'
            },
             {
                 title: '栏目',
                 field: 'CategoryName',
                 sortable: false,
                 align: 'left'
             },
                    {
                        title: '录入者',
                        field: 'Inputer',
                        sortable: false,
                        align: 'left'
                    },
                       {
                           title: '概要',
                           field: 'Intro',
                           sortable: false,
                           align: 'left'
                       },
                          {
                              title: '点击量',
                              field: 'Hits',
                              sortable: false,
                              align: 'left'
                          },
                    {
                        title: '发表时间',
                        field: 'ReleaseDate',
                        sortable: false,
                        // footerFormatter: totalNameFormatter,
                        align: 'left'
                    },
                     {
                         title: '状态',
                         field: 'StatusString',
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
            //if (index % 2 == 1) {
            //    $detail.html('Loading from ajax request...');
            //    $.get('LICENSE', function (res) {
            //        $detail.html(res.replace(/\n/g, '<br>'));
            //    });
            //}
           // $detail.html(row.MenuName.join(','));
        });
        $table.on('all.bs.table', function (e, name, args) {
            console.log(name, args);
        });
        $remove.click(function () {
            var ids = getIdSelections();
            if ($remove.data('method') == 'delete') {
                BootstrapDialog.confirm({
                    title: '警告',
                    message: '确认要删除这些文章吗？',
                    type: BootstrapDialog.TYPE_DANGER, // <-- Default value is BootstrapDialog.TYPE_PRIMARY
                    closable: true, // <-- Default value is false
                    draggable: true, // <-- Default value is false
                    btnCancelLabel: '取消', // <-- Default value is 'Cancel',
                    btnOKLabel: '删除', // <-- Default value is 'OK',
                    btnOKClass: 'btn-danger', // <-- If you didn't specify it, dialog type will be used,
                    callback: function (result) {
                        // result will be true if button was click, while it will be false if users close the dialog directly.
                        if (result) {
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
                    }
                });
            }
            else {
                BootstrapDialog.confirm({
                    title: '警告',
                    message: '确认要恢复这些文章吗？',
                    type: BootstrapDialog.TYPE_DANGER, // <-- Default value is BootstrapDialog.TYPE_PRIMARY
                    closable: true, // <-- Default value is false
                    draggable: true, // <-- Default value is false
                    btnCancelLabel: '取消', // <-- Default value is 'Cancel',
                    btnOKLabel: '恢复', // <-- Default value is 'OK',
                    btnOKClass: 'btn-success', // <-- If you didn't specify it, dialog type will be used,
                    callback: function (result) {
                        // result will be true if button was click, while it will be false if users close the dialog directly.
                        if (result) {
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
            return row.ModelID;
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
        //return '1233';//row.MenuName.join(',');
    }
    function operateFormatter(value, row, index) {
        if (row.State != -3)
            return [
                '<a class="btn btn-primary btn-xs edit" href="/Admin/Article/'+row.ModelID+'" title="编辑此文章">',
                '<i class="icon icon icon-pencil"></i>',
                '</a>  ',
                '<a class="btn btn-danger btn-xs remove" href="javascript:void(0)" title="移除此文章">',
                '<i class="icon icon-remove"></i>',
                '</a>  '
            ].join('');
        else return [
            '<a class="btn btn-success btn-xs recovery" href="javascript:void(0)" title="恢复">',
            '<i class="icon-undo"></i>',
            '</a>',
             '<a class="btn btn-danger btn-xs delete" href="javascript:void(0)" title="永远删除此文章">',
                '<i class="icon icon-remove"></i>',
                '</a>  '
        ].join('');
    }

    window.operateEvents = {
        'click .edit': function (e, value, row, index) {
           // $('#Article-form')[0].reset();
            //$('#FormTitle').text('修改职位');
            //$('#Article-form').fill(row, { styleElementName: 'none' });
            //$('#Parent').val(row.parentName);
            //$('#method').val('Edit');
            //$('#ModelID').val(row.ModelID);
            //$('#ArticleFormModal').modal('show');
            // alert('You click edit action, row: ' + JSON.stringify(row));
        },
        'click .remove': function (e, value, row, index) {
            console.log(row);
            BootstrapDialog.confirm({
                title: '警告',
                message: '确认要删除[' + row.Title + ']吗？',
                type: BootstrapDialog.TYPE_DANGER, // <-- Default value is BootstrapDialog.TYPE_PRIMARY
                closable: true, // <-- Default value is false
                draggable: true, // <-- Default value is false
                btnCancelLabel: '取消', // <-- Default value is 'Cancel',
                btnOKLabel: '删除', // <-- Default value is 'OK',
                btnOKClass: 'btn-danger', // <-- If you didn't specify it, dialog type will be used,
                callback: function (result) {
                    // result will be true if button was click, while it will be false if users close the dialog directly.
                    if (result) {
                        Service.Delete([row.ModelID], function (data) {
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
                    } else {
                       // dialogRef.close();
                    }
                }
            });
        },
        'click .recovery': function (e, value, row, index) {
            BootstrapDialog.confirm({
                title: '警告',
                message: '确认要恢复[' + row.Title + ']吗？',
                type: BootstrapDialog.TYPE_DANGER, // <-- Default value is BootstrapDialog.TYPE_PRIMARY
                closable: true, // <-- Default value is false
                draggable: true, // <-- Default value is false
                btnCancelLabel: '取消', // <-- Default value is 'Cancel',
                btnOKLabel: '恢复', // <-- Default value is 'OK',
                btnOKClass: 'btn-success', // <-- If you didn't specify it, dialog type will be used,
                callback: function (result) {
                    // result will be true if button was click, while it will be false if users close the dialog directly.
                    if (result) {
                        Service.Recovery([row.ModelID], function (data) {
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
                }
            });

        },
        'click .delete': function (e, value, row, index) {
            BootstrapDialog.confirm({
                title: '警告',
                message: '删除后无法恢复，确认要彻底[' + row.Title + ']吗？',
                type: BootstrapDialog.TYPE_DANGER, // <-- Default value is BootstrapDialog.TYPE_PRIMARY
                closable: true, // <-- Default value is false
                draggable: true, // <-- Default value is false
                btnCancelLabel: '取消', // <-- Default value is 'Cancel',
                btnOKLabel: '彻底删除', // <-- Default value is 'OK',
                btnOKClass: 'btn-danger', // <-- If you didn't specify it, dialog type will be used,
                callback: function (result) {
                    // result will be true if button was click, while it will be false if users close the dialog directly.
                    if (result) {
                        Service.RealDelete([row.ModelID], function (data) {
                            if (data.result) {
                                $table.bootstrapTable('refresh');
                                BootstrapDialog.alert({
                                    message: '彻底删除成功！',
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
                }
            });
        }
    };

    function getHeight() {
        return $(window).height() - 120;

    }

    function ajaxRequest(params) {
        params.data.state = $('#State').data('state');
        console.log(params.data);
        Service.getData(params.data, function (data) {
            params.success(data);
            params.complete();
        });
    }
    $('#State a').on('click', function () {
        $('#State').data('state', $(this).data('state'));
        $table.bootstrapTable('refresh');
        if ($(this).data('state') == "Deleted") {
            $remove.html('恢复文章').removeClass('btn-danger').addClass('btn-success');
            $remove.data('method', 'recovery');
            $('#State .text').html('已删除');
        }
        else {
            $remove.html('删除文章').removeClass('btn-success').addClass('btn-danger');
            $remove.data('method', 'delete');
            $('#State .text').html('正常');
        }
    });

    $('#ArticleFormModal .save').on('click', function () {
        console.log($('#method').val());
        Service.update($('#method').val(), function (data) {
            $('#ArticleFormModal').modal('hide');
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
                            $('#ArticleFormModal').modal('show');
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
                        $('#ArticleFormModal').modal('show');
                    }
                });
            }
        });
    });

    $('#Parent').on('click focus', function () {
        $('#ArticleModal').modal('show');
    });
    $('#ArticleModal').on('show.bs.modal', function () {
        $('#ArticleFormModal').modal('hide');
    });
    $('#ArticleModal').on('hidden.bs.modal', function () {
        $('#ArticleFormModal').modal('show');
    });
  
    //$('input').iCheck({
    //    labelHover: false,
    //    cursor: true,
    //    checkboxClass: 'icheckbox_flat-green',
    //    radioClass: 'iradio_flat-green'
    //});

}();