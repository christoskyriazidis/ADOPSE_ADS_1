class NavbarComponent extends HTMLElement {
    static get observedAttributes() {
        return ["logged"];
    }
    get logged() {
        return this.getAttribute("logged");
    }
    get filters() {
        return this.getAttribute("filters");
    }
    constructor() {
        super();
        this.render();
    }
    attributeChangedCallback(name, oldValue, newValue) {
        this.render();

    }
    render = () => {
        let listItems;
        switch (this.logged) {
            case "true": {
                listItems = `
               
              
                <li class="nav-item">
                    <a class="nav-link authorized" href="#">My Fields</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link authorized" href="#">My Workers</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link authorized notification" onclick="attachNotifications(event)" href="#">Notifications</a>
                </li>
                <li class="nav-item dropdown">
                    <a class="nav-link dropdown-toggle authorized" href="#" id="navbarDropdownMenuLink" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                    More Actions
                    </a>
                    <div class="dropdown-menu" aria-labelledby="navbarDropdownMenuLink">
                    <a class="dropdown-item" href="#">Account Settings</a>
                    <a class="dropdown-item" href="#">Sensor Logs</a>
                    <a class="dropdown-item" href="#">Weather History</a>

                    </div>
                </li>
                <li class="nav-item">
                    <a class="nav-link authorized" href="#" onclick="signOut()">Log out</a>
                </li>
                <notification-component class="fresh" style="display:none;"></notification-component>

                  `;
                break;

            }
            default: {
                listItems = `
                <li class="nav-item">
                    <a class="nav-link unauthorized" href="#" onclick="signIn()">Log In</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link signUpBtn unauthorized" href="https://localhost:44305/Auth/Register">Sign Up</a>
                </li>
                  `;
                break;

            }
        }

        this.innerHTML = `
        
        
        <nav class="navbar navbar-expand-lg ">
            <a class="navbar-brand" href="#" style="">WaterGrape<br><p style="font-size:small;text-align:center;">Smart Irrigation Management</p></a>
            <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarNavDropdown" aria-controls="navbarNavDropdown" aria-expanded="false" aria-label="Toggle navigation">
                <span class="navbar-toggler-icon"></span>
            </button>
            <div class="collapse navbar-collapse" id="navbarNavDropdown">
            <ul class="navbar-nav">
                ${listItems}
            </ul>
            </div>
        </nav>
        <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css" integrity="sha384-ggOyR0iXCbMQv3Xipma34MD+dH/1fQ784/j6cY/iJTQUOhcWr7x9JvoRxT2MZw1T" crossorigin="anonymous">

        <link rel="stylesheet" href="/Home/styles/navbar/navbar.css">

        `
    };
}
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
    notificationComponent.style.top=y;
    notificationComponent.style.left=x;
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

