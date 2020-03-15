let Rentals = function() {
    let table = null;

    function init() {
        initTable();
    }

    function initTable() {
        table = $("#rentals").DataTable({
            ajax: {
                url: "/api/rentals",
                dataSrc: ""
            },
            columns: [
                {
                    data: "movie.name",
                },
                {
                    data: "customer.name"
                },
                {
                    data: "dateRented"
                }
                //{
                //    data: "id",
                //    render: function (data) {
                //        return "<button class='btn-link js-delete' data-customer-id=" + data + ">Delete</button>";
                //    }
                //}
            ]
        });
    }

    return {
        init: init
    }
}();

$(document).ready(function () {
    Rentals.init();


    //$("#customers").on("click", ".js-delete", function () {
    //    var button = $(this);

    //    bootbox.confirm("Are you sure you want to delete this customer?", function (result) {
    //        if (result) {
    //            $.ajax({
    //                url: "/api/customers/" + button.attr("data-customer-id"),
    //                method: "DELETE",
    //                success: function () {
    //                    table.row(button.parents("tr")).remove().draw();
    //                }
    //            });
    //        }
    //    });
    //});
});