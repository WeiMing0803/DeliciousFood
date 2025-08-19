//搜索按钮
$('select[name="recommendType"]').on('change', function () {
   // e.preventDefault();
    $('#recommend-table').bootstrapTable('refresh', {
        pageNumber: 1
    });
});

var myModal = new bootstrap.Modal(document.getElementById('getTheRecipeListModel'));
$('#recommend-btn').on('click', function () {
    myModal.show();
});

// 取消推荐按钮点击事件
$(document).on('click', '.js-cancel-recommend', function (e) {
    e.preventDefault();
    let recipeGuid = $(this).data('guid');

    if (confirm('确定要取消推荐该配方吗？')) {
        $.post('/FrontPageManagement/CancelRecommendedRecipe', { guid: recipeGuid })
            .done(function (res) {
                if (res.success) {
                    $('#recommend-table').bootstrapTable('refresh');
                    $('#table').bootstrapTable('refresh');
                } else {
                    alert(res.message || '取消失败');
                }
            })
            .fail(function () {
                alert('请求失败，请稍后重试');
            });
    }
});