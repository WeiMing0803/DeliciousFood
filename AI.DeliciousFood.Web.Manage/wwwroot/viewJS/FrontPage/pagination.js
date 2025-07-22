/**
* 分页相关的配置
**/
const pagination = {
    // 分页方式：[client] 客户端分页，[server] 服务端分页
    sidePagination: "server",
    // 初始化加载第一页，默认第一页
    pageNumber: 1,
    // 每页的记录行数
    pageSize: 10,
    // 可供选择的每页的行数 - (亲测大于1000存在渲染问题)
    pageList: [5, 10, 25, 50, 100],
    // 在上百页的情况下体验较好 - 能够显示首尾页
    paginationLoop: true,
    // 展示首尾页的最小页数
    paginationPagesBySide: 2
};

/**
 * 按钮相关配置
 **/
const button = {
    // 按钮的类
    buttonsClass: 'default',
    // 类名前缀
    buttonsPrefix: 'btn'
}

/**
 * 图标相关配置
 **/
const icon = {
    // 图标前缀
    iconsPrefix: 'mdi',
    // 图标大小
    iconSize: 'mini',
    // 图标的设置
    icons: {
        paginationSwitchDown: 'mdi-door-closed',
        paginationSwitchUp: 'mdi-door-open',
        refresh: 'mdi-refresh',
        toggleOff: 'mdi-toggle-switch-off',
        toggleOn: 'mdi-toggle-switch',
        columns: 'mdi-table-column-remove',
        detailOpen: 'mdi-plus',
        detailClose: 'mdi-minus',
        fullscreen: 'mdi-monitor-screenshot',
        search: 'mdi-table-search',
        clearSearch: 'mdi-trash-can-outline'
    }
};

/**
 * 表格相关的配置
 **/
const table = {
    classes: 'table table-bordered table-hover table-striped lyear-table',
    // 请求地址
    url: '/FrontPageManagement/GetRecommendedRecipeList',
    // 唯一ID字段
    uniqueId: 'id',
    // 每行的唯一标识字段
    idField: 'id',
    // 是否启用点击选中行
    clickToSelect: true,
    // 是否显示详细视图和列表视图的切换按钮(clickToSelect同时设置为true时点击会报错)
    // showToggle: true,
    // 请求得到的数据类型
    dataType: 'json',
    // 请求方法
    method: 'get',
    // 工具按钮容器
    toolbar: '#toolbar',
    // 是否分页
    pagination: true,
    // 是否显示所有的列
    showColumns: false,
    // 是否显示刷新按钮
    showRefresh: false,
    // 显示图标
    showButtonIcons: true,
    // 显示文本
    showButtonText: false,
    // 显示全屏
    showFullscreen: false,
    // 开关控制分页
    showPaginationSwitch: false,
    // 总数字段
    totalField: 'total',
    // 当字段为 undefined 显示
    undefinedText: '-',
    // 排序方式
    sortOrder: "asc",
    ...icon,
    ...pagination,
    ...button
};

/**
 * 用于演示的列信息
 **/
const columns = [{
    field: 'example',
    checkbox: true,
    width: 5,
    widthUnit: 'rem'
}, {
    field: 'guid',
    title: '编号',
    align: 'center',
    sortable: true,
    sortName: 'sortId',
    switchable: false,
    width: 8,
    widthUnit: 'rem',
    visible: false
}, {
    field: 'recipeName',
    align: 'center',
    title: '菜谱名称',
    titleTooltip: '菜谱名称'
}, {
    field: 'userName',
    align: 'center',
    title: '作者',
}, {
    field: 'startTime',
    align: 'center',
    title: '开始时间'
}, {
    field: 'endTime',
    align: 'center',
    title: '结束时间'
}, {
    field: 'type',
    align: 'center',
    title: '类型',
    formatter: function (value, row, index) {
        return `<span class="badge bg-success">${value}</span>`;
    }
}, {
    field: 'operate',
    title: '操作',
    align: 'center',
    formatter: function (value, row, index) {
        return btnGroup(row);
    }   
}];

// 自定义操作按钮
function btnGroup(row) {
    let html =
        `<a href="#!" class="js-create-tab" data-title="${row.recipeName}" data-url="/RecipeManagement/GetRecipe?recipeGuid=${row.recipeGuid}"><i class="mdi mdi-pencil"></i></a>`;
        //+ '<a href="#!" class="btn btn-sm btn-default del-btn" title="删除" data-bs-toggle="tooltip"><i class="mdi mdi-window-close"></i></a>'
    return html;
}

$('table').bootstrapTable({
    ...table,
    // 自定义的查询参数
    queryParams: function (params) {
        const recommendType = $('select[name="recommendType"]').val();
        return {
            type: recommendType,
            // 每页数据量
            limit: params.limit,
            // sql语句起始索引
            offset: params.offset,
            page: (params.offset / params.limit) + 1,
            // 排序的列名
            sort: params.sort,
            // 排序方式 'asc' 'desc'
            sortOrder: params.order
        };
    },
    columns,
    onLoadSuccess: function (data) {
        //console.log(data); // 调试输出返回的数据
        $("[data-bs-toggle='tooltip']").tooltip();
    }
});