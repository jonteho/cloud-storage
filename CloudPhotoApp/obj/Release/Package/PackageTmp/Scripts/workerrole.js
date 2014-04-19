/// <reference path="jquery-1.10.2.min.js" />
/// <reference path="jquery.signalR-2.0.2.min.js" />
/// <reference path="/signalr/hubs" />

$(function () {
    
    var workerHub = $.connection.workerHub;
    
    $.connection.hub.logging = true;
    
    workerHub.client.broadcastMessage = function (msg) {
        $("#traceInformation").append("<li>" + msg + "</li>");

    };

    $.connection.hub.start().done(function () {
        
        $("#traceInformation").append("<li>" + "ASDASD" + "</li>");

    });
});