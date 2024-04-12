
document.addEventListener('DOMContentLoaded', (event) => {
     //检查 localStorage 是否有保存的显示消息的标志
    if (localStorage.getItem("toastShown") === "true") {
        const myToast = new bootstrap.Toast(document.getElementById('myToast'), {
            delay: 5000
        });
        myToast.show();

        // 设置一个变量来追踪 Toast 是否已经被移除
        let toastRemoved = false;

        // 在 5 秒后自动移除 Toast
        setTimeout(() => {
            // 如果用户没有在 5 秒内离开页面，则移除 Toast
            if (!toastRemoved) {
                myToast.hide();
                localStorage.removeItem("toastShown");
            }
        }, 5000);

        // 监听页面卸载事件，如果用户在 5 秒内离开页面，则移除 localStorage 中的标志
        window.addEventListener('beforeunload', function () {
            toastRemoved = true;
            localStorage.removeItem("toastShown");
        });
    }
});

//显示消息框
function showToastShown() {
    localStorage.setItem("toastShown", "true");
    var myToast = new bootstrap.Toast(document.getElementById('myToast'));
    myToast.show();
}

// 定义一个函数来设置 toast 消息
function showToastMessage(message) {
    var $toastBody = $('.toast-body');
    if ($toastBody.length) {
        $toastBody.html(message);
    }
}

// 封装localStorage的操作
function saveToastMessage(message) {
    localStorage.setItem('toastMessage', message);
    showToastMessage(message);
}

// 初始的toast消息处理
function initializeToastMessage() {
    var toastMessage = localStorage.getItem('toastMessage');
    if (toastMessage === null) {
        // 如果toastMessage不存在，则设置初始值并显示消息
        saveToastMessage('初始化成功！');
    } else {
        // 如果toastMessage存在，则直接显示消息
        showToastMessage(toastMessage);
    }
}

// 页面加载时初始化toast消息
initializeToastMessage();

//显示自定义提示框
function showAlert(message) {
    var modalMessage = document.getElementById('modalMessage');
    modalMessage.innerHTML = message;
    var myModal = new bootstrap.Modal(document.getElementById('customAlertModal'));
    myModal.show();
}

