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
    minimized = false;
    constructor() {
        super();
        this.pageNum = 1;
        this.callApi();
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
            this.callApi();
        })
    }
    callApi = () => {
        axios.get("https://localhost:44374/message?chatid=" + this.chatId).then(res => res.data)
            .then(data => {
                console.log("chat", data);
                let chatUntilNow = ""
                data = data.reverse();
                for (let object of data) {
                    if (object.customerId == this.customerId) {
                        chatUntilNow += ` <div class="chatCell" >
                                        <div class="chatCellText">${object.message}</div>
                                        </div > `
                    } else {
                        chatUntilNow += ` <div class="chatCellB" >
                                        <div class="chatCellTextB">${object.customerId}: ${object.message}</div>
                                        </div > `
                    }
                }

                this.render(chatUntilNow);
                document.querySelector(".contentDisplay").scrollTo(0, document.querySelector(".contentDisplay").scrollHeight)
            })
    }
    get chatId() {
        return this.getAttribute("chat-id");
    }
    get customerId() {
        return this.getAttribute("customer-id");
    }
    set pageNum(page) {
        this.setAttribute("pageNum", page)
    }
    get pageNum() {
        return this.getAttribute("pageNum")
    }
    render = (chatContent) => {
        this.id = this.chatId;
        this.innerHTML = `
            <div class="mainChatBody showBorder">
            <div class="chatHeader ">
                <div class="profileImg"><img src="/styles/graphics/logo.png"></div>
                <img class="minimizeBtn" onclick="minimize()" src="/styles/graphics/minimize.svg" alt="">
            </div>
            <div class="contentDisplay">
            ${chatContent}
            </div>
            <div class="inputDiv">
                <input type="text" class="inputForm">
                <div class="sendBtn"><img src="/styles/graphics/sent.svg" alt=""></div>
            </div>
            </div>
        `
        document.getElementById(this.id).querySelector(".contentDisplay").addEventListener("scroll", this.loadNextPage)
        document.getElementById(this.id).querySelector(".minimizeBtn").addEventListener("click", this.minimize)
        document.getElementById(this.id).querySelector(".sendBtn").addEventListener("click", () => {
            let data = {
                Message: document.getElementById(this.id).querySelector(".inputForm").value,
                CustomerId: this.customerId,
                ChatId: this.chatId
            }
            axios.post("https://localhost:44374/message", data
            ).then(console.log)
                .catch(console.error)
        })
    }
    minimize = () => {
        let thisChat = document.getElementById(this.chatId);
        console.log(this.chatId)
        if (!this.minimized) {
            thisChat.querySelector(".chatHeader").classList.remove("afterClickMaximize")
            thisChat.querySelector(".contentDisplay").classList.remove("afterClickShow")
            thisChat.querySelector(".inputDiv").classList.remove("afterClickShow")
            if (thisChat.querySelector(".chatCell"))
                thisChat.querySelector(".chatCell").classList.remove("afterClickShow")
            if (thisChat.querySelector(".chatCellB"))
                thisChat.querySelector(".chatCellB").classList.remove("afterClickShow")
            thisChat.querySelector(".mainChatBody").classList.remove("showBorder")
            thisChat.querySelector(".mainChatBody").style.backgroundColor = "transparent"
            thisChat.querySelector(".mainChatBody").classList.add("hideBorder")
            thisChat.querySelector(".chatHeader").classList.add("afterClickMinimize")
            thisChat.querySelector(".minimizeBtn").style.height = "20px";
            thisChat.querySelector(".contentDisplay").classList.add("afterClickHide")
            thisChat.querySelector(".inputDiv").classList.add("afterClickHide")
            if (thisChat.querySelector(".chatCell"))
                thisChat.querySelector(".chatCell").classList.add("afterClickHide")
            if (thisChat.querySelector(".chatCellB"))
                thisChat.querySelector(".chatCellB").classList.add("afterClickHide")
            this.minimized = true;
        }
        else {
            thisChat.querySelector(".chatHeader").classList.remove("afterClickMinimize")
            thisChat.querySelector(".contentDisplay").classList.remove("afterClickHide")
            thisChat.querySelector(".inputDiv").classList.remove("afterClickHide")
            if (thisChat.querySelector(".chatCell"))
                thisChat.querySelector(".chatCell").classList.remove("afterClickHide")
            if (thisChat.querySelector(".chatCellB"))
                thisChat.querySelector(".chatCellB").classList.remove("afterClickHide")
            thisChat.querySelector(".mainChatBody").classList.remove("hideBorder")
            thisChat.querySelector(".mainChatBody").style.backgroundColor = "FFF"
            thisChat.querySelector(".mainChatBody").classList.add("showBorder")
            thisChat.querySelector(".chatHeader").classList.add("afterClickMaximize")
            thisChat.querySelector(".contentDisplay").classList.add("afterClickShow")
            thisChat.querySelector(".inputDiv").classList.add("afterClickShow")
            if (thisChat.querySelector(".chatCell"))
                thisChat.querySelector(".chatCell").classList.add("afterClickShow")
            if (thisChat.querySelector(".chatCellB"))
                thisChat.querySelector(".chatCellB").classList.add("afterClickShow")
            this.minimized = false;
        }
    }
    loadNextPage = (event) => {
        if (this.querySelector(".contentDisplay").scrollTop == 0) {
            axios.get("https://localhost:44374/message?chatid=" + this.chatId + "&pageNumber=" + this.pageNum).then(res => res.data)
                .then(data => {

                    this.pageNum++;
                    const previousChat = document.getElementById(this.id).querySelector(".contentDisplay").innerHTML;
                    let currentChatPage = "";
                    data = data.reverse();
                    for (let object of data) {
                        if (object.customerId == this.customerId) {
                            currentChatPage += ` <div class="chatCell" >
                                            <div class="chatCellText">${object.message} PAGE:${this.pageNum}</div>
                                            </div > `
                        } else {
                            currentChatPage += ` <div class="chatCellB" >
                                            <div class="chatCellTextB">${object.customerId}: ${object.message} PAGE:${this.pageNum}</div>
                                            </div > `
                        }
                    }
                    
                    console.log(data.length);
                    document.querySelector(".contentDisplay").scrollTo(0, -document.querySelector(".contentDisplay").scrollHeight)
                    this.render(currentChatPage + previousChat)
                })
        }
    }
}

customElements.define("chat-component", ChatComponent)

