//搜索按钮
$('.search-form').on('submit', function (e) {
    e.preventDefault();
    $('#table').bootstrapTable('refresh', {
        pageNumber: 1
    });
});