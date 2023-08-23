$('#tab-login').click(function () {
    var tabregister = $(this);
    tabregister.addClass('active');

    var tabLogin = $('#tab-register');
    var classValue = tabLogin.attr('class');
    var newClassValue = classValue.replace('active', '');
    tabLogin.removeClass(classValue).addClass(newClassValue);

    var pillsRegister = $('#pills-login');
    pillsRegister.addClass('show').addClass('active');

    var pillsLogin = $('#pills-register');
    classValue = pillsLogin.attr('class');
    newClassValue = classValue.replace('active', '').replace('show', '');
    pillsLogin.removeClass(classValue).addClass(newClassValue);
});

$('#tab-register').click(function () {
    register();
}); 

$('#tab-register2').click(function () {
    register();
});

function register() {
    var tabregister = $('#tab-register');
    tabregister.addClass('active');

    var tabLogin = $('#tab-login');
    var classValue = tabLogin.attr('class');
    var newClassValue = classValue.replace('active', '');
    tabLogin.removeClass(classValue).addClass(newClassValue);

    var pillsLogin = $('#pills-login');
    classValue = pillsLogin.attr('class');
    newClassValue = classValue.replace('show', '').replace('active', '');
    pillsLogin.removeClass(classValue).addClass(newClassValue);

    var pillsRegister = $('#pills-register');
    pillsRegister.addClass('show').addClass('active');
}