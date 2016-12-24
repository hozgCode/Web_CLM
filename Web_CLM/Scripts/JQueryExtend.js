(function ($) {
    $.getUrlParam = function (name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
        var r = window.location.search.substr(1).match(reg);
        if (r != null) return unescape(r[2]); return null;
    }

    $.forward = function (defaultURLIffails) {
        window.history.forward();
        window.location = defaultURLIffails;
    };

    $.back = function () {
        window.history.back();
    };

    $.deleteConfirm = function () {
        return $.confirm("确定要删除吗?");
    };

    $.confirm = function (confirmText) {
        return confirm(confirmText);
    };

    $.popup = function (msg, timeout) {
        var popCount = $(".__pupop").length;
        var popHeight = 0;
        if (popCount > 0) {
            popHeight = $(".__pupop")[0].clientHeight * popCount;
        }
        var msgTop = $(document).scrollTop() + 5 + popHeight;
        var html = "<div class='__pupop' style='min-width: 250px;max-width:350px; left: 45%; position: absolute;z-index:2000; top: " + msgTop + "px; font-size: 12px; background: #f8f0d2; border: 1px #eedaae solid; text-align: center; line-height: 22px; -webkit-border-radius: 3px; border-radius: 3px; -moz-border-radius: 3px;'>" + msg + "</div>";
        $(document).find("body").append(html);

        if (arguments.length == 1) {
            setTimeout(function () {
                $(".__pupop").slideUp(500, function () {
                    $(".__pupop").remove();
                });
            }, 3000);
        }
        else if (arguments.length == 2) {
            var times = parseInt(timeout) * 1000;
            setTimeout(function () {
                $(".__pupop").slideUp(500, function () {
                    $(".__pupop").remove();
                });
            }, times);
        }
    };


    //判断是否为空
    $.IsNullOrEmpty = function (str) {
        var suc = (str == undefined || str == null || str == "");
        return suc;
    }
    ///等待操作提示
    $.uploading = function (msgTip) {
        var msgTitle = "正在上传，请等待...";
        if (msgTip != undefined && msgTip != "") {
            msgTitle = msgTip;
        }
        var div_upLoading = $("#divUpLoading");
        if (div_upLoading != null && div_upLoading != undefined && div_upLoading.length > 0) {
            return;
        }
        var strHtml = "<div class='theme_popover_mask'></div>";
        strHtml += "<div class='y-tac upload_div' id='divUpLoading'>";
        strHtml += "<div class='upload_title'>" + msgTitle + "</div><div class='upload_degree'><div class='upload_percent' id='divUpPercent'></div><span class='up_percentNum'>0%</span></div>";
        strHtml += "</div>";
        $("body").append(strHtml);
        RemoveOverFlowY();
    }
    ///移除等待操作提示
    $.removeupLoading = function () {
        $(".theme_popover_mask").remove();
        $("#divUpLoading").remove();
        $("html").css({ "overflow-y": "auto" });
        $("html body").css({ "overflow-y": "auto" });
    }
    ///播放视频
    $.playVideo = function (vdoImgPath, vdoPath, divId, vdoWidth, vdoHeight) {
        if (vdoWidth == undefined) {
            vdoWidth = 230;
        }
        if (vdoHeight == undefined) {
            vdoHeight = 128;
        }
        var flashvars = {
            p: 0,
            e: 1,
            i: vdoImgPath,
            f: vdoPath
        };
        var video = [vdoPath + '->video/mp4'];
        var support = ['all'];
        CKobject.embed('/Scripts/ckplayer/ckplayer.swf', divId, 'ckplayer_a1', vdoWidth, vdoHeight, false, flashvars, video, support);
    }
    ///图片轮播预览
    $.fancyboxImg = function (fancyboxClass) {
        $("." + fancyboxClass).fancybox({
            'titlePosition': 'over',
            'nextClick': true,
            'cyclic': false,
            'titleFormat': function (title, currentArray, currentIndex, currentOpts) {
                return '<span id="fancybox-title-over">' + (currentIndex + 1) + ' / ' + currentArray.length + (title.length ? '   ' + title : '') + '</span>';
            }, helpers: {
                buttons: {
                    posistion: 'top'
                }
            }
        });
    }
    var oldHTML = $.fn.html;
    $.fn.formhtml = function () {
        if (arguments.length) return oldHTML.apply(this, arguments);
        $("input,textarea,button", this).each(function () {
            this.setAttribute('value', this.value);
        });
        $(":radio,:checkbox", this).each(function () {
            if (this.checked) this.setAttribute('checked', 'checked');
            else this.removeAttribute('checked');
        });
        $("option", this).each(function () {
            if (this.selected) this.setAttribute('selected', 'selected');
            else this.removeAttribute('selected');
        });
        return oldHTML.apply(this);
    };

    $.fn.setAttr1 = function (attrName, attrValue) {
        if ($(this).attr("type") == "checkbox" && attrName == "checked") {
            $(this).prop("checked", attrValue);
            $(this).attr("checked", attrValue);
            return;
        }

        $(this).attr("attrName", attrValue);
    };
})(jQuery);


///移除Y轴滚动条
var RemoveOverFlowY = function () {
    $("html").css({ "overflow-y": "hidden" });
    $("html body").css({ "overflow-y": "hidden" });
}
var AddForm = (function (actionName, arrayData) {

    var html = "<div id='divForm'  ><form name='submitForm' id='submitForm' method='post' action='" + actionName + "'>";

    for (var item in arrayData) {
        html += "<input id='" + item + "' name='" + item + "' value='" + arrayData[item] + "'/>";
    }

    html += "<input id='submit'  type='submit'>";
    html += "</form></div>";

    $("body").append(html);

    $("#submit").click();
    $("#divForm").html();
    $("#divForm").remove();

})