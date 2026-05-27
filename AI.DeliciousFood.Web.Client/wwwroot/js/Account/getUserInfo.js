const $inputs = $('#userName, #email, #phoneNumber'); // 可编辑的输入框
const $editIcon = $('#editIcon'); // 编辑图标
const $cancelButton = $('#cancelButton'); // 取消按钮
const $saveButton = $('#saveButton'); // 保存按钮
const $errorMessages = {
    userName: $('#userNameError'),
    email: $('#emailError'),
    phoneNumber: $('#phoneError'),
}; // 各字段的错误提示
const $generalError = $('<div class="text-danger" id="generalError" style="display: none;"></div>'); // 全局错误提示

// 在表单顶部动态添加全局错误提示区域
$('#userForm').prepend($generalError);

let isEditing = false; // 用于跟踪是否处于编辑状态
let originalValues = {}; // 保存初始值

// 页面加载时禁用输入框和隐藏按钮
$inputs.prop('disabled', true);
$cancelButton.hide();
$saveButton.hide();

// 点击编辑图标
$editIcon.on('click', function () {
    if (isEditing) {
        // 如果处于编辑状态，恢复为禁用状态
        resetFormToInitialState();
    } else {
        // 启用输入框并保存当前值
        originalValues = {
            userName: $('#userName').val(),
            email: $('#email').val(),
            phoneNumber: $('#phoneNumber').val(),
        };
        $inputs.prop('disabled', false);
        $cancelButton.show();
        $saveButton.show();
        isEditing = true;
    }
});

// 点击取消按钮
$cancelButton.on('click', function () {
    // 恢复到初始值并禁用输入框
    resetFormToInitialState();
});


// 点击保存按钮
$saveButton.on('click', function () {
    // 获取输入框的值
    const userInfo = {
        id: $('#id').val(),
        userName: $('#userName').val(),
        email: $('#email').val(),
        phoneNumber: $('#phoneNumber').val(),
    };

    // 清除错误提示
    Object.values($errorMessages).forEach($error => $error.hide());
    $generalError.hide().text(''); // 隐藏全局错误提示

    // 发起Ajax请求
    $.ajax({
        url: "/Account/SaveUserInfo",
        method: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(userInfo),
        success: function (response) {
            if (response.success) {
                // 保存成功，禁用输入框并隐藏按钮
                $inputs.prop('disabled', true);
                $cancelButton.hide();
                $saveButton.hide();
                isEditing = false;

                saveToastMessage('保存成功！');
                showToastShown();
            } else if (response.errors) {
                // 处理错误提示
                if (response.errors.userName) {
                    $errorMessages.userName.text(response.errors.userName).show();
                }
                if (response.errors.email) {
                    $errorMessages.email.text(response.errors.email).show();
                }
                if (response.errors.phoneNumber) {
                    $errorMessages.phoneNumber.text(response.errors.phoneNumber).show();
                }
                if (response.errors.general) {
                    // 显示全局错误提示
                    $generalError.text(response.errors.general).show();
                }
            }
        },
        error: function () {
            // 其他未知错误
            $generalError.text('保存失败，请稍后重试！').show();
            saveToastMessage('保存失败，请稍后重试！');
            showToastShown();
            console.error(error);
        },
    });
});



// 创建一个函数来恢复初始状态
function resetFormToInitialState() {
    // 恢复输入框的原始值
    $('#userName').val(originalValues.userName);
    $('#email').val(originalValues.email);
    $('#phoneNumber').val(originalValues.phoneNumber);

    // 禁用输入框
    $inputs.prop('disabled', true);

    // 隐藏取消和保存按钮
    $cancelButton.hide();
    $saveButton.hide();

    // 隐藏全局错误提示
    $generalError.hide().text('');

    // 更新编辑状态
    isEditing = false;
}


//跳转到菜谱详情页
function goToDetails(e) {
    var guid = $(e).data("guid");
    window.location.href = "/Account/GetRecipe?recipeGuid=" + guid;
}