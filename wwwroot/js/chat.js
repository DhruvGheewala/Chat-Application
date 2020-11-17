"use strict";
let connection = new signalR.HubConnectionBuilder().withUrl('/chathub').build();

// ################# Initial #######################
$('#btn-send').disabled = true;
let uname, conId;
let userInfo = [];

function concatJSON(json1, json2) {
    for(var key in json2)
        json1[key] = json2[key];
    return json1;
}

// ############## On Connection/Disconnection ########################
connection.on('UserConfig', (_uname, _conId) => {
    uname = _uname;
    conId = _conId;

    $('#uname').text(uname + ', ' + conId);
});

connection.on('UserConnected', (_uname, _conId, json) => {
    userInfo.push({ uname: _uname, conId: _conId });

    if(conId !== _conId)
        $('#online').append($('<option></option>').val(_conId).text(_uname));
});

connection.on('UserDisconnected', (_conId) => {
    userInfo.forEach(user => {
        if(user['conId'] === _conId) {
            userInfo.pop(user);
            return;
        }
    });

    $('#online').find('option[value=' + _conId + ']').remove();
});

// ################### To the Hubs #################################
$('#btn-send').click(() => {
    var receiver_conId = $('#online').val();
    var msg = $('#msg').val();

    connection.invoke('SendMessage', uname, receiver_conId, msg);
});

// ################# From the Hubs #########################
connection.on('ReceiveMessage', (sender, msg) => {
    msg = `${sender}: ${msg}`;
    $('#msgList').prepend($('<li></li>').text(msg));
});

// ################ Connection Startup #############################
connection.start().then(() => $('#btn-send').disabled = false).catch(err => console.error(err.toString()));