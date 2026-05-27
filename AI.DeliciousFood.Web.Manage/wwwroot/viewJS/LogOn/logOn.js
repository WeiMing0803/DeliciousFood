var loader;
$(document).ajaxStart(function () {
    $("button:submit").html('登录中...').attr("disabled", true);
    loader = $('button:submit').lyearloading({
        opacity: 0.2,
        spinnerSize: 'nm'
    });
}).ajaxStop(function () {
    loader.destroy();
    $("button:submit").html('立即登录').attr("disabled", false);
});
$('.signin-form').on('submit', function (event) {
    if ($(this)[0].checkValidity() === false) {
        event.preventDefault();
        event.stopPropagation();
        $(this).addClass('was-validated');
        return false;
    }

    var $data = $(this).serialize();

    $.post($(this).attr('action'), $data, function (res) {
        if (res.code === 200) {
        // 这里没有后端地址，就直接假设成功
        //if (true) {
            $.notify({
                message: '登录成功，页面即将跳转~',
            }, {
                type: 'success',
                placement: {
                    from: 'top',
                    align: 'right'
                },
                z_index: 10800,
                delay: 1500,
                animate: {
                    enter: 'animate__animated animate__fadeInUp',
                    exit: 'animate__animated animate__fadeOutDown'
                }
            });
            setTimeout(function () {
                location.href = '/Home/DefaultIndex';
            }, 1500);
        } else {
            $.notify({
                message: '登录失败，错误原因：' + res.msg,
            }, {
                type: 'danger',
                placement: {
                    from: 'top',
                    align: 'right'
                },
                z_index: 10800,
                delay: 1500,
                animate: {
                    enter: 'animate__animated animate__shakeX',
                    exit: 'animate__animated animate__fadeOutDown'
                }
            });
            $('#password').val('');
            $("#captcha").click();
        }
    }).fail(function () {
        $.notify({
            message: '服务器错误',
        }, {
            type: 'danger',
            placement: {
                from: 'top',
                align: 'right'
            },
            z_index: 10800,
            delay: 1500,
            animate: {
                enter: 'animate__animated animate__shakeX',
                exit: 'animate__animated animate__fadeOutDown'
            }
        });
    });

    return false;
});