$(document).ready(function () {
    var $thumbs = $('.detail-thumb-photo');
    var $main = $('#mainDishPhoto');

    $thumbs.on('click', function () {
        $main.attr('src', $(this).attr('src'));
        $thumbs.removeClass('thumb-active');
        $(this).addClass('thumb-active');
    });

    // 页面打开时默认第一个缩略图高亮
    $thumbs.first().addClass('thumb-active');
});