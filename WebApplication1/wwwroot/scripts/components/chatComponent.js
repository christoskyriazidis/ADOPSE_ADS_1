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
`;
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
      .then(function () {})
      .catch(function (err) {
        return console.error(err.toString());
      });
    connection.on("ReceiveMessage", (chatId) => {
      if (chatId == this.chatId) {
        this.pageNum = 1;
        this.callApi();
      }
    });
  }
  callApi = () => {
    const data = { params: { pageNumber: this.pageNum, chatId: this.chatId } };
    axios
      .get("https://localhost:44374/message", data)
      .then((res) => res.data)
      .then((data) => {
        console.log("chat", data);
        let chatUntilNow = "";
        data = data.reverse();
        for (let object of data) {
          console.log(object.username);
          if (object.subId == me.profile.sub) {
            chatUntilNow += ` <div class="chatCell" >
                                        <div class="chatCellText">${object.message}</div>
                                        </div > `;
          } else {
            chatUntilNow += ` <div class="chatCellB" >
                                        <div class="chatCellTextB">${object.username}: ${object.message}</div>
                                        </div > `;
          }
        }

        this.render(chatUntilNow);
        document
          .querySelector(".contentDisplay")
          .scrollTo(0, document.querySelector(".contentDisplay").scrollHeight);
      });
  };
  get chatId() {
    return this.getAttribute("chat-id");
  }
  get customerId() {
    return this.getAttribute("customer-id");
  }
  set pageNum(page) {
    this.setAttribute("pageNum", page);
  }
  get pageNum() {
    return this.getAttribute("pageNum");
  }
  get adId() {
    return this.getAttribute("ad-id");
  }
  get type() {
    return this.getAttribute("type");
  }
  get sold() {
    return this.getAttribute("sold");
  }
  get profileImg(){
    return this.getAttribute("profile-img");
  }
  get username(){
    return this.getAttribute("customer-username");
  }
  render = (chatContent) => {
    this.id = this.chatId;
    this.innerHTML = `
            <div class="mainChatBody showBorder">
            <div class="chatHeader ">
                <div class="profileImg"><img src="${this.profileImg}"></div>
                ${
                  this.type == "Seller"
                    ? `<span onclick="sellAd(${this.adId},${this.customerId})" class="sellAd" >Sell</span>`
                    : ``
                }
                <img class="minimizeBtn" onclick="minimize()" src="/styles/graphics/minimize.svg" alt="">
            </div>
            <div class="contentDisplay">
            ${chatContent}
            </div>
            <div class="inputDiv">
                <input type="text" class="inputForm" ${
                  this.sold == "true"
                    ? "disabled placeholder='This chat has ended'"
                    : ""
                }>
                <div class="sendBtn"><img src="/styles/graphics/sent.svg" alt=""></div>
            </div>
            </div>
        `;
    document
      .getElementById(this.id)
      .querySelector(".contentDisplay")
      .addEventListener("scroll", this.loadNextPage);
    document
      .getElementById(this.id)
      .querySelector(".minimizeBtn")
      .addEventListener("click", this.minimize);
    document
      .getElementById(this.id)
      .querySelector(".sendBtn")
      .addEventListener("click",this.sendMessage);
      
  };

  sendMessage = () => {
    if (this.sold == "true") {
      return;
    }
    let data = {
      MessageText: document.getElementById(this.id).querySelector(".inputForm")
        .value,
      ActiveChat: this.chatId,
    };
    document.getElementById(this.id).querySelector(".inputForm").value=""
    axios
      .post("https://localhost:44374/message", data)
      .then(console.log)
      .catch(console.error);
    document
      .getElementById(this.id)
      .querySelector(".contentDisplay").innerHTML += ` <div class="chatCell" >
                                    <div class="chatCellText">${data.MessageText}</div>
                                    </div > `;
    document
      .querySelector(".contentDisplay")
      .scrollTo(0, document.querySelector(".contentDisplay").scrollHeight);
  };
  minimize = () => {
    let thisChat = document.getElementById(this.chatId);
    console.log(this.chatId);
    if (!this.minimized) {
      thisChat
        .querySelector(".chatHeader")
        .classList.remove("afterClickMaximize");
      thisChat
        .querySelector(".contentDisplay")
        .classList.remove("afterClickShow");
      thisChat.querySelector(".inputDiv").classList.remove("afterClickShow");
      if (thisChat.querySelector(".chatCell"))
        thisChat.querySelector(".chatCell").classList.remove("afterClickShow");
      if (thisChat.querySelector(".chatCellB"))
        thisChat.querySelector(".chatCellB").classList.remove("afterClickShow");
      thisChat.querySelector(".mainChatBody").classList.remove("showBorder");
      thisChat.querySelector(".mainChatBody").style.backgroundColor =
        "transparent";
      thisChat.querySelector(".mainChatBody").classList.add("hideBorder");
      thisChat.querySelector(".chatHeader").classList.add("afterClickMinimize");
      thisChat.querySelector(".minimizeBtn").style.height = "20px";
      thisChat.querySelector(".contentDisplay").classList.add("afterClickHide");
      thisChat.querySelector(".inputDiv").classList.add("afterClickHide");
      if (thisChat.querySelector(".chatCell"))
        thisChat.querySelector(".chatCell").classList.add("afterClickHide");
      if (thisChat.querySelector(".chatCellB"))
        thisChat.querySelector(".chatCellB").classList.add("afterClickHide");
      this.minimized = true;
    } else {
      thisChat
        .querySelector(".chatHeader")
        .classList.remove("afterClickMinimize");
      thisChat
        .querySelector(".contentDisplay")
        .classList.remove("afterClickHide");
      thisChat.querySelector(".inputDiv").classList.remove("afterClickHide");
      if (thisChat.querySelector(".chatCell"))
        thisChat.querySelector(".chatCell").classList.remove("afterClickHide");
      if (thisChat.querySelector(".chatCellB"))
        thisChat.querySelector(".chatCellB").classList.remove("afterClickHide");
      thisChat.querySelector(".mainChatBody").classList.remove("hideBorder");
      thisChat.querySelector(".mainChatBody").style.backgroundColor = "FFF";
      thisChat.querySelector(".mainChatBody").classList.add("showBorder");
      thisChat.querySelector(".chatHeader").classList.add("afterClickMaximize");
      thisChat.querySelector(".contentDisplay").classList.add("afterClickShow");
      thisChat.querySelector(".inputDiv").classList.add("afterClickShow");
      if (thisChat.querySelector(".chatCell"))
        thisChat.querySelector(".chatCell").classList.add("afterClickShow");
      if (thisChat.querySelector(".chatCellB"))
        thisChat.querySelector(".chatCellB").classList.add("afterClickShow");
      this.minimized = false;
    }
  };
  loadNextPage = (event) => {
    console.log(this.querySelector(".contentDisplay").scrollTop)
    if (this.querySelector(".contentDisplay").scrollTop == 0) {
      this.pageNum++;
      const data = {
        params: { pageNumber: this.pageNum, chatId: this.chatId },
      };
      axios
        .get("https://localhost:44374/message", data)
        .then((res) => res.data)
        .then((data) => {
          
          let previousChat = document
            .getElementById(this.id)
            .querySelector(".contentDisplay").innerHTML;
           
          let currentChatPage = "";
          data = data.reverse();
          for (let object of data) {
            if (object.subId == me.profile.sub) {
              currentChatPage += ` <div class="chatCell" >
                                            <div class="chatCellText">${object.message}</div>
                                            </div > `;
            } else {
              currentChatPage += ` <div class="chatCellB" >
                                            <div class="chatCellTextB">${object.username}: ${object.message} </div>
                                            </div > `;
            }
          }

          console.log(data.length);
          // document
          //   .querySelector(".contentDisplay")
          //   .scrollTo(
          //     0,
          //     -1000
          //   );
          this.render(currentChatPage+previousChat);
        });
    }
  };
}
sellAd = (adId, customerId) => {
  const data = { adId: adId, buyerId: customerId };
  axios
    .post("https://localhost:44374/ad/sell", data)
    .then(() => alert(`ad #${adId} has been marked as sold`))
    .catch(console.log);
};
customElements.define("chat-component", ChatComponent);
