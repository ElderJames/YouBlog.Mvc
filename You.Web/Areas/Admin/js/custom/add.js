
var Script = function () {
   
    //表单自动保存和恢复
    //if ($('#title').val() == '') $('#ArticalForm').sisyphus();

    $('#Category').on('click focus', function () {
        $('#categoryModal').modal('show');
    });



    var uploadimg = new Array();

    var Service = {
        getData: function (callback) {
            $.getJSON('/Admin/Category/Tree?type=0', function (data) { callback(data); return data; });
        },
        update: function (method, callback) {
            var data=$('#ArticalForm').serialize();
            $.ajax({
                url: $('#ModelID').val()=="" ? '/Admin/Article/Add' : '/Admin/Article/Edit/' + $('#ModelID').val(),
                type: 'POST',
                data: data,
                dataType: 'json',
                success: function (result) {
                    // if (result) alert('添加成功');
                    callback(result);
                },
                error: function (result) {
                    callback(false);
                }
            });
        },
        getTags:function(callback){
            $.getJSON('/Admin/Tag/List', function (data) { callback(data); return data; });
        },
        getTranslation: function (origin, callback) {
            $.get('/Admin/Translate?query=' + origin, function (data) { callback(data); })
        }
    }

    $(function () {

        var testEditor;

        $(function () {

            //  $.get('test.md', function(md){
            testEditor = editormd("editor", {
                width: "100%",
                height: 500,
                path: '/Areas/Admin/js/lib/',
                theme: "default",
                previewTheme: "default",
                editorTheme: "default",
                markdown: $('#article-content').val(),
                codeFold: true,
                //syncScrolling : false,
                saveHTMLToTextarea: true,    // 保存 HTML 到 Textarea
                searchReplace: true,
                //watch : false,                // 关闭实时预览
                htmlDecode: "style,script,iframe|on*",            // 开启 HTML 标签解析，为了安全性，默认不开启    
                //toolbar  : false,             //关闭工具栏
                //previewCodeHighlight : false, // 关闭预览 HTML 的代码块高亮，默认开启
                emoji: true,
                //taskList: true,
                //tocm: true,         // Using [TOCM]
                //tex: true,                   // 开启科学公式TeX语言支持，默认关闭
                //flowChart: true,             // 开启流程图支持，默认关闭
                //sequenceDiagram: true,       // 开启时序/序列图支持，默认关闭,
                dialogLockScreen : false,   // 设置弹出层对话框不锁屏，全局通用，默认为true
                //dialogShowMask : false,     // 设置弹出层对话框显示透明遮罩层，全局通用，默认为true
                //dialogDraggable : false,    // 设置弹出层对话框不可拖动，全局通用，默认为true
                //dialogMaskOpacity : 0.4,    // 设置透明遮罩层的透明度，全局通用，默认值为0.1
                //dialogMaskBgColor : "#000", // 设置透明遮罩层的背景颜色，全局通用，默认为#fff
                imageUpload: true,
                imageFormats: ["jpg", "jpeg", "gif", "png", "bmp", "webp"],
                imageUploadURL: "/Admin/Attachment/FileUpload?action=uploadimage",
                onload: function () {
                    console.log('onload');
                    $.post('/Admin/Attachment/FileUpload', { action: 'config' }, function (data) {
                        console.log(data);
                    });
                    //this.fullscreen();
                    //this.unwatch();
                    //this.watch().fullscreen();
                    //this.width("100%");
                    //this.height(480);
                    //this.resize("100%", 640);
                },
                onchange: function () {
                    $('#article-content').val(this.getMarkdown());
                }
            });
        });


        //var simplemde = new SimpleMDE({
        //    element: $('#editor')[0],
        //    toolbar: ["bold", "italic", "strikethrough", "heading", "|", "code", "quote", "unordered-list", "ordered-list", "link", "image", "table", "horizontal-rule", "side-by-side", "preview", "fullscreen", "guide"],
        //    renderingConfig: {
        //        codeSyntaxHighlighting: true
        //    },
        //    parsingConfig: {
        //        allowAtxHeaderWithoutSpace: true
        //    },
        //});

        //simplemde.codemirror.on("change", function () {
        //    $('#editor').val(simplemde.value());
        //});

        //var ue = UE.getEditor('editor');
        //ue.ready(function () {
        //    ue.addListener('beforeInsertImage', function (t, arg) {
        //        console.log(arg);
        //        uploadimg = new Array();
        //        if ($('#imgpath').val() != '') uploadimg.push($('#imgpath').val());
        //        $.each(arg, function (index, item) {
        //            uploadimg.push(item.src);
        //        });
        //        $('#uploadimg').val(uploadimg.join(','));
        //    });
        //});


        //var ue_img = UE.getEditor('editor-img');
        //ue_img.ready(function () {
        //    //设置编辑器不可用
        //    //ue_img.setDisabled();
        //    //隐藏编辑器，因为不会用到这个编辑器实例，所以要隐藏
        //    ue_img.hide();
        //    //侦听图片上传
        //    ue_img.addListener('beforeInsertImage', function (t, arg) {
                
        //        //将地址赋值给相应的input,只取第一张图片的路径
        //        uploadimg.splice(jQuery.inArray($("#imgpath").val(), uploadimg), 1);
        //        $("#imgpath").attr("value", arg[0].src);
        //        uploadimg.push($("#imgpath").val());
        //        $('#uploadimg').val(uploadimg.join(','));

        //        //图片预览
        //        $(".fileupload-preview img").attr('src', arg[0].src);
        //        $('.fileupload-exists').css('display', 'inline-block');
        //        $('.fileupload-new .fileupload-new').hide();
        //    });
        //    //侦听文件上传，取上传文件列表中第一个上传的文件的路径
        //    ue_img.addListener('afterUpfile', function (t, arg) {
        //        $("#file").attr("value", _editor.options.filePath + arg[0].url);
        //    });
        //    //侦听文件上传，取上传文件列表中第一个上传的文件的路径
        //    //_editor.addListener('afterUpfile', function (t, arg) {
        //    //    $("#file").attr("value", _editor.options.filePath + arg[0].url);
        //    //})
        //});
        //弹出图片上传的对话框
        $('.choose').on('click', function () {
            var myImage = ue_img.getDialog("insertimage");
            myImage.open();
        });
        //弹出文件上传的对话框
        function upFiles() {
            var myFiles = _editor.getDialog("attachment");
            myFiles.open();
        }
        $('.delete').on('click', function () {
            $('.fileupload-exists').hide();
            $('.fileupload-new .fileupload-new').show();
            $("#imgpath").attr("value", '');
            $(".fileupload-preview img").attr("src", '');
        });

        //Array.prototype.remove = function (from, to) {
        //    var rest = this.slice((to || from) + 1 || this.length);
        //    this.length = from < 0 ? this.length + from : from;
        //    return this.push.apply(this, rest);
        //};

        //标签操作
        var addtags = new Array(), removetags = new Array(), oldtags = $('#tags').val().split(',');
        Service.getTags(function (data) {
            $('#tags').select2({
                placeholder: "请添加标签",
                tags: data,
                allowClear: true,
                tokenSeparators: [",", " ", "，"]
            })
             .on("change", function (e) {
                 if (e.added != null) {
                     //原本没有的才添加，添加旧标签相对于原本标签等于没添加
                     if ($.inArray(e.added.id, oldtags) == -1) addtags.push(e.added.text);
                     removetags.splice(removetags.indexOf(e.added.text), 1);
                 }

                 if (e.removed != null) {
                     addtags.splice(addtags.indexOf(e.removed.text), 1);
                     //原本有的才删除，删除新添加的标签相对于原本标签等于没删除
                     if ($.inArray(e.removed.id, oldtags) > -1) removetags.push(e.removed.text);
                 }
                 $('#addtags').val(addtags.join(','));
                 $('#removetags').val(removetags.join(','));
                 console.log("addtags:" + JSON.stringify(addtags) + " removetags:" + JSON.stringify(removetags));
             });
        });

        //获取文章分类树
        Service.getData(function (data) {
            var categoryID = $('#CategoryID').val();
            getCategoryName(categoryID, data)
            $('#Category').val(name);
            $('#categoryTree').treeview({
                levels: 99,
                data: data,
                onNodeSelected: function (event, node) {
                    $('#Category').val(node.text);
                    $('#CategoryID').val(node.id);
                    $('#categoryModal').modal('hide');
                }
            });
        });

        var name='';
        function getCategoryName(id,data)
        {
            for (var i = 0; i < data.length; i++) {
                if (data[i].id == id) name= data[i].text;
                if (data[i].nodes != undefined) {
                   getCategoryName(id, data[i].nodes);
                }
               
            }
        }
        console.log($('#submit').data('method'));

        $('#submit').on('click', function () {
            var dialog = BootstrapDialog.show({
                message: '正在提交，请稍候...',
                closable: false
            });
            Service.update($(this).data('method'), function (result) {
                dialog.close();
                if (result) {
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
                                window.location.href = '/Admin/Article/Add';
                            } else {
                                window.location.href = '/Admin';
                            }
                        }
                    });
                } else BootstrapDialog.alert({
                    message: "提交失败，请重试！",
                    type: BootstrapDialog.TYPE_DANGER,
                    closable: true, // <-- Default value is false
                    draggable: true,
                    buttonLabel: '确定'
                });
            });
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

    if ($('#imgpath').val() != '') {
        $(".fileupload-preview img").attr('src', $('#imgpath').val());
        $('.fileupload-exists').css('display', 'inline-block');
        $('.fileupload-new .fileupload-new').hide();
    }
}();