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
        var url = "Alipay/GetMenberPrice";

        $.ajax({
            url: url,
            type: "GET",
            success: function (response) {
                const data = response.data.result;

                const membersContainer = document.getElementById('members-container');
                const membersQRCode = document.getElementById('members-QRCode');

                // 清空之前的内容
                membersContainer.innerHTML = '';

                // 添加会员信息
                Object.keys(data).forEach((key, index) => {
                    const memberPrice = data[key];

                    const memberBox = document.createElement('div');
                    memberBox.classList.add('member-box');

                    //设置默认的支付二维码
                    if (index === 0) {
                        $.ajax({
                            url: "Alipay/AlipayTradePrecreate",
                            type: 'GET',
                            data: { memberPriceGuid: memberPrice.memberPriceGuid },
                            success: function (data2) {
                                // 创建支付宝预支付二维码
                                if (data2.success) {
                                    const qrCode = document.createElement('div');
                                    qrCode.innerHTML = `
                                            <div>
                                                <span>扫描二维码支付</span>
                                                <span class="currency">￥</span>
                                                <span class="amount">${memberPrice.price}</span>                            
                                            </div>
                                             <img src="https://quickchart.io/qr?text=${encodeURIComponent(data2.data)}" />
                                        `;
                                    membersQRCode.appendChild(qrCode);
                                }
                            },
                            error: function (error2) {
                                console.error("Second request error:", error2);
                            }
                        });
                        memberBox.classList.add('member-select');                       
                    }


                    memberBox.setAttribute('data-id', memberPrice.memberPriceGuid);
                    memberBox.innerHTML = `
                            <div>${memberPrice.memberName}</div>
                            <div>
                                <span class="currency">￥</span>
                                <span class="amount">${memberPrice.price}</span>                            
                            </div>
                        `;
                    //添加点击监听事件
                    memberBox.addEventListener('click', function () {
                        changeOnActive(this);
                    });
                    membersContainer.appendChild(memberBox);

                    console.log(`${index}`);
                    console.log(`会员价格GUID: ${memberPrice.memberPriceGuid}`);
                    console.log(`会员名称: ${memberPrice.memberName}`);
                    console.log(`价格: ${memberPrice.price}`);
                    console.log("------------------------");
                });
            },
            error: function (xhr, status, error) {
                console.error(error);
                $('#members-container').html("无法获取数据，请稍后再试。");
            }
        });


    });

    function changeOnActive(element) {

        // 移除之前设置的 'member-select'
        const previouslySelected = document.querySelector('.member-select');
        if (previouslySelected) {
            previouslySelected.classList.remove('member-select');
        }
        // 给当前的 div 添加 class 样式 'member-select'
        element.classList.add('member-select');

        console.log(element);
        var id = $(element).data('id');
        console.log(id);
    }
});


var connection = new WebSocket('ws://' + window.location.host + '/ws');

connection.onmessage = function (event) {
    console.log('Payment status:', event.data);
    if (event.data === '支付成功') {
        alert('支付成功');
    }
};

connection.onerror = function (error) {
    console.error('WebSocket error:', error);
};


// 断开连接，如果页面关闭
//window.onbeforeunload = function () {
//    connection.close();
//};