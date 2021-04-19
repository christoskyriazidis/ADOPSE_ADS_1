//var profileString = window.localStorage.getItem('oidc.user:https://localhost:44305/:client_id_js');
//var me = JSON.parse(profileString);

//const onlineUsers = document.querySelector('#online-users');

//var connection = new signalR.HubConnectionBuilder().withUrl("https://localhost:44374/NotificationHub", {
//    accessTokenFactory: () => me.access_token
//}).build();
//var connection = new signalR.HubConnectionBuilder().withUrl("https://localhost:44374/notificationHub").build();

////Disable send button until connection is established
//document.getElementById("sendButton").disabled = true;

var connection = new signalR.HubConnectionBuilder().withUrl("https://localhost:44374/notificationHub").build();

connection.start().then(function () {
}).catch(function (err) {
    return console.error(err.toString());
});

connection.on("ReceiveWishListNotification", (asd) => {
    console.log("dsd");
});

////connection.on("testing", function (asd) {
////    console.log(asd);
////});
//console.log("asd")

//connection.on("OnlineUsers", function (users) {
//    console.log("asd")
//});

//document.getElementById("sendButton").addEventListener("click", function (event) {
//    var message = document.getElementById("messageInput").value;
//    connection.invoke("SendWishListNotification", message).catch(function (err) {
//        return alert(err.toString());
//    });
//    event.preventDefault();
//});

//document.getElementById("messageInput").addEventListener("input", function (event) {
//    connection.invoke("IamTyping").catch(function (err) {
//        return alert(err.toString());
//    });
//    event.preventDefault();
//});

//connection.on("wishListNotification", function () {
//    axios.get("https://localhost:44374/notification")
//            .then(res => {
//                console.log(res)
//            })
//            .catch(err => {
//                alert(err)
//                console.error(err);
//            })
    
//});

//const chat = document.querySelector('.chat')
//connection.on("Typing", function (user) {
//    var p = document.createElement('p');
//    p.classList.add("typing")
//    p.innerHTML = `${user} is typing`;
//    if (!document.querySelector('.typing')) {
//        chat.appendChild(p)
//    }
//    setTimeout(() => chat.removeChild(p), 2500)
//});



//connection.start().then(function () {
//    document.getElementById("sendButton").disabled = false;
//}).catch(function (err) {
//    return alert(err.toString());
//});






