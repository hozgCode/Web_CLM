$(function () {
    $("#CustomerTab").addClass("active");
    $("#CustomerTabTitle").addClass("active");
    $("#CustomerPage").addClass("active");
    initPagination();
    AddSaveCustomer();
    SaveCustomer();
    $("#btndel").click(DeleteCustomers);
    $("#addData #customerNumber").blur(CheckCustomer);

    $("input[name=iscklicense]").each(function () {
        $(this).change(function () {
            var ckAdmin = $(this).prop('checked');
            $(this).parent().find("input[name=islicense]").val(ckAdmin);
        });
    });
});

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
    GetCustomerList();
}

//查询用户列表
function GetCustomerList() {
    var showCount = pageIndex * pageSize;
    var pIndex
    $("#userlist").html("");
    $.post("/Customer/GetCustomerListByPage", { rowCount: showCount, pageSize: pageSize }, function (data) {
        var str = "";
        $.each(data, function (i, n) {
            str += "<tr><td><input type='checkbox' name='post[]' value='" + n.ID + "'></td>";
            str += "<td>" + n.customerNumber + "</td>";
            str += "<td>" + n.customerName + "</td>";
            str += "<td>" + n.customerAddress + "</td>";
            str += "<td>" + n.customerContact + "</td>";
            if (n.islicense) {
                str += "<td>是</td>";
            }
            else {
                str += "<td>否</td>";
            }
            str += "<td>" + n.licenceCount + "</td>";
            str += "<td>" + n.usedCount + "</td>";
            str += "<td class='text-right'><div class='btn-group'><a href='#' data-toggle='modal' data-target='#upModal' onclick=\"OpenUpModal(" + n.ID + ",'" + n.customerNumber + "','" + n.customerName + "','" + n.customerAddress + "','" + n.customerContact + "'," + n.islicense + "," + n.licenceCount + "," + n.usedCount + ")\"><i class='fa fa-pencil'></i></a></div></td></tr>";
        });
        $("#userlist").append(str);
    });
}

//检查客户是否可以注册
function CheckCustomer() {
    var $uid = $("#addData #customerNumber");
    $uid.removeClass("parsley-error").addClass("parsley-success");
    $("#ckverify").remove();
    var uid = $.trim($uid.val());
    if ($.IsNullOrEmpty(uid)) {
        return false;
    }
    $.post("/Customer/CheckCustomer", { "uid": uid }, function (data) {
        if (data != "ok") {
            $uid.removeClass("parsley-success").addClass("parsley-error");
            $uid.after("<ul id='ckverify' class='parsley-error-list' style='display: block;'><li class='required' style='display: list-item;'>" + data + "</li></ul>");
        }
    });
}
//添加用户
function AddSaveCustomer() {
    $("#addData").ajaxForm(function (data) {
        if (data.suc) {
            $("#addData .form-group input").val("");
            $("#closeModal").click();
            GetCustomerList();
        }
        else {
            alert(data.errmsg);
        }
    });
}
//修改弹层
function OpenUpModal(uid, customerNumber, customerName, customerAddress, customerContact, islicense, licenceCount, usedCount) {
    var $upform = $("#upData");
    $upform.find("#uid").val(uid);
    $upform.find("#customerNumber").val(customerNumber);
    $upform.find("#customerNumber").prop("readonly", true);
    $upform.find("#customerName").val(customerName);
    $upform.find("#customerAddress").val(customerAddress);
    $upform.find("#customerContact").val(customerContact);
    $upform.find("#licenceCount").val(licenceCount);
    $upform.find("#usedCount").val(usedCount);

    $upform.find("#islicenseVal").val(islicense);
    if (islicense) {
        $upform.find("#islicense").prop('checked', true);
        $upform.find("#islicense").parent().find("i").addClass("checked");
    }
    else {
        $upform.find("#islicense").prop('checked', false);
        $upform.find("#islicense").parent().find("i").removeClass("checked");
    }
}
//修改用户
function SaveCustomer() {
    $("#upData").ajaxForm(function (data) {
        if (parseInt(data)) {
            $("#closeUpModal").click();
            GetCustomerList();
        }
        else {
            alert(data.errmsg);
        }
    });
}
//删除用户
function DeleteCustomers() {
    var delIds = new Array();
    $("#userlist :checkbox:checked").each(function (i, d) {
        delIds.push($(d).val());
    });
    if (delIds.length == 0) {
        alert("请选择一行");
    }
    else {
        $.post("/Customer/DeleteCustomer", { "delIds": JSON.stringify(delIds) }, function (data) {
            if (parseInt(data) > 0) {
                GetCustomerList();
            }
            else {
                alert("删除失败！");
            }
        });
    }
}
