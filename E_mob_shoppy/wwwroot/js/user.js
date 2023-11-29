
var datatable;
$(document).ready(function () {
    loadDataTable();
});


function loadDataTable() {
    datatable = $('#usertbl').DataTable({
        "ajax": { url: '/admin/user/GetAll' },
        "columns": [
            { data: 'name', "width": "15%" },
            { data: 'email', "width": "20%" },
            { data: 'userName', "width": "20%" },
            { data: 'phoneNumber', "width": "10%" },
           
        ]
    });
}
