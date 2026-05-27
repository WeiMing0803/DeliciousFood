$(function () {
    $('#category-list').on('click', '.category-link', function (e) {
        e.preventDefault();
        $('#category-list .category-link').removeClass('active');
        $(this).addClass('active');
        var cid = $(this).data('id');
        loadRecipes(cid || null, 1);
    });
    $('#recipe-content').on('click', '.page-link-btn', function (e) {
        e.preventDefault();
        var page = $(this).data('page');
        var cid = $('#category-list .category-link.active').data('id');
        loadRecipes(cid || null, page);
    });

    function loadRecipes(category, page) {
        $('#recipe-content').html('<div class="text-secondary text-center p-5">加载中...</div>');
        $.get('/RecipeCategories/GetRecipeList', { category: category, offset: page || 1, limit: 12}, function (html) {
            $('#recipe-content').html(html);
        });
    }
    // 默认页不点击，已显示全部；如果要默认高亮“全部”：
    $('#category-list .category-link').first().addClass('active');
});