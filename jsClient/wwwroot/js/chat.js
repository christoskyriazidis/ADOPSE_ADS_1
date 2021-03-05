var profileString = window.localStorage.getItem('oidc.user:https://localhost:44305/:client_id_js');
var me = JSON.parse(profileString);

const onlineUsers = document.querySelector('#online-users');

var connection = new signalR.HubConnectionBuilder().withUrl("https://localhost:44374/chatHub", {
    accessTokenFactory: () => me.access_token
 }).build();

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = user + " says " + msg;
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li);
});

const chat = document.querySelector('.chat')
connection.on("Typing", function (user) {
    var p = document.createElement('p');
    p.classList.add("typing")
    p.innerHTML = `${user} is typing`;
    if (!document.querySelector('.typing')) {
        chat.appendChild(p)
    }
    setTimeout(() => chat.removeChild(p), 2500)
});

connection.on("OnlineUsers", function (users) {
    userBox = '';
    for (i of users) {
        userBox += `<li>${i}</li>`;
    }
    onlineUsers.innerHTML = userBox
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return alert(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", message).catch(function (err) {
        return alert(err.toString());
    });
    event.preventDefault();
});

document.getElementById("messageInput").addEventListener("input", function (event) {
    connection.invoke("IamTyping").catch(function (err) {
        return alert(err.toString());
    });
    event.preventDefault();
});


