// const template = document.createElement('template');
// template.innerHTML = html

class NotificationComponent extends HTMLElement {
    constructor() {
        super();
        var connection = new signalR.HubConnectionBuilder().withUrl("https://localhost:44374/NotificationHub").build();
        connection.on("ReceiveWishListNotification", () => {
            console.log("ajajajaja")
        })        
        axios.get("https://localhost:44374/wishlist/notification")
            .then((response) => response.data)
            .then(handleApiDataNotifications)
            .then(this.render)
            .catch(x => console.log(x))
            .finally()

    }
    connectedCallback() {

    }
    render = (html) => {

        this.innerHTML = html
    }
}
function listen() {

}
//style="background-image:url('${object.productphoto}')
function handleApiDataNotifications(data) {
    let allItems = "";
    for (object of data) {
        const item = `
        <li>
            <a href="https://localhost:44366/home/ad/index.html?id=${object.adId}">
                <span class="itemImage qwe" style='background-image:url(${object.img})' alt=""></span>
                <div class="itemDescription">
                    <span class="title">${object.title}</span>
                    <span class="info">${object.username}</span>
                    <span class="price">${object.price}$</span>
                    <span class="date">Before ${Math.round((Date.now() - (new Date(object.lastUpdate)).getTime()) / 1000 / 60)} minutes </span>
                </div>
            </a>
        </li>
        `
        allItems += item;
    }

    const html = `
    <div class="notificationContainer">
        <div class="notificationContent">
            <ul class="notificationItems">
                ${allItems}
            </ul>
        </div>
        <br>
        <div class="notificationMore">
            <a href="#">Expand this list</a>
        </div>
    </div>
    <style>@import "/styles/components/notification/notification.css"</style>
    `
    console.log(html);
    return html;
}
customElements.define("notification-component", NotificationComponent)