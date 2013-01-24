function fillCalendar(url) {
    $.ajax({
        url: url,
        type: "GET",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data, status, request) {
            alert("success");
        },
        error: function(request, status, error){
            alert("error");
        }
    });    
}

function buildCalendarTable(monthYear) {
    var calendarTable = "";
    // this array gives the weekday names
    var Weekday = new Array();
    Weekday[0] = "Sunday";
    Weekday[1] = "Monday";
    Weekday[2] = "Tuesday";
    Weekday[3] = "Wednesday";
    Weekday[4] = "Thursday";
    Weekday[5] = "Friday";
    Weekday[6] = "Saturday";
    // this array gives month names
    var MonthA = new Array();
    MonthA[0] = "January";
    MonthA[1] = "February";
    MonthA[2] = "March";
    MonthA[3] = "April";
    MonthA[4] = "May";
    MonthA[5] = "June";
    MonthA[6] = "July";
    MonthA[7] = "August";
    MonthA[8] = "September";
    MonthA[9] = "October";
    MonthA[10] = "November";
    MonthA[11] = "December";
    // this array gives the number of days in each month
    var Mdays = new Array();
    Mdays[0] = 31;
    Mdays[1] = 28;
    Mdays[2] = 31;
    Mdays[3] = 30;
    Mdays[4] = 31;
    Mdays[5] = 30;
    Mdays[6] = 31;
    Mdays[7] = 31;
    Mdays[8] = 30;
    Mdays[9] = 31;
    Mdays[10] = 30;
    Mdays[11] = 31;
    // this code gets current date information
    var Today = monthYear;
    var Date = Today.getDate();
    var Month = Today.getMonth();
    var dow = Today.getDay();
    var Year = Today.getYear();
    // these are variables for 
    var day = 1;
    var i, j;
    // adjust year for browser differences
    if (Year < 2000) {
        Year += 1900;
    }
    // account for leap year
    if ((Year % 400 == 0) || ((Year % 4 == 0) && (Year % 100 !=0)))
        Mdays[1] = 29;
    // determine day of week for first day of the month
    var Mfirst = Today;
    Mfirst.setDate(1);
    var dow1 = Mfirst.getDay();
    // construct calendar for current month
    calendarTable+="<TABLE BORDER=5 BORDERCOLOR=INDIGO>" +
		"<TR><TH COLSPAN=7 ALIGN=CENTER>" + MonthA[Month] + " " + Year + "</TH></TR>";
    calendarTable+="<TR><TH>Sun</TH><TH>Mon</TH><TH>Tue</TH><TH>Wed</TH>" +
		"<TH>Thu</TH><TH>Fri</TH><TH>Sat</TH></TR>";
    for (i = 0; i < 6; i++) {
        // this loop writes one row of days Sun-Sat
        calendarTable+="<TR>";
        for (j = 0; j < 7; j++) {
            // this loop determines which dates to display and in which column
            if ((i == 0 && j < dow1) || (day > Mdays[Month])) {
                // this puts in blank cells at the beginning an end of the month
                calendarTable+="<TD><BR></TD>";
            } else {
                if (day == Date) {
                    // highlight the current day and display the date for this cell
                    calendarTable+="<TD><FONT COLOR=red>" + day + "</FONT></TD>";
                } else {
                    // display the date for this cell
                    calendarTable+="<TD>" + day + "</TD>";
                }
                // increment day counter
                day++;
            }
        }
        // end of row; determine if more rows needed
        calendarTable+="</TR>";
        if (day > Mdays[Month]) {
            i = 6;
        }
    }
    // end of table
    calendarTable+="</TABLE>";
}