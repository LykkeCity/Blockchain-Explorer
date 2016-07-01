$(document).ready(function() {
    $(".show-details-load").on("click", function (e) {
        var _this = $(this), parent = _this.parent(), tx = _this.data("tx");
        _this.html('<img src="/Content/images/processing-menu.gif" />');
        $.get("/Transaction/PartialTransactionDetails/" + tx, function (data) {
            parent.html(data);
        });
        e.preventDefault();
    });
});