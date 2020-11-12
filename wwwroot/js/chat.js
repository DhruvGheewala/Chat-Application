"use strict";

// Connection Builder
var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

// ##############################################################
const buttons = ['btn-caller', 'btn-id', 'btn-all'];
// Disable send button until connection is established
buttons.forEach(btn => {$('#' + btn).disabled = true;});

// ##############################################################
const user = prompt('Enter Your Name: ');
let userInfo = [];
let idToUser = {};
let userToId = {};

connection.on('UserConfig', (conId) => {
    // 
});

// ##############################################################
// User Connected/Disconnected

connection.on('UserConnected', (conId) => {
    $('#online').append($('<option></option>')
                .val(conId)
                .text(idToUser[conId]));

    // console.log('user:' + user);
    // console.log('idToUser:' + idToUser);
    // console.log('userToId:' + userToId);
    // console.log('userInfo:' + userInfo);
});

connection.on('UserDisconnected', (conId) => {
    $('#online').find('option[value=' + conId + ']').remove();
});

// ##############################################################
// Event emitted *from Hubs*
connection.on('RecieveMessageCaller', (from, to, msg) => {
    msg = `Message From Caller: ${from} -> ${to}: ${msg}`;
    $('#msgList').prepend($('<li></li>').text(msg));
});

connection.on('RecieveMessageTo', (from, to, msg) => {
    msg = `Message From To: ${from} -> ${idToUser[to]}: ${msg}`;
    $('#msgList').prepend($('<li></li>').text(msg));
});

connection.on('RecieveMessageAll', (from, to, msg) => {
    msg = `Message From All: ${from} -> ${idToUser[to]}: ${msg}`;
    $('#msgList').prepend($('<li></li>').text(msg));
});

// ##############################################################
// when connection is established, you can see sendButton
connection.start().then(() => {
    buttons.forEach(btn => {$('#' + btn).disabled = false;})
}).catch(err => {
    return console.error(err.toString());
});

// ##############################################################
// invoking event *to hubs*
function invokeHub(invokeFxn, from, to, msg) {
    connection.invoke(invokeFxn, from, to, msg).catch((err) => {
        return console.error(err.toString());
    });
}

$('#btn-caller').click(() => {
    var invokeFxn = "SendMessageCaller";
    var from = user;
    var to = from;
    var msg = $('#msg').val();
    invokeHub(invokeFxn, from, to, msg);
});

$('#btn-id').click(() => {
    var invokeFxn = "SendMessageTo";
    var from = $('#uname').val();
    var to = $('#online').val();
    var msg = $('#msg').val();

    invokeHub(invokeFxn, from, to, msg);
});

$('#btn-all').click(() => {
    var invokeFxn = "SendMessageAll";
    var from = $('#uname').val();
    var to = 'Everyone';
    var msg = $('#msg').val();
    invokeHub(invokeFxn, from, to, msg);
});