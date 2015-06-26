var hidden;

function categorySubLoad(parentId) {
    hidden.val(parentId);
    $.ajax({
        url: "/admin/share/handler/goods/categories.ashx",
        type: "POST",
        data: { "pid": parentId },
        dataType: "html",
        success: function (response) {
            var second = $("ul[data-role='menu-second']");
            if (second.size() > 0) {
                second.html(response);
                if (typeof(__scid) != "undefined" && !$.isNullable(__scid)) {
                    $("ul[data-role='menu-second'] [data-value='" + __scid + "']").click();
                }
            }
        }
    });
}

function categorySubSave(cid) {
    hidden.val(cid);
}

$(function () {
    hidden = $(hid);

    if (typeof(__mcid) != "undefined" && !$.isNullable(__mcid)) {
        $("ul[data-role='menu-first'] [data-value='" + __mcid + "']").click();
    }  
});