document.getElementById('saveButton').addEventListener('click', function () {
    const role = $('#originalRole').val();
    const expireAt = $('#originalExpireAt').val();
    const newRole = $('#role').val();
    const newExpireAt = $('#datepicker').val();

    const data = {
        id: $('#userId').val()
    };

    if (newRole !== role) {
        data.roleId = newRole;
    }

    if (newExpireAt !== expireAt) {
        data.membershipExpireAt = newExpireAt;
    }

    // 如果没有任何值更改，则不执行保存操作
    if (!data.roleId && !data.membershipExpireAt) {
        console.log('没有发现任何更改，保存操作被跳过');
        closeModal('editUserModal');
        return;
    }

    $.ajax({
        url: '/UserManagement/SaveUser',
        type: 'POST',
        data: data,
        success: function (response) {
            console.log('保存成功');
            // 保存成功后，刷新表格
            $('#table').bootstrapTable('refresh');
        },
        error: function (error) {
            console.error('保存失败');
        }
    });

    // 关闭模态框，可以选择在保存成功后关闭
    closeModal('editUserModal');
});

function closeModal(modalId) {
    var myModal = bootstrap.Modal.getInstance(document.getElementById(modalId));
    if (myModal) {
        myModal.hide();
    }
}

//搜索按钮
$('.search-form').on('submit', function (e) {
    e.preventDefault();
    $('#table').bootstrapTable('refresh', {
        pageNumber: 1
    });
});

