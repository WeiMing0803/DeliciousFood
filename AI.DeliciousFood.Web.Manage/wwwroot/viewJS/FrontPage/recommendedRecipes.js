//搜索按钮
$('select[name="recommendType"]').on('change', function () {
   // e.preventDefault();
    $('#recommend-table').bootstrapTable('refresh', {
        pageNumber: 1
    });
});

var myModal = new bootstrap.Modal(document.getElementById('getTheRecipeListModel'));
$('#recommend-btn').on('click', function () {
    //重新计算模态框中的推荐数量
    initRecommendCounter('#table', '#modalRecommendCount');
    myModal.show();
});


// 事件委托绑定取消推荐按钮
$(document).on('click', '.js-cancel-recommend', function (e) {
    e.preventDefault();
    let recipeGuid = $(this).data('guid');

    showConfirm('确定要取消推荐该配方吗？', function () {
        // 确认回调
        $.post('/FrontPageManagement/CancelRecommendedRecipe', { guid: recipeGuid })
            .done(function (res) {
                if (res.success) {
                    $('#recommend-table').bootstrapTable('refresh');
                    $('#table').bootstrapTable('refresh');
                    layer.msg('取消推荐成功', { icon: 1, time: 1500 });
                } else {
                    layer.alert(res.message || '取消失败', { icon: 2 });
                }
            })
            .fail(function () {
                layer.alert('请求失败，请稍后重试', { icon: 2 });
            });
    });
});

/**
* 初始化推荐数量统计
* @param {string} tableSelector 表格选择器，如 "#table"
* @param {string} countSelector 显示统计的 div 选择器，如 "#modalRecommendCount"
*/
function initRecommendCounter(tableSelector, countSelector) {
    const recommendType = $('select[name="recommendType"]').val();
    let totalConfig = 0;

    if (recommendType === "HotList") {
        totalConfig = $('#hotListTotal').val();
    } else if (recommendType === "Monthly") {
        totalConfig = $('#monthlyTotal').val();
    }

    const mainSelected = parseInt($('#recommendCount').text().split('/')[0]) || 0;
    let remaining = totalConfig - mainSelected;
    if (remaining < 0) remaining = totalConfig;

    $(countSelector).text(`0 / ${remaining}`);

    // 绑定一次事件即可，捕捉所有选中变化
    $(tableSelector)
        .off('check.bs.table uncheck.bs.table check-all.bs.table uncheck-all.bs.table', updateCount)
        .on('check.bs.table uncheck.bs.table check-all.bs.table uncheck-all.bs.table', updateCount);

    function updateCount() {
        const checkedCount = $(tableSelector).bootstrapTable('getSelections').length;
        $(countSelector).text(`${checkedCount} / ${remaining}`);
    }
}


/**
 * 通用确认弹框（独立 layer 版）
 * @param {string} message - 提示内容
 * @param {function} onConfirm - 确定回调
 * @param {function} onCancel  - 取消回调（可选）
 */
function showConfirm(message, onConfirm, onCancel) {
    layer.confirm(message, {
        icon: 3, // 问号图标
        title: '提示'
    }, function (index) {
        if (typeof onConfirm === 'function') {
            onConfirm();
        }
        layer.close(index);
    }, function (index) {
        if (typeof onCancel === 'function') {
            onCancel();
        }
        layer.close(index);
    });
}