(function ($) {
    $.error = console.error;
    $.isNullable = function (obj) {
        if (obj.jquery)
            return obj.size() == 0;

        var _type = $.type(obj);
        if (_type == "string")
            return obj + "" == "";
        else if (_type == "array")
            return obj.length == 0;
        else if (_type == "object")
            return $.isEmptyObject(obj);
        else
            return false;
    }
})(jQuery);

function Init() {
    var links = $("a[data-module][data-action]");
    if (!!links && links.size() > 0) {
        links.each(function () {
            var _this = $(this);
            var m = _this.attr("data-module");
            var a = _this.attr("data-action");
            var p = _this.attr("data-parameters");
            var href = "/admin/modules/" + m + "/" + a + ".aspx?m=" + m + "&a=" + a;

            if (!!p && p != null && p != "") {
                if (p.indexOf(",") != -1)
                    p = p.replace(",", "&");
                href += "&" + p;
            }

            _this.attr("href", href);

            var span = _this.find("span:first");
            if (!!span && span.size() > 0 && span.next("i").size() <= 0)
                span.after("<i class='fa fa-arrow-right pull-right hide'></i>");
        });
    }

    if ($("div#show_error").size() <= 0)
        $("body").prepend("<div id='show_error' class='alert alert-warning text-center message-box' role='alert'></div>");
    
    menuInit();

    tabInit();

    layoutInit();

    if (
        typeof (__module) != "undefined"
     && typeof (__action) != "undefined"
     && !$.isNullable(__module)
     && !$.isNullable(__action)
    )
        active(__module, __action);

    if (
        typeof (__success) != "undefined"
     && typeof (__message) != "undefined"
     && !$.isNullable(__success)
     && !$.isNullable(__message)
    )
        showMessageBox(__message, __success);
}

function layoutInit() {
    $(window).resize(function () {
        var wd = $(this).width();
        var wh = $(this).height();
        var dh = $(document).height();
        $("body").css("min-height", wh + "px");
        $("#main,#sidebar").css("min-height", (dh - 50) + "px").find("#panel-body").css("min-height", (dh - 50 - 30) + "px");
        $(".message-box").css("left", ((wd - 200) / 2));
        $("nav.nav-pager").css("top", (wd - 84) + "px");
    }).resize();
}

function showMessageBox(msg, status) {
    var _show = $("div#show_error");
    if (!!_show && _show.size() > 0) {
        var s = status == 1 ? "fa fa-2x fa-check text-success" : "fa fa-2x fa-close text-danger";
        _show.html("<i class='" + s + "'></i>&nbsp;&nbsp;<strong>" + msg + "</strong>");
        if (!_show.hasClass("active"))
            _show.addClass("active");
    }
}

function active(module, action) {
    var links = $("#sidebar a[data-module][data-action]");
    if (!!links && links.size() > 0) {
        links.each(function () {
            var _this = $(this);
            var m = _this.attr("data-module");
            var a = _this.attr("data-action");
            var i = _this.find("i");

            if (m == module && a == action) {
                i.removeClass("hide");
                var parent = _this.prevAll("a.active").text();
                var href = _this.attr("href");
                var span = _this.find("span:first");
                var text;

                if (!!span && span.size() > 0) {
                    var v = span.attr("data-text");
                    if (v == null || v == "")
                        v = span.text();
                    text = v;
                }

                $("a#module")
                    .attr("href", href)
                    .text(parent)
                    .parent("li")
                    .after("<li class='active'>" + text + "</li>");
            }
            else {
                if (!i.hasClass("hide"))
                    i.addClass("hide");
            }
        });
    }
}

function menuInit() {
    var menus = $("ul[role='menu'][data-role]");
    if (menus.size() > 0) {
        menus.each(function () {
            var _this = $(this);
            var roleId = _this.attr("data-role");
            _this.delegate("[data-value]", "click", function () {
                var self = $(this);
                var value = self.attr("data-value");
                var func = self.attr("data-toggle");
                var text = self.text();
                var label = $("[data-for='" + roleId + "']");
                if (label.size() > 0) {
                    label.text(text);
                }

                if (!$.isNullable(func) && $.type(window[func]) == "function") {
                    window[func](value);
                }
            });
        });
    }
}

function tabInit() {
    var tabs = $("[data-role='tabContainer']");
    if (tabs.size() > 0) {
        tabs.each(function () {
            var _this = $(this);
            var id = _this.attr("id");
            var navs = _this.find("ul.nav[role='tablist'] a[role='tab']");
            if (navs.size() > 0) {
                if (typeof (__tabs) != "undefined" && !$.isNullable(__tabs) && !$.isNullable(id)) {
                    var current = __tabs[id]["current"];                
                    navs.filter("[aria-controls='" + current + "']").tab("show");
                }
            }
        });
    }
}

$(function () {
    Init();
});