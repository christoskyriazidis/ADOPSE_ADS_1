
class ChatDropdown extends HTMLElement {
    get customerId() {
        return this.getAttribute("customer-id");
    }

    connectedCallback() {

    }
    constructor() {
        super();

        let items = "";
        axios.get("https://localhost:44374/profile/chat")
            .then(res => res.data)
            .then(data => {
                console.log(data);
                for (let object of data) {
                    let receiverId, receiverUsername;
                    if (object.sid == this.customerId) {
                        receiverId = object.bid
                        receiverUsername = object.buyer
                    } else {
                        receiverId = object.sid
                        receiverUsername = object.seller
                    }
                    console.log(receiverUsername);
                    items += `
                    <li onclick="createChat(${object.id})">
                        <a href="#">
                            <span class="itemImage" style='background-image:url()' alt=""></span>
                            <div class="itemDescription">
                                <span class="title">${receiverUsername}</span>
                            </div>
                        </a>
                    </li>
                    `
                }
                return items
            }).then(this.render)




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
        `
    }

}
customElements.define("chatdropdown-component", ChatDropdown)
createChat = (id) => {
  
    for(let chat of document.querySelector(".chatsContainer").children){
        console.log(chat.classList.contains(`chat${id}`));
        if(chat.classList.contains(`chat${id}`)){
            return
        }
    }
   
    document.querySelector(".chatsContainer").innerHTML += `
    <chat-component class="chat${id}" chat-id="${id}" customer-id="1"></chat-component>
    `
}