// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


$(document).ready(function() {
    // auto refresh page after 1 second
    setInterval('refreshPage()', 1800000);
});

function refreshPage() { 
    location.reload(); 
}

function updateClock() {

    var currentTime = new Date;

    var currentHours = currentTime.getHours();
    var currentMinutes = currentTime.getMinutes();
    var currentSeconds = currentTime.getSeconds();

    // Pad the minutes and seconds with leading zeros, if required
    currentMinutes = (currentMinutes < 10 ? "0" : "") + currentMinutes;
    currentSeconds = (currentSeconds < 10 ? "0" : "") + currentSeconds;

    // Convert an hours component of "0" to "12"
    currentHours = (currentHours == 0) ? 12 : currentHours;

    // Compose the string for display
    var currentTimeString = currentHours + ":" + currentMinutes;

    return currentTimeString;
}

function updateDay() {

    var currentTime = new Date;

    var currentDay = currentTime.getDate();
    var currentMonth = currentTime.getMonth();
    var currentYear = currentTime.getFullYear();

    var Months = ["Janúar", "Febrúar", "Mars", "Apríl", "Maí", "Júní", "Júlí", "Ágúst", "September", "Október", "Nóvember", "Desember"];

    // Compose the string for display
    var currentDayString = currentDay + "." + Months[currentMonth] + " " + currentYear;

    return currentDayString;
}
