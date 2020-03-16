let Rentals = function() {
    let table = null;

    function init() {
        initTable();
    }

    function initTable() {
        table = $("#rentals").DataTable({
            ajax: {
                url: "/api/rentals?includeReturned=false",
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
                },
                {
                    data: "id",
                    render: function (data) {
                        return "<button class='btn-link js-return' data-rental-id=" + data + ">Return</button>";
                    },
                    orderable: false
                }
            ]
        });

        initReturn();
    }

    function initReturn() {
        $("#rentals").on("click", ".js-return", function () {
            debugger;
            var button = $(this);

            bootbox.confirm("Are you sure you want to return this rental?", function (result) {
                if (result) {
                    $.ajax({
                        url: "/api/rentals/return/" + button.attr("data-rental-id"),
                        method: "PUT",
                        success: function () {
                            table.row(button.parents("tr")).remove().draw();
                            toastr.success("Movie has successfully returned!")
                        }
                    });
                }
            });
        });
    }

    return {
        init: init
    }
}();

$(document).ready(function () {
    Rentals.init();
});