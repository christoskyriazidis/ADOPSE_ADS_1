class ChatDropdown extends HTMLElement {
  get customerId() {
    return this.getAttribute("customer-id");
  }

  connectedCallback() {}
  constructor() {
    super();
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
    connection.on("ReceiveActiveChat", (subId) => {
      this.callApi();
    });
    this.callApi();
  }
  callApi() {
    let items = "";
    axios
      .get("https://localhost:44374/activechat")
      .then((res) => res.data)
      .then((data) => {
        console.log(data);
        for (let object of data) {
          items += `
                    <li onclick="createChat(${object.id},${object.adId},${object.sold},${object.customerId},'${object.type}')">
                        
                            <span class="chatImage" style='background-image:url(${object.profileImg})' alt=""></span>
                            <div class="chatDescription">
                                <span class="chatUsername"><b>${object.username}</b> regarding <a href="https://localhost:44366/home/ad/index.html?id=${object.adId}">#${object.adId}</a> Ad</span>
                                <span class="latestMessage">Latest message: ${object.latestMessage}</span>
                                <span class="chatType">Role: ${object.type} </span>
                            </div>
                        
                    </li>
                    `;
        }
        return items;
      })
      .then(this.render);
  }
  render = (items) => {
    this.innerHTML = `
        <div class="chatContainer">
            <div class="chatContent">
                <ul class="chatItems">
                    ${items}
                </ul>
            </div>
           
        </div>
        
        <style>@import "/styles/components/chatDropdown/chatDropdown.css"</style>
        <style>@import "/styles/components/chat/chat.css"</style>
        `;
  };
}
customElements.define("chatdropdown-component", ChatDropdown);
createChat = (id, adId, sold, customerId, type) => {
  for (let chat of document.querySelector(".chatsContainer").children) {
    console.log(chat.classList.contains(`chat${id}`));
    if (chat.classList.contains(`chat${id}`)) {
      return;
    }
  }

  document.querySelector(".chatsContainer").innerHTML += `
    <chat-component class="chat${id}" chat-id="${id}" sold="${sold}" ad-id="${adId}"  type="${type}"  customer-id="${customerId}"></chat-component>
    `;
};
