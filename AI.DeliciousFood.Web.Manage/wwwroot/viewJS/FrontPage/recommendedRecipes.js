//搜索按钮
$('.search-form').on('submit', function (e) {
    e.preventDefault();
    $('#recommend-table').bootstrapTable('refresh', {
        pageNumber: 1
    });
});

var myModal = new bootstrap.Modal(document.getElementById('getTheRecipeListModel'));
$('#recommend-btn').on('click', function () {
    myModal.show();
});