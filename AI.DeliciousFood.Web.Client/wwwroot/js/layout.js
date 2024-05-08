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

// 当您的文档加载完毕时
$(document).ready(function () {
    $('#vipModal').on('show.bs.modal', function (event) {
        var url = "Home/GetMenberPrice";

        $.ajax({
            url: url,
            type: "GET",
            success: function (response) {
                const data = response.data.result;

                const membersContainer = document.getElementById('members-container');
                const arrowContainer = document.getElementById('arrow-container');

                // 清空之前的内容
                membersContainer.innerHTML = '';

                // 添加会员信息
                Object.keys(data).forEach((key, index) => {
                    const memberPrice = data[key];

                    const memberBox = document.createElement('div');
                    memberBox.classList.add('member-box');
                    memberBox.innerHTML = `
                            <div>会员名称：${memberPrice.memberName}</div>
                            <div>会员价格：${memberPrice.price}</div>
                        `;

                    // 只有三个 div 显示在视图中
                    if (index < 3) {
                        membersContainer.appendChild(memberBox);
                    }

                    console.log(`会员价格GUID: ${memberPrice.menberPriceGuid}`);
                    console.log(`会员名称: ${memberPrice.memberName}`);
                    console.log(`价格: ${memberPrice.price}`);
                    console.log("------------------------");
                });

                // 如果超过三个会员，显示右箭头
                if (Object.keys(data).length > 3) {
                    arrowContainer.style.display = 'block';
                } else {
                    arrowContainer.style.display = 'none';
                }
            },
            error: function (xhr, status, error) {
                console.error(error);
                $('#vipModalBody').html("无法获取数据，请稍后再试。");
            }
        });
    });
});