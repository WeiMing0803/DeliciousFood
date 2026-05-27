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


$("#forgot").on('submit',function (e) {
    e.preventDefault();

    var $btn = $("#sendEmailBtn");
    $btn.prop("disabled", true).text("正在发送...");
    $(".text-danger").text(""); // 清空错误消息

    // 提交到后端
    $.ajax({
        url: "/AccountLogon/ForgotPassword",
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({ Email: $('#forgotModel_Email').val() }),
        success: function (resp) {
            if (resp.success) {
                // 发送成功，开始倒计时
                var sec = 60;
                $btn.text("重新发送(" + sec + ")");

                var timer = setInterval(function () {
                    sec--;
                    $btn.text("重新发送(" + sec + ")");
                    if (sec === 0) {
                        clearInterval(timer);
                        $btn.prop("disabled", false).text("发送邮件");
                    }
                }, 1000);
            } else {
                // 验证失败，根据返回的数据显示错误
                $btn.prop("disabled", false).text("发送邮件");
                // 假设返回格式 resp.errors: {email: "xxx错误"}，或者有resp.message
                if (resp.errors && resp.errors.email) {
                    $("span[data-valmsg-for='forgotModel.Email']").text(resp.errors.email);
                } else if (resp.message) {
                    alert(resp.message);
                }
            }
        },
        error: function () {
            $btn.prop("disabled", false).text("发送邮件");
            alert("服务器错误，请稍后重试。");
        }
    });
});