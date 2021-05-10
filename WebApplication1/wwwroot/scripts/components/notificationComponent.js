// const template = document.createElement('template');
// template.innerHTML = html

class NotificationComponent extends HTMLElement {
  constructor() {
    super();
    var connection = new signalR.HubConnectionBuilder()
      .withUrl("https://localhost:44374/NotificationHub")
      .build();
    connection
      .start()
      .then(function () {})
      .catch(function (err) {
        return console.error(err.toString());
      });
    connection.on("ReceiveWishListNotification", (subId) => {
      if (me.profile.sub != subId) {
        document.querySelector(".notification").style.backgroundColor =
          "#1860AA";
        document.querySelector(".notification").style.border =
          "1px solid white";
        this.callApi();
      }
    });
    this.callApi();
  }
  callApi = () => {
    axios
      .get("https://localhost:44374/notification/1")
      .then((response) => response.data)
      .then(handleApiDataNotifications)
      .then(this.render)
      .catch((x) => console.log(x))
      .finally();
  };
  connectedCallback() {}
  render = (html) => {
    this.innerHTML = html;
  };
}
function listen() {
  axios
    .get("https://localhost:44374/notification/1")
    .then((response) => response.data)
    .then(handleApiDataNotifications)
    .then(this.render)
    .catch((x) => console.log(x))
    .finally();
}
setSeen = (notificationId, adId) => {
  axios
    .put("https://localhost:44374/notification/seen/" + notificationId)
    .then((response) => response.data)
    .then(() => {
      window.location.href =
        "https://localhost:44366/home/ad/index.html?id=" + adId;
    })
    .then(this.render)
    .catch((x) => console.log(x))
    .finally();
};
//style="background-image:url('${object.productphoto}')
function handleApiDataNotifications(data) {
  let allItems = "";
  for (object of data) {
    console.log(object);
    const item = `
        <li onclick="setSeen(${object.nId},${object.adId})" class="${
      object.clicked ? "" : "new"
    }" >
            <a href="${
              object.type != "Review"
                ? "https://localhost:44366/home/ad/index.html?id=" + object.adId
                : "https://localhost:44366/home/profile/index.html?id=" +
                  object.customerId
            }">
                <span class="itemImage qwe" style='background-image:url(${
                  object.img
                })' alt=""></span>
                <div class="itemDescription">
                    <span class="title">${
                      object.type == "SubCategory"
                        ? `New ad on ${object.title}`
                        : object.type == "Review"
                        ? `You can now drop a review on seller ${object.username}`
                        : `Ad update: ${object.title}`
                    }</span>
                    
                    ${
                      object.type == "Review"
                        ? ""
                        : `<span class="price">${object.price}$</span>
                        <span class="info">${object.username}</span>`
                    }
                    <span class="date">Before  ${Math.round(
                      (Date.now() - object.timestamp) / 1000 / 60
                    )} minutes </span>
                </div>
            </a>
        </li>
        `;
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
    `;

  return html;
}
customElements.define("notification-component", NotificationComponent);
