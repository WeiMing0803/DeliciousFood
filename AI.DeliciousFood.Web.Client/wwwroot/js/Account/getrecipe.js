/* 图片展示 */
$('.js-upload-images').magnificPopup({
    delegate: 'a.btn-link-pic', // 只对带有这个类的<a>标签进行图片预览
    type: 'image',
    gallery: {
        enabled: true  // 启用画廊模式（左右切换）
    }
});