$(document).ready(function () {
    // 获取初始激活的表单
    var activeForm = $("#formContainer").data("active-form") || "login";

    // 初始化所有表单隐藏 toggle
    $("#login, #register, #forgot").removeClass("toggle");

    // 初始化根据 activeForm 显示
    if (activeForm === "register") {
        $("#register").addClass("toggle");
        $("#formContainer").addClass("toggle"); // 只有注册页需要增加高度
    } else if (activeForm === "forgot") {
        $("#forgot").addClass("toggle");
        // 不需要改变 formContainer 高度
    }

    // 点击“新用户”按钮
    $('.registerBtn').on('click', function () {
        $("#register").addClass("toggle");
        $("#forgot").removeClass("toggle");
        $("#formContainer").addClass("toggle"); // 注册页需要加高
    });

    // 注册页里的“返回”按钮
    $('.registerBtn.return').on('click', function () {
        $("#register").removeClass("toggle");
        $("#formContainer").removeClass("toggle"); // 注册页返回 → 恢复高度
    });

    // 点击“忘记密码”按钮
    $('.forgotBtn').on('click', function () {
        $("#forgot").addClass("toggle");
        $("#register").removeClass("toggle");
        // 注意：不改变 formContainer 高度
    });

    // 忘记密码页里的“返回”按钮
    $('.forgotBtn.return').on('click', function () {
        $("#forgot").removeClass("toggle");
        // 忘记密码页返回 → 无需改变 formContainer 高度
    });
});
