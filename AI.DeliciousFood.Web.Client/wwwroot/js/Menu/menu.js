var currentValue = 2;
var uploadedFiles = [];
var html;

/*-----------------------图片上传------------------------------*/
$("#input-ke-2").fileinput({
    language: 'zh',                                          // 多语言设置，需要引入local中相应的js，例如locales/zh.js
    theme: "explorer-fa",                               // 主题
    uploadUrl: '/menu/UploadImage',         // 上传地址
    allowedFileExtensions: ['jpg', 'png', 'gif', 'jpeg'],//允许的文件类型
    minFileCount: 1,                                        // 最小上传数量
    maxFileCount: 5,                                        // 最大上传数量
    overwriteInitial: false,                        // 覆盖初始预览内容和标题设置
    showCancel: false,                                       // 显示取消按钮
    showZoom: false,                                         // 显示预览按钮
    showCaption: false,                                  // 显示文件文本框
    dropZoneEnabled: false,                          // 是否可拖拽
    uploadLabel: "上传附件",                         // 上传按钮内容
    browseLabel: '选择附件',                            // 浏览按钮内容
    showRemove: false,                                       // 显示移除按钮
    //browseClass: "layui-btn",                        // 浏览按钮样式
    //uploadClass: "layui-btn",                        // 上传按钮样式
    //uploadExtraData: { 'taskId': 1, 'createBy': 1, 'createByname': 1 },   // 上传数据
    hideThumbnailContent: true,                  // 是否隐藏文件内容
    fileActionSettings: {                               // 在预览窗口中为新选择的文件缩略图设置文件操作的对象配置
        showRemove: true,                                   // 显示删除按钮
        showUpload: true,                                   // 显示上传按钮
        showDownload: false,                            // 显示下载按钮
        showZoom: true,                                    // 显示预览按钮
        showDrag: false,                                        // 显示拖拽
        //removeIcon: '<i class="fa fa-trash"></i>',   // 删除图标 
        //uploadIcon: '<i class="fa fa-upload"></i>',     // 上传图标
        //uploadRetryIcon: '<i class="fa fa-repeat"></i>'  // 重试图标
    },
});
//每上传一个文件成功，触发一次此事件
$("#input-ke-2").on("fileuploaded", function (event, data, previewId, index) {
    var response = data.response;
    if (response) {
        //console.log('上传成功: ' + response.fileName);
        //console.log('数据: ' + response.fileBytes);
        //console.log('数据: ' + previewId);

        // 保存文件信息到数组
        uploadedFiles.push({
            fileName: response.fileName,
            fileBytes: response.fileBytes,
            previewId: previewId // 将 previewId 也保存起来，以便将来删除
        });
    } else {
        console.log('上传完成，但没有返回数据');
    }
});
// 上传成功回调,当所有文件上传成功，此事件触发一次
//$("#input-ke-2").on("filebatchuploadcomplete", function (event, files, extra) {
//    var data = extra.response; // 获取上传成功之后返回的数据
//    if (data) {
//        alert('上传成功: ' + data.Message); // 显示返回的消息
//        // 如果您想对数据做进一步的处理，可以在这里添加代码
//    }
//});
// 上传失败回调
$('#input-ke-2').on('fileerror', function (event, data, msg) {

});

$('#input-ke-2').on('filesuccessremove', function (event, previewId) {
    var fileIndex = uploadedFiles.findIndex(file => file.previewId === previewId);
    if (fileIndex !== -1) {
        uploadedFiles.splice(fileIndex, 1);
        //console.log('文件已从列表中删除: ' + previewId);
    }
});

/*-----------------------富文本框------------------------------*/
const { createEditor, createToolbar } = window.wangEditor

const editorConfig = {
    placeholder: 'Type here...',
    onChange(editor) {
        html = editor.getHtml()
        //console.log('editor content', html)
        // 也可以同步到 <textarea>
    }
}

const editor = createEditor({
    selector: '#editor-container',
    html: '<p><br></p>',
    config: editorConfig,
    mode: 'default', // or 'simple'
})

//从数据库读取值进行赋值操作
const content = document.querySelector('#editor-container').getAttribute('data-practice')
if (content) {
    editor.setHtml(content)
}

const toolbarConfig = {}

const toolbar = createToolbar({
    editor,
    selector: '#toolbar-container',
    config: toolbarConfig,
    mode: 'default', // or 'simple'
})


/*-----------------------食材明细------------------------------*/
$(document).on('click', '.icon', function (e) {
    var currentId = $(this).attr('id');
    if (currentId === 'ai-undiminished') {
        return;
    }
    if (currentId === 'ai-add') {
        var current_page_div = $(this).parents('.row').eq(1).attr('id');
        $('#' + current_page_div).after(`<div class="row mb-1 ai-ingredientsDetails"><div class="col-md-6"><input type="text" class="form-control" placeholder="食材名称"></div><div class="col-md-2"><input type="text" class="form-control" placeholder="用量"></div><div class="col-md-2"><div class="row"><div class="col-md-2"><svg t="1697177914495" class="icon" id="ai-reduce" viewBox="0 0 1024 1024" version="1.1" xmlns="http://www.w3.org/2000/svg" p-id="15671" width="32" height="32"><path d="M960.031235 159.921913v703.656418c0 52.974134-42.979014 95.953148-95.953148 95.953148h-703.656418c-52.974134 0-95.953148-42.979014-95.953148-95.953148v-703.656418c0-52.974134 42.979014-95.953148 95.953148-95.953148h703.656418c52.974134 0 95.953148 42.979014 95.953148 95.953148z m-831.593949-159.921913C57.771791 0 0.499756 57.272035 0.499756 127.937531v767.625183c0 70.665495 57.272035 127.937531 127.93753 127.93753h767.625183c70.665495 0 127.937531-57.272035 127.937531-127.93753v-767.625183c0-70.665495-57.272035-127.937531-127.937531-127.937531h-767.625183z" p-id="15672" fill="#d81e06"></path><path d="M799.609566 544.234261H223.890678c-17.691362 0-31.984383-14.293021-31.984382-31.984383s14.293021-31.984383 31.984382-31.984383h575.718888c17.691362 0 31.984383 14.293021 31.984382 31.984383s-14.293021 31.984383-31.984382 31.984383z" p-id="15673" fill="#d81e06"></path></svg></div><div class="col-md-2"><svg t="1697178067934" class="icon" id="ai-add" viewBox="0 0 1024 1024" version="1.1" xmlns="http://www.w3.org/2000/svg" p-id="18351" width="32" height="32"><path d="M960.031235 159.921913v703.656418c0 52.974134-42.979014 95.953148-95.953148 95.953148h-703.656418c-52.974134 0-95.953148-42.979014-95.953148-95.953148v-703.656418c0-52.974134 42.979014-95.953148 95.953148-95.953148h703.656418c52.974134 0 95.953148 42.979014 95.953148 95.953148z m-831.593949-159.921913C57.771791 0 0.499756 57.272035 0.499756 127.937531v767.625183c0 70.665495 57.272035 127.937531 127.93753 127.93753h767.625183c70.665495 0 127.937531-57.272035 127.937531-127.93753v-767.625183c0-70.665495-57.272035-127.937531-127.937531-127.937531h-767.625183z" p-id="18352" fill="#3b7bff"></path><path d="M831.593948 513.149439c-0.499756 17.391508-15.192582 31.084822-32.684041 31.084822H559.726696c-8.795705 0-15.992191 7.196486-15.992191 15.992191v239.183211c0 17.391508-13.693314 32.184285-31.084822 32.684041-18.091166 0.499756-32.883943-13.993167-32.883944-31.984382V560.226452c0-8.795705-7.196486-15.992191-15.992191-15.992191H224.590337c-17.391508 0-32.184285-13.693314-32.684041-31.084822-0.499756-18.091166 13.993167-32.883943 31.984382-32.883944h239.88287c8.795705 0 15.992191-7.196486 15.992191-15.992191V225.090093c0-17.391508 13.693314-32.184285 31.084822-32.684041 18.091166-0.499756 32.883943 13.993167 32.883944 31.984382v239.88287c0 8.795705 7.196486 15.992191 15.992191 15.992191h239.88287c17.991215 0 32.484139 14.792777 31.984382 32.883944z" p-id="18353" fill="#3b7bff"></path></svg></div></div></div></div>`);
    } else if (currentId === 'ai-reduce') {
        $(this).parents('.row').eq(1).remove();
    }
    e.stopPropagation(); //阻止冒泡事件
})

/*-----------------------提交食谱------------------------------*/
// 提交审核按钮
$('#submitReview').on('click', function (event) {
    handleMenuSubmit(event, false);
});

// 保存草稿按钮
$('#saveDraft').on('click', function (event) {
    handleMenuSubmit(event, true);
});

function handleMenuSubmit(event, isDraft) {
    event.preventDefault(); // 阻止表单默认提交行为

    var menuData = {
        IsDraft: isDraft,
        RecipeGuid: "",
        RecipeName: $('#recipeName').val(),
        Description: $('#description').val(),
        ProductionDifficulty: $('.radio-group input[name="difficulty"]:checked').next('label').text(),
        NeedsTime: $('#timeOutput').val(),
        Taste: $('#taste').val(),
        CookingCraft: $('#cookingCraft').val(),
        KitchenUtensils: $('#kitchenUtensils').val(),
        Tips: $('#tips').val(),
        Steps: html,
        IngredientsDetails: getIngredients(),
        Files: uploadedFiles,
        DeletedFiles: []
    }

    // 如果不是草稿，需要校验
    if (isDraft) {
        if (!menuData.RecipeName) {
            var errorMessage = `<ul><li>请输入菜谱名称</li></ul>`
        }            
    } else {
        var errorMessage = validateMenuData(menuData);
    }

    if (errorMessage) {
        showAlert(errorMessage);
        return;
    }

    $.ajax({
        url: '/Menu/SaveMenu', // 这里可以根据需要区分 URL
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(menuData),
        success: function (response) {
            saveToastMessage(isDraft ? '草稿保存成功！' : '操作成功！');
            showToastShown();
            window.location.href = response.redirectUrl;
        },
        error: function (error) {
            console.error(error);
        }
    });
}

// 获取所有食材的名称和用量
function getIngredients() {
    var ingredients = [];
    $('.row.mb-1.ai-ingredientsDetails').each(function () {
        var name = $(this).find('.col-md-6 input').val();
        var quantity = $(this).find('.col-md-2 input').val();
        ingredients.push({ name: name, quantity: quantity });
    });
    return ingredients;
}


// 给每个输入框绑定失去焦点事件
$(document).on('blur', '#recipeName, #description, #taste, #cookingCraft, #kitchenUtensils, #tips', function () {
    if ($(this).val() === '') {
        $(this).css('border-color', 'red');
    }
});

// 给每个输入框绑定点击事件
$(document).on('click', '#recipeName, #description, #taste, #cookingCraft, #kitchenUtensils, #tips', function () {
    $(this).css('border-color', '');
});

function validateMenuData(menu) {
    const checks = [
        { condition: !menu.RecipeName, message: "请输入菜谱名称" },
        { condition: menu.Files == null || menu.Files.length === 0, message: "请上传图片" },
        { condition: !menu.Description, message: "请输入菜品描述" },
        { condition: !menu.Taste, message: "请选择口味" },
        { condition: !menu.CookingCraft, message: "请选择烹饪工艺" },
        { condition: !menu.KitchenUtensils || menu.KitchenUtensils.length === 0, message: "请选择使用厨具" },
        {
            condition: !menu.IngredientsDetails ||
                menu.IngredientsDetails.length === 0 ||
                menu.IngredientsDetails[0].name === '' ||
                menu.IngredientsDetails[0].quantity === '',
            message: "请输入食材明细"
        },
        { condition: !menu.Steps || menu.Steps === "<p><br></p>", message: "请输入做法步骤" },
        { condition: !menu.Tips, message: "请输入小窍门" } 
    ];

    let errorMessage = "";
    checks.forEach((check) => {
        if (check.condition) {
            errorMessage += `<li>${check.message}</li>`;
        }
    });

    return errorMessage ? `<ul>${errorMessage}</ul>` : "";
}