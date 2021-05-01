class ChatRequest extends HTMLElement {
    get customerId() {
        return this.getAttribute("customer-id");
    }

    connectedCallback() {

    }
    constructor() {
        super();

        let items = "";
        axios.get("https://localhost:44374/chat/chatrequest")
            .then(res => res.data)
            .then(data => {
                console.log(data);
                for (let object of data) {


                    items += `
                    <li>

                            <span class="itemImage" style='background-image:url(${object.profileImg})' alt=""></span>
                            <div class="itemDescription">
                                <span class="requestTitle">
                                    User <b>${object.username}</b> wants to chat with you for 
                                    <a href="https://localhost:44366/home/ad/index.html?id=${object.adId}">this ad</a>
                                </span>
                            </div>
                            <div class="requestActions">
                                <div onclick="acceptRequest(${object.id})">Y</div>
                                <div onclick="declineRequest(${object.id})">N</div>
                            </div>
                    </li>
                    `
                }
                return items
            }).then(this.render)




    }
    render = (items) => {
        this.innerHTML = `
        <div class="chatRequestContainer">
            <div class="chatRequestContent">
                <ul class="chatRequestItems">
                    ${items}
                </ul>
            </div>
           
        </div>
        
        <style>@import "/styles/components/chatRequest/chatRequest.css"</style>
        <style>@import "/styles/components/chat/chat.css"</style>
        `
        console.log(this.innerHTML)
    }
}
acceptRequest = (id) => {
    axios.post("https://localhost:44374/chat/chatrequest/confirm/" + id).then(console.log)
}
customElements.define("chatrequest-component", ChatRequest)

