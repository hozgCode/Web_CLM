$(function () {
    $("#CustomerTab").addClass("active");
    $("#CustomerTabTitle").addClass("active");
    $("#RegistrationPage").addClass("active");
    initPagination();
    GetCustomerItems();
    AddSaveRegistration();
    SaveRegistration();
    $("#btndel").click(DeleteRegistrations);
    $("#addData #RegistrationNumber").blur(CheckRegistration);
    InitDate();
});

//加载客户编号
function GetCustomerItems() {
    $.post("/Customer/GetCustomerItems", function (data) {
        var str = "";
        $.each(data, function (i, n) {
            str += "<option value=" + n.ID + ">" + n.customerNumber + "</option>";
        });
        $("#selCustomer").append(str);
    });
}
//日期控件初始化
function InitDate() {
    $(".jedateinput").each(function () {
        $(this).jeDate({
            format: "YYYY-MM-DD",
            skinCell: "jedategreen",
            isTime: false,
            minDate: $.nowDate(1)
        });
    });
    
}

var pageSize = 10;
var pageIndex = 1;
//初始化分页
function initPagination() {
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
}

function pageselectCallback(page_index, jq) {
    pageIndex = page_index;
    GetRegistrationList();
}

//查询用户列表
function GetRegistrationList() {
    var showCount = pageIndex * pageSize;
    var pIndex
    $("#userlist").html("");
    $.post("/Customer/GetRegistrationListByPage", { rowCount: showCount, pageSize: pageSize }, function (data) {
        var str = "";
        $.each(data, function (i, n) {
            str += "<tr><td><input type='checkbox' name='post[]' value='" + n.ID + "'></td>";
            str += "<td>" + n.customerNumber + "</td>";
            str += "<td>" + n.registrationKey + "</td>";
            str += "<td>" + n.registrationTime + "</td>";
            if (n.registrationState == 1) {
                str += "<td>有效</td>";
            }
            else {
                str += "<td>失效</td>";
            }
            str += "<td>" + n.failTime + "</td>";
            str += "<td>" + n.failReason + "</td>";
            str += "<td>" + n.licenceCount + "</td>";
            str += "<td>" + n.usedCount + "</td>";
            str += "<td class='text-right'><div class='btn-group'><a href='#' data-toggle='modal' data-target='#upModal' onclick=\"OpenUpModal(" + n.ID + ",'" + n.customerID + "','" + n.customerNumber + "','" + n.registrationKey + "'," + n.registrationState + ",'" + n.failTime + "','" + n.failReason + "'," + n.licenceCount + "," + n.usedCount + ")\"><i class='fa fa-pencil'></i></a></div></td></tr>";
        });
        $("#userlist").append(str);
    });
}

//检查客户是否可以注册
function CheckRegistration() {
    var $uid = $("#addData #RegistrationNumber");
    $uid.removeClass("parsley-error").addClass("parsley-success");
    $("#ckverify").remove();
    var uid = $.trim($uid.val());
    if ($.IsNullOrEmpty(uid)) {
        return false;
    }
    $.post("/Customer/CheckRegistration", { "uid": uid }, function (data) {
        if (data != "ok") {
            $uid.removeClass("parsley-success").addClass("parsley-error");
            $uid.after("<ul id='ckverify' class='parsley-error-list' style='display: block;'><li class='required' style='display: list-item;'>" + data + "</li></ul>");
        }
    });
}
//添加用户
function AddSaveRegistration() {
    $("#addData").ajaxForm(function (data) {
        if (data.suc) {
            $("#addData .form-group input").val("");
            $("#closeModal").click();
            GetRegistrationList();
        }
        else {
            alert(data.errmsg);
        }
    });
}
//修改弹层
function OpenUpModal(uid, customerID, customerNumber, registrationKey, registrationState, failTime, failReason, licenceCount, usedCount) {
    var $upform = $("#upData");
    $upform.find("#uid").val(uid);
    $upform.find("#customerID").val(customerID);
    $upform.find("#customerNumber").val(customerNumber);
    $upform.find("#registrationKey").val(registrationKey);
    $upform.find("#registrationState").val(registrationState);
    $upform.find("#failTime").val(failTime);
    $upform.find("#failReason").val(failReason);
    $upform.find("#licenceCount").val(licenceCount);
    $upform.find("#usedCount").val(usedCount);
    if (registrationState==1) {
        $upform.find("#stateShow").text("注册码有效");
        $upform.find("#ckregState").parent().find("i").addClass("checked");
    }
    else {
        $upform.find("#stateShow").text("注册码失效");
        $upform.find("#ckregState").parent().find("i").removeClass("checked");
    }
    //$upform.find("#usedCount").prop("disabled", true);

    $upform.find("#ckregState").change(function () {
        var ckAdmin = $(this).prop('checked');
        if (ckAdmin) {
            $upform.find("#registrationState").val(1);
            $upform.find("#stateShow").text("注册码有效");
        }
        else {
            $upform.find("#registrationState").val(0);
            $upform.find("#stateShow").text("注册码失效");
        }
    });
}
//修改用户
function SaveRegistration() {
    $("#upData").ajaxForm(function (data) {
        if (parseInt(data)) {
            $("#closeUpModal").click();
            GetRegistrationList();
        }
        else {
            alert(data.errmsg);
        }
    });
}
//删除用户
function DeleteRegistrations() {
    var delIds = new Array();
    $("#userlist :checkbox:checked").each(function (i, d) {
        delIds.push($(d).val());
    });
    if (delIds.length == 0) {
        alert("请选择一行");
    }
    else {
        $.post("/Customer/DeleteRegistration", { "delIds": JSON.stringify(delIds) }, function (data) {
            if (parseInt(data) > 0) {
                GetRegistrationList();
            }
            else {
                alert("删除失败！");
            }
        });
    }
}
