$(function () {
    initPagination();
    AddUser();
    SaveUser();
    $("#btndel").click(DeleteUsers);
    $("#addData #UserID").blur(CheckUser);
});

var pageSize = 10;
var pageIndex = 1;

function initPagination() {
    //var jPagination = function () {
    var tCount = parseInt($("#TotalCount").val());
    // 创建分页
    $("#Pagination").pagination(tCount, {
        num_edge_entries: 1, //边缘页数
        num_display_entries: 4, //主体页数
        callback: pageselectCallback,
        items_per_page: pageSize, //每页显示1项
        prev_text: "上一页",
        next_text: "下一页"
    });
    //};
    ////ajax加载
    //$("#userlist").load("/User/UserList", null, jPagination);
}

function pageselectCallback(page_index, jq) {
    pageIndex = page_index;
    GetUserList();
}

//查询用户列表
function GetUserList() {
    var showCount = pageIndex * pageSize;
    var pIndex
    $("#userlist").html("");
    $.post("/User/GetUserListByPage", { rowCount: showCount, pageSize: pageSize }, function (data) {
        var str = "";
        $.each(data, function (i, n) {
            str += "<tr><td><input type='checkbox' name='post[]' value='" + n.ID + "'></td>";
            str += "<td>" + n.userID + "</td>";
            str += "<td>" + n.userName + "</td>";
            if (n.isAdmin) {
                str += "<td>是</td>";
            }
            else {
                str += "<td>否</td>";
            }
            str += "<td>" + n.lastLoginTime + "</td>";
            str += "<td class='text-right'><div class='btn-group'><a href='#' data-toggle='modal' data-target='#upModal' onclick=\"OpenUpModal(" + n.ID + ",'" + n.userID + "','" + n.userName + "'," + n.isAdmin + ")\"><i class='fa fa-pencil'></i></a></div></td></tr>";
        });
        $("#userlist").append(str);
    });
}
//检查用户是否可以注册
function CheckUser() {
    var $uid = $("#addData #UserID");
    $uid.removeClass("parsley-error").addClass("parsley-success");
    $("#ckverify").remove();
    var uid = $.trim($uid.val());
    if ($.IsNullOrEmpty(uid)) {
        return false;
    }
    $.post("/User/CheckUser", { "UserID": uid }, function (data) {
        if (data != "ok") {
            $uid.removeClass("parsley-success").addClass("parsley-error");
            $uid.after("<ul id='ckverify' class='parsley-error-list' style='display: block;'><li class='required' style='display: list-item;'>" + data + "</li></ul>");
        }
    });
}
//添加用户
function AddUser() {
    $("#addData").ajaxForm(function (data) {
        if (data.suc) {
            $("#closeModal").click();
            GetUserList();
        }
        else {
            alert(data.errmsg);
        }
    });
}
//修改弹层
function OpenUpModal(uid, userId, uName, isAdmin) {
    var $upform = $("#upData");
    $upform.find("#uid").val(uid);
    $upform.find("#userID").val(userId);
    $upform.find("#userID").prop("readonly", true);
    $upform.find("#userName").val(uName);
    $upform.find("#isAdminVal").val(isAdmin);
    if (isAdmin) {
        $upform.find("#isAdmin").prop('checked', true);
        $upform.find("#isAdmin").parent().find("i").addClass("checked");
    }
    else {
        $upform.find("#isAdmin").prop('checked', false);
        $upform.find("#isAdmin").parent().find("i").removeClass("checked");
    }

    $upform.find("#isAdmin").change(function () {
        var ckAdmin = $(this).prop('checked');
        $upform.find("#isAdminVal").val(ckAdmin);
    });
}
//修改用户
function SaveUser() {
    $("#upData").ajaxForm(function (data) {
        if (parseInt(data)) {
            $("#closeUpModal").click();
            GetUserList();
        }
        else {
            alert(data.errmsg);
        }
    });
}
//删除用户
function DeleteUsers() {
    var delIds = new Array();
    $("#userlist :checkbox:checked").each(function (i, d) {
        delIds.push($(d).val());
    });
    if (delIds.length == 0) {
        alert("请选择一行");
    }
    else {
        $.post("/User/DeleteUser", { "delIds": JSON.stringify(delIds) }, function (data) {
            if (parseInt(data) > 0) {
                GetUserList();
            }
            else {
                alert("删除失败！");
            }
        });
    }
}
