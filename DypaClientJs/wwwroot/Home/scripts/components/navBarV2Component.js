class NavbarComponent extends HTMLElement {
    static get observedAttributes() {
        return ["logged"];
    }
    get logged() {
        return this.getAttribute("logged");
    }

    constructor() {
        super();
    }
    attributeChangedCallback(name, oldValue, newValue) {
        this.render();

    }
    render = () => {
        let listItems;
        switch (this.logged) {
            case "true": {
                listItems = `
                <li class="nav-items">
                    <a href="#" class="nav-links">
                    <i class="fas fa-volleyball-ball"></i>
                    <span class="links-text">Cats</span>
                    </a>
                </li>
            
                <li class="nav-items " >
                    <a href="#" class="nav-links notification"  onclick="attachNotifications(event)">
                    <i class="fas fa-volleyball-ball"></i>
                    <span class="links-text">Aliens</span>
                    </a>
                </li>
            
                <li class="nav-items">
                    <a href="#" class="nav-links">
                    <i class="fas fa-volleyball-ball"></i>
                    <span class="links-text">Space</span>
                    </a>
                </li>
            
                <li class="nav-items">
                    <a href="#" class="nav-links">
                    <i class="fas fa-volleyball-ball"></i>
                    <span class="links-text">Shuttle</span>
                    </a>
                </li>
                <notification-component class="fresh" style="display:none;"></notification-component>

                  `;
                break;

            }
            default: {
                listItems = `
                <li><a href="#">Log in
                    <i class="fas fa-bell"></i>
                    </a>
                </li>
                    <li><a href="#">Sign up
                    <i class="fas fa-user"></i>
                    </a>
                </li>
                  `;
                break;

            }
        }

        this.innerHTML = `
  
        <nav class="navbar dark">
        <ul class="navbar-nav">
          <li class="logos">
            <a href="#" class="nav-links">
              <span class="links-text logos-text">WaterGrape</span>
              <i class="fas fa-volleyball-ball "></i>
            </a>
          </li>
    
          ${listItems}
    
          <li class="nav-items" id="themeButton">
            <a href="#" class="nav-links">
              <i class="fas fa-volleyball-ball"></i>
              <span class="links-text">Log out</span>
              
            </a>
          </li>
        </ul>
      </nav>


        `

    };
}
$(document).ready(function () {
    $(".hamburger").click(function () {
        $(".wrapperer").toggleClass("collapseSidebar");
    });
});
const attachNotifications = (event) => {
    if (
        document.querySelector("notification-component").classList.contains("fresh")
    ) {


        document.querySelector("notification-component").classList.remove("fresh");
        let notification = document.querySelector("notification-component");
        document.querySelector("notification-component").style.display = "block";
        document.body.addEventListener("click", (event) => {
            if (
                !(
                    (event.pageY > notificationComponent.offsetTop &&
                        event.pageY <
                        notificationComponent.offsetTop +
                        notificationComponent.offsetHeight &&
                        event.pageX <
                        notificationComponent.offsetLeft +
                        notificationComponent.offsetWidth &&
                        event.pageX > notificationComponent.offsetLeft) ||
                    event.target.classList.contains("notification")
                )
            ) {
                notification.style.display = "none";
            }
        });
    } else {

        document.querySelector("notification-component").style.display = "block";


    }
    let x = document.querySelector(".notification").getBoundingClientRect().left;
    let y = document
        .querySelector(".notification")
        .getBoundingClientRect().bottom;;
    const notificationComponent = document.querySelector(
        "notification-component"
    );
    notificationComponent.style.top = y;
    notificationComponent.style.left = x;
}

customElements.define("navbar-component", NavbarComponent);


let notifCounter = 0;
class NotificationComponent extends HTMLElement {
    counter = 0;
    constructor() {
        super();

        // var connection = new signalR.HubConnectionBuilder()
        //     .withUrl("https://localhost:44374/NotificationHub")
        //     .build();
        // connection
        //     .start()
        //     .then(function () { })
        //     .catch(function (err) {
        //         return console.error(err.toString());
        //     });
        // connection.on("ReceiveWishListNotification", (subId) => {
        //     if (me.profile.sub != subId) {
        //         document.querySelector(".notification").style.backgroundColor =
        //             "#1860AA";
        //         document.querySelector(".notification").style.border =
        //             "1px solid white";
        //         notifCounter = 0;
        //         document.querySelector(".notificationItems").innerHTML = "";
        //         this.callApi();


        //     }
        // });
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
    connectedCallback() { }
    render = (html) => {

        this.innerHTML = html;
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

    console.log(data);
    let prevData = document.querySelector(".notificationItems")
        ? document.querySelector(".notificationItems").innerHTML
        : "";
    let allItems = "";
    data = data.reverse();
    for (object of data) {
        console.log(object);
        const item = `
        <li onclick="setSeen('${object.type}',${object.adId},${object.id},${object.sold
            })" class="${object.clicked ? "" : "new"}" >
            <a href="${object.type != "Review" ? "#" : "#"}">
                <span class="itemImage qwe" style='background-image:url(${object.img
            })' alt=""></span>
                <div class="itemDescription">
                    <span class="title">${object.type == "SubCategory"
                ? `New ad on ${object.title}`
                : object.type == "Review"
                    ? `You can now drop a review on seller ${object.username}`
                    : `Ad update: ${object.title}`
            }</span>
                    
                    ${object.type == "Review"
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
                ${allItems + prevData}
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

