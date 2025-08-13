//搜索按钮
$('.search-form').on('submit', function (e) {
    e.preventDefault();
    $('#table').bootstrapTable('refresh', {
        pageNumber: 1
    });
});


document.addEventListener("click", function (e) {
    var link = e.target.closest(".js-open-offcanvas");
    if (!link) return; // 不是点击眼睛按钮就退出

    e.preventDefault();

    var title = link.getAttribute("data-title");
    var url = link.getAttribute("data-url");

    // 更新标题
    document.getElementById("offcanvasRightLabel").innerText = title;

    // 动态加载 iframe
    var body = document.querySelector("#offcanvasRight .offcanvas-body");
    body.innerHTML = `<iframe src="${url}" style="width:100%; height:100%; border:none;"></iframe>`;

    // 打开 Offcanvas
    var offcanvas = new bootstrap.Offcanvas(document.getElementById("offcanvasRight"));
    offcanvas.show();
});