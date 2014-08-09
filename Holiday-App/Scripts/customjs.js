




$(document).ready(function () {

    
    $("#StartDate").attr('type', 'text');
    $("#EndDate").attr('type', 'text');
    $('#NoOfDays').attr('readonly', 'readonly');
    $('#NoOfDays').css('background-color', '#D4D4D4');


    var select = function (dateStr) {
        var d1 = $('#StartDate').datepicker('getDate');
        var d2 = $('#EndDate').datepicker('getDate');
        var diff =0;
        if (d1 && d2) {
            if (d1.getTime() === d2.getTime()) {
                diff = parseFloat(0.5);
            }
            else
                diff = Math.floor((d2.getTime() - d1.getTime()) / 86400000); // ms per day
        }
        diff = parseFloat(diff);
        $('#NoOfDays').val(diff);
        
    }

    $("#StartDate").datepicker({
        minDate: 0,
        onSelect: select
    });
    $('#EndDate').datepicker({ minDate: 0, onSelect: select });
    


   

});