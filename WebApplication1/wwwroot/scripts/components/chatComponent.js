html = `
        <div class="mainChatBody showBorder">
        <div class="chatHeader ">
            <div class="profileImg"><img src="logo.png"></div>
            <img class="minimizeBtn" src="minimize.svg" alt="">
        </div>
        <div class="contentDisplay">
        </div>
        <div class="inputDiv">
            <input type="text" class="inputForm">
            <div class="sendBtn"><img src="sent.svg" alt=""></div>
        </div>
        </div>
`
class ChatComponent extends HTMLElement {
    constructor() {
        super();
        axios.get("https://localhost:44374/message?chatid=3").then(res => res.data)
            .then(data => {
                let chatUntilNow = ""
                for (let object of data) {
                    if (object.customerId == this.customerId) {
                        chatUntilNow += ` <div class="chatCell" >
                                        <div class="chatCellText">${message}</div>
                                        </div > `
                    } else {
                        chatUntilNow += ` <div class="chatCellB" >
                                        <div class="chatCellTextB">${username}: ${message}</div>
                                        </div > `
                    }
                }
                this.render(chatUntilNow);
            })
        var connection = new signalR.HubConnectionBuilder()
            .withUrl("https://localhost:44374/chathub", {
                accessTokenFactory: () => me.access_token,
            })
            .build();

        connection
            .start()
            .then(function () { })
            .catch(function (err) {
                return console.error(err.toString());
            });
        connection.on("ReceiveMessage", (username, message) => {
            let chatCell = `<div class="chatCellB">                    
                            <div class="chatCellTextB">${username}: ${message}</div>
                        </div>`
            messageBody.append(chatCell)
        })
    }

    get chatId() {
        return this.getAttribute("chat-id");
    }
    get customerId() {
        return this.getAttribute("customer-id");
    }
    render = (chatContent) => {
        this.innerHTML = `
            <div class="mainChatBody showBorder">
            <div class="chatHeader ">
                <div class="profileImg"><img src="logo.png"></div>
                <img class="minimizeBtn" src="minimize.svg" alt="">
            </div>
            <div class="contentDisplay">
            ${chatContent}
            </div>
            <div class="inputDiv">
                <input type="text" class="inputForm">
                <div class="sendBtn"><img src="sent.svg" alt=""></div>
            </div>
            </div>
        `
    }
}
customElements.define("chat-component", ChatComponent)