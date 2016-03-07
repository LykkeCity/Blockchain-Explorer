$(document).ready(function() {
    $(".show-details-load").on("click", function (e) {
        var _this = $(this), parent = _this.parent(), tx = _this.data("tx");
        _this.html('<img src="/images/processing-menu.gif" />');
        $.post("/Block/TransactionDetails/" + tx, function (data) {
            parent.html(data);
        });
        e.preventDefault();
    });
});