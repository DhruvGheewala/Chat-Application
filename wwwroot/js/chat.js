"use strict";
let connection = new signalR.HubConnectionBuilder().withUrl('/Home/Index').build();

// ################# Initial #######################
$('#sendBtn').disabled = true;
$('#chatBox').hide();
$('#userData').hide();

// ############# UserData - Starts ##################
let userData = $('#userData')[0].classList;
let email = userData[0];
let firstname = userData[1];
let lastname = userData[2];
let username = firstname + lastname;
let connectionId;
let receiverEid;

let connections = {};

// ############# Fetching msgs ##################
//var __tag = $('#msgs').children();
//for (var msg of __tag)
//    console.log(msg.children[0].textContent);

// #############  ##################
let elements = $('.discussion');
var fir = true;

for (const elem of elements) {
    //search-bar
    if (fir) {
        fir = false;
        continue;
    }

    var eid = elem.id;
    // emails.push(eid);

    // Adding click event listener on each user
    var tag = document.getElementById(eid);
    tag.addEventListener('click', (ev) => {
        $('#chatBox').hide();

        var element = ev.path;
        // Removing Existing Active Message
        for (let _elem of elements)
            _elem.classList.remove('message-active');

        var done = false;
        // Iterating parent nodes, untill we get to root element for each user
        for (let node of element) {
            for (let each of node.classList) {
                if (each === 'discussion') {
                    element = node;
                    node.classList.add('message-active');

                    done = true;
                    break;
                }
            }
            if (done) break;
        }

        //here we have to first render the message between username & other user with email element.id
        // Sender's Fullname = username
        // Sender's Email = email
        // reciever's Email = element.id

        receiverEid = element.id;
        $('#RecieverEid').val(receiverEid);

        var _tag = document.getElementById(receiverEid);
        var desired_tag = (_tag.childNodes[3].childNodes[1]);
        $('#recieverName').text(desired_tag.textContent);

        render();
        $('#chatBox').show();
    });
}

$("#myLink").click(function (e) {
    e.preventDefault();
    $.ajax({
        url: $(this).attr("href"),
        success: sendMessage(),
        data: {
            reid: receiverEid,
            msg: $('#msg').val(),
            uname: email
        }
    });
});

function render() {
    var div = $('#msgs').html('');
    for (var msg of messages) {
        if (email == msg.sid && receiverEid == msg.rid) {

            var elem = document.createElement('div');
            elem.className = 'message';

            var ielem = document.createElement('div');
            ielem.className = 'response';

            var iielem = document.createElement('p');
            iielem.className = 'text';
            iielem.innerText = msg.msg;

            ielem.appendChild(iielem);
            elem.appendChild(ielem);
            div.append(elem);
        }

        if (email == msg.rid && receiverEid == msg.sid) {
            var elem = document.createElement('div');
            elem.className = 'message';


            var ielem = document.createElement('p');
            ielem.className = 'text';
            ielem.innerText = msg.msg;

            elem.appendChild(ielem);
            div.append(elem);
        }

        //console.log(msg, email, receiverEid, msg.rid, msg.sid);
        //console.log(email === msg.sid && receiverEid === msg.eid);
        //console.log(email === msg.sid && receiverEid === msg.eid);
    }

    console.log(messages);
    console.log(div);
    //$('#msgs').append(div);
}

// ################### To the Hubs #################################
function sendMessage() {
    var _conId = connections[receiverEid];
    var _msg = $('#msg').val();
    console.log(_conId, _msg, username);

    connection.invoke('SendMessage', _conId, email, _msg);

    messages.push({ sid: email, rid: receiverEid, msg: _msg });
    console.log(username, messages);
   render();
}

// ################# From the Hubs #########################
connection.on('SetConnectionId', _conid => {
    connectionId = _conid;
    connection.invoke('RegisterUser', email, connectionId);
});

connection.on('NewUserJoined', (_email, _conid) => {
    connections[_email] = _conid;
});

connection.on('ReceiveMessage', (from, msg) => {
    messages.push({ sid: from, rid: email, msg });
    console.log(username, messages);
    render();
});

//connection.on('NewUserJoined', (_email, _uname, _conid) => {

//});

// ################ Connection Startup #############################
connection.start().then(() => $('#sendBtn').disabled = false).catch(err => console.error(err.toString()));