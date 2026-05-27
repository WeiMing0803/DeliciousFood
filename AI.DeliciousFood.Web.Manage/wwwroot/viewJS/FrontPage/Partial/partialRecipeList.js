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

// 保存推荐菜谱按钮点击事件
$('#saveRecommendation').on('click', function () {
    // 获取选中行的数据
    var rows = $('#table').bootstrapTable('getSelections');
    if (rows.length === 0) {
        layer.msg('请先选择至少一条数据', { icon: 1, time: 1500 });
        return;
    }

    // 判断勾选的数据不能超过最大限制数量
    var countText = $('#modalRecommendCount').text().trim(); // e.g. "3 / 8"
    var parts = countText.split('/');
    var currentCount = parseInt(parts[0], 10); // 左边数字
    var maxCount = parseInt(parts[1], 10);     // 右边数字

    if (currentCount > maxCount) {
        layer.msg('推荐数量已超上限，请先取消部分已选推荐', { icon: 2, time: 2000 });
        return; // 中断后续执行
    }

    var recommendType = $('select[name="recommendType"]').val();
    var dataToSend = rows.map(function (row) {
        return {
            recipeGuid: row.recipeGuid,
            type: recommendType,
        };
    });

    $.ajax({
        url: '/FrontPageManagement/AddRecommendedRecipe',
        type: 'POST',
        data: JSON.stringify(dataToSend),
        contentType: 'application/json; charset=utf-8',
        success: function (res) {
            if (res.success) {
                // 关闭模态框
                $('#getTheRecipeListModel').modal('hide');
                // 刷新表格（可选）
                $('#recommend-table').bootstrapTable('refresh');
                $('#table').bootstrapTable('refresh');
            } else {
                layer.msg("操作失败：" + (res.message || ""), { icon: 2, time: 1500 });
            }
        },
        error: function (err) {
            console.error(err);
            layer.msg("请求出错，请稍后重试", { icon: 2, time: 1500 });
        }
    });

});