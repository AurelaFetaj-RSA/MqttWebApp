"use strict";

// Connect to SignalR hub
var connection = new signalR.HubConnectionBuilder()
    .withUrl("/mqttHub")
    .build();

connection.on("ReceiveMessage", function (topic, message) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    li.textContent = `Topic: ${topic}, Message: ${message}`;
});

connection.start().then(function () {
    console.log("Connected to SignalR hub!");
}).catch(function (err) {
    return console.error(err.toString());
});
