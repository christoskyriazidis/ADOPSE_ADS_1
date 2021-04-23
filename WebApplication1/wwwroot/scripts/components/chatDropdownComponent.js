
class ChatDropdown extends HTMLElement {
    get customerId() {
        return this.getAttribute("customer-id");
    }

    connectedCallback() {

    }
    constructor() {
        super();

        let items="";
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
                    <li>
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