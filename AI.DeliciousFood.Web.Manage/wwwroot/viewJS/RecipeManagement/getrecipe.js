// 图片展示
$('.js-upload-images').magnificPopup({
    delegate: 'a.btn-link-pic', // 只对带有这个类的<a>标签进行图片预览
    type: 'image',
    gallery: {
        enabled: true  // 启用画廊模式（左右切换）
    }
});

// 文本域使用
$("textarea#textarea").maxlength({
    threshold: 255,
    warningClass: "badge bg-info",
    limitReachedClass: "badge bg-warning"
});


var myModal = new bootstrap.Modal(document.getElementById('recipeReviewRejectModal'));
var approvalStatus = false;

// 审核通过事件
$(".btn-outline-primary").click(function () {
    $('#recipeReviewRejectModalLabel').text($('#recipeName').text());
    $('#recipeReviewApproval').show();
    $('#textarea').hide();
    approvalStatus = true;
    myModal.show();
});

// 审核驳回事件
$(".btn-outline-danger").click(function () {
    $('#recipeReviewRejectModalLabel').text($('#recipeName').text());
    $('#recipeReviewApproval').hide();
    $('#textarea').show();
    approvalStatus = false;
    myModal.show();
});

// 提交审核驳回
$('#submitButton').click(function () {
    const recipe = {
        recipeGuid: $('#recipeGuid').val(),
        Comment: $('#textarea').val(),
        Approval: approvalStatus
    };

    $.ajax({
        url: "/RecipeManagement/SaveRecipeComment",
        method: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(recipe),
        success: function (response) {
            myModal.hide();
            window.location.href = "/RecipeManagement/GetRecipe?recipeGuid=" + $('#recipeGuid').val();
        },
        error: function () {
        },
    });
});