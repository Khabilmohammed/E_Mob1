﻿
var datatable;
$(document).ready(function () {
    var url = window.location.search;
    if (url.includes("inprocess")) {
        loadDataTable("inprocess");
    }
    else {
        if (url.includes("completed")) {
            loadDataTable("completed");
        }
        else {
            if (url.includes("pending")) {
                loadDataTable("pending");
            }
            else {
                if (url.includes("approved")) {
                    loadDataTable("approved");
                }
                else {
                    loadDataTable("all");
                }
            }
        }
    }
    
});


function loadDataTable(status) {
    datatable = $('#tblData').DataTable({
        "ajax": { url: '/admin/order/getall?status='+ status },
        "columns": [
            { data: 'orderHeaderId', "width": "10%" },
            { data: 'name', "width": "15%" },
            { data: 'phoneNumber', "width": "15%" },
            { data: 'applicationUser.email', "width": "15%" },
            { data: 'orderStatus', "width": "10%" },
            { data: 'orderTotal', "width": "10%" },
            {
                data: 'orderDate', "width": "15%",
                render: function (data) {
                    const date = new Date(data);
                    return date.toLocaleDateString(); // Format: dd/mm/yyyy
                }
            },
            {
                data: 'orderHeaderId',
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                       <a href="/admin/order/detail?orderId=${data}" class="btn btn-primary mx-2">
                           <i class="bi bi-pencil-square"></i>
                       </a>
                    </div>`;
                },
                "width": "10%"
            }
        ]
    });
}
