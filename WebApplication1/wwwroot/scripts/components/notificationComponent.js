// const template = document.createElement('template');
// template.innerHTML = html
let notifCounter = 0;
class NotificationComponent extends HTMLElement {
  counter = 0;
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
    notifCounter++;
    axios
      .get("https://localhost:44374/notification/" + notifCounter)
      .then((response) => response.data)
      .then(handleApiDataNotifications)
      .then(this.render)
      .catch((x) => console.log(x))
      .finally();
  };
  connectedCallback() {}
  render = (html) => {
    this.innerHTML = html;
    console.log("....", document.querySelector(".notificationItems"));
    document
      .querySelector(".notificationItems")
      .addEventListener("scroll", () => {
        if (document.querySelector(".notificationItems").scrollTop == 0) {
          notifCounter++;
          axios
            .get("https://localhost:44374/notification/" + notifCounter)
            .then((response) => response.data)
            .then(handleApiDataNotifications)
            .then(this.render)
            .catch((x) => console.log(x))
            .finally();
        }
      });
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
setSeen = (type, adId, id, sold) => {
  const data = {
    Type: type,
    Id: id,
  };
  if (sold) {
    alert("Already droped a review");
    return;
  }
  if (type != "Review") {
    axios
      .put("https://localhost:44374/notification/", data)
      .then((response) => response.data)
      .then(() => {
        if (type != "Review") {
          window.location.href =
            "https://localhost:44366/home/ad/index.html?id=" + adId;
        } else {
        }
      })
      .then(this.render)
      .catch((x) => console.log(x))
      .finally();
  } else {
    window.location.href =
      "https://localhost:44366/home/profile/index.html?reviewMode=1&id=" +
      object.customerId +
      "&adId=" +
      object.adId;
  }
};
//style="background-image:url('${object.productphoto}')
function handleApiDataNotifications(data) {
  let prevData=document.querySelector(".notificationItems")?document.querySelector(".notificationItems").innerHTML:""
  let allItems= "";
  data=data.reverse();
  for (object of data) {
    console.log(object);
    const item = `
        <li onclick="setSeen('${object.type}',${object.adId},${object.id},${
      object.sold
    })" class="${object.clicked ? "" : "new"}" >
            <a href="${object.type != "Review" ? "#" : "#"}">
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
                    <span class="date">Before  ${determineNotation(
                      Math.round((Date.now() - object.timestamp) / 1000)
                    )}</span>
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
                ${allItems+prevData}
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

determineNotation = (seconds) => {
  let final;
  if (seconds < 60) {
    return Math.round(seconds) + " seconds";
  } else if (seconds / 60 < 60) {
    return Math.round(seconds / 60) + " minutes";
  } else if (seconds / 60 / 60 < 24) {
    return Math.round(seconds / 60 / 60) + " hours";
  } else if (seconds / 60 / 60 / 24 < 30) {
    return Math.round(seconds / 60 / 60 / 24) + " days";
  }
};

customElements.define("notification-component", NotificationComponent);
