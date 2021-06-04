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
    firstTime
    constructor() {
        super();
        this.render();
        document.querySelector(".dependencies").innerHTML = ""
        let script = document.createElement("script")
        script.src = "https://maps.googleapis.com/maps/api/js?key=AIzaSyBYNtt3TEftA6RmWg7PlntfcT7OZ6KJN84&callback=initMap&libraries=&v=weekly"
        document.querySelector(".dependencies").appendChild(script);
        //axios.get(presets)

    }
    attributeChangedCallback(name, oldValue, newValue) {
        this.render();
        attachPresets();
    }
    render = () => {
        let listItems;
        switch (this.logged) {
            case "true": {
                listItems = `
               
              
                <li class="nav-item">
                    <a class="nav-link authorized" href="/Home/myFields.html">My Fields</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link authorized" href="/home/myPresets.html">My Presets</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link authorized notification" onclick="attachNotifications(event)" href="#">Notifications</a>
                </li>
                <li class="nav-item dropdown">
                    <a class="nav-link dropdown-toggle authorized" href="#" id="navbarDropdownMenuLink" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                    More Actions
                    </a>
                    <div class="dropdown-menu" aria-labelledby="navbarDropdownMenuLink">
                    <a class="dropdown-item" href="#" data-toggle="modal" data-target="#addModal" >Add Field</a>
                    <a class="dropdown-item" href="#">Account Settings</a>
                    <a class="dropdown-item" href="/home/logAnalytics.html">Sensor Logs</a>
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
                    <a class="nav-link signUpBtn unauthorized" href="https://localhost:44305/Auth/Register?returnUrl=%2Fconnect%2Fauthorize%2Fcallback%3Fclient_id%3Dclient_id_js_Dypa%26redirect_uri%3Dhttps%253A%252F%252Flocalhost%253A44376%252Fhome%252Fsignin.html%26response_type%3Dcode%26scope%3Dopenid%2520ApiDypa%2520credentials%26state%3Dfdf0368865ce49e1b0b3e65bbf0784a8%26code_challenge%3DUL9cvUaAEcM6wiAZ23JnVnVVhrbfRtnMezSu0jsm5P4%26code_challenge_method%3DS256%26response_mode%3Dquery">Sign Up</a>
                </li>
                  `;
                break;

            }
        }

        this.innerHTML = `
        
        
        <nav class="navbar navbar-expand-lg ">
            <a class="navbar-brand" href="/Home/index.html" style="">WaterGrape<br><p style="font-size:small;text-align:center;">Smart Irrigation Management</p></a>
            <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarNavDropdown" aria-controls="navbarNavDropdown" aria-expanded="false" aria-label="Toggle navigation">
                <span class="navbar-toggler-icon"></span>
            </button>
            <div class="collapse navbar-collapse" id="navbarNavDropdown">
            <ul class="navbar-nav">
                ${listItems}
            </ul>
            </div>
        </nav>
        <div class="modal fade"  id="addModal" tabindex="-1" role="dialog" aria-labelledby="addModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="addModalLabel">Add Field</h5>
                    <button type="button" onclick="clearAdd()" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="group">
                        <label for="#fieldDescriptionAdd">Title</label>

                        <input id="fieldDescriptionAdd" type="text">
                    </div>

                    <div class="group">
                        <label for="#fieldAcresAdd">Acres</label>

                        <input id="fieldAcresAdd" type="text">
                    </div>
                    <div class="group">
                        <label for="#fieldPlantRootsAdd">Number of plant roots</label>

                        <input id="fieldPlantRootsAdd" type="text">
                    </div>
                    <div class="group">
                        <label for="#fieldWaterSupplyAdd">Water supply in L/m</label>

                        <input id="fieldWaterSupplyAdd" type="text">
                    </div>
                    <div class="group">
                        <label for="#fieldPresetAdd">Choose a crop type preset</label>
                        <select id="fieldPresetAdd" ><option value="owners">other</option></select>
                    </div>
                    <div class="group">
                        <label for="#fieldVarietyAdd">Specify crop variety</label>
                        <select id="fieldVarietyAdd" ></select>
                    </div>
                    <div class="group">
                        <label for="#fieldLocationAdd">General Location</label>
                        <input id="fieldLocationAdd" type="text">
                    </div>
                    
                    <div id="map"></div>
                    
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                    <button type="button" class="btn btn-primary" onclick="postXwrafi()">Save</button>
                </div>
            </div>
        </div>
    </div>
        <div class="dependencies"></div>
        <link rel="stylesheet" href="/Home/styles/navbar/navbar.css">

        `

    };
}
const displayAddFieldModal = () => {

}
const fillEdit = () => {
    document.querySelector("#fieldDescriptionAdd").value = document.querySelector(".fieldAreaInfo").innerHTML;
    document.querySelector("#fieldAcresAdd").value = document.querySelector(".fieldAcresInfo").innerHTML;
    document.querySelector("#fieldPlantRootsAdd").value = document.querySelector(".fieldRootsInfo").innerHTML;
    document.querySelector("#fieldWaterSupplyAdd").value = document.querySelector(".fieldWaterSupplyInfo").innerHTML;
    document.querySelector("#fieldLocationAdd").value = document.querySelector(".fieldDescriptionAdd").innerHTML;
    document.querySelector("#fieldPresetAdd").selectedIndex
    document.querySelector("#fieldVarietyAdd").selectedIndex
    clearAdd();
}
const clearAdd = () => {
    const title = document.querySelector("#fieldDescriptionAdd").value = "";
    const acres = document.querySelector("#fieldAcresAdd").value = "";
    const roots = document.querySelector("#fieldPlantRootsAdd").value = "";
    const water = document.querySelector("#fieldWaterSupplyAdd").value = "";
    const location = document.querySelector("#fieldLocationAdd").value = "";

}
const postXwrafi = () => {
    const title = document.querySelector("#fieldDescriptionAdd");
    const acres = document.querySelector("#fieldAcresAdd");
    const roots = document.querySelector("#fieldPlantRootsAdd");
    const water = document.querySelector("#fieldWaterSupplyAdd");
    const preset = document.querySelector("#fieldPresetAdd").options[document.querySelector("#fieldPresetAdd").selectedIndex];
    const location = document.querySelector("#fieldLocationAdd");
    const variety = document.querySelector("#fieldVarietyAdd").options[document.querySelector("#fieldVarietyAdd").selectedIndex];
    if (locationSelected) {
        locationSelected = false;
        let data = {
            Title: title.value,
            Acres: acres.value,
            PlantRoots: roots.value,
            WaterSupply: water.value,
            PresetId: preset.value == "owners" ? variety.value : preset.value,
            LocationTitle: location.value,
            Latitude: latLong.lat,
            Longitude: latLong.lng
        }
        console.log(data)
        axios.post("https://localhost:44357/xorafi", data).then(console.log).catch(console.error)
    } else {
        alert("You must choose a location")
    }
}
const attachPresets = () => {
    const presetSelect = document.querySelector("#fieldPresetAdd")

    axios.get("https://localhost:44357/category")
        .then(res => res.data)
        .then(data => {

            let options = ""
            for (const object of data) {
                options += `<option value="${object.id}">${object.title}</option>`
            }
            presetSelect.innerHTML += options;
        })
    presetSelect.addEventListener("change", () => {
        const subcat = document.querySelector("#fieldVarietyAdd")
        let selectedCategory = presetSelect.options[presetSelect.selectedIndex]
        if (selectedCategory.value != "owners") {
            s = "https://localhost:44357/subcategory/" + selectedCategory.value
        } else {
            s = "https://localhost:44357/owner/category/"
        }
        axios.get(s).then(res => res.data)
            .then(data => {
                console.log(data)
                let options = ""
                for (const object of data) {
                    options += `<option value="${object.id}">${object.title}</option>`
                }
                subcat.innerHTML = options;
            })
    })
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
    notificationComponent.style.top = y;
    notificationComponent.style.left = x;
}

customElements.define("navbar-component", NavbarComponent);


let notifCounter = 0;
class NotificationComponent extends HTMLElement {
    counter = 0;
    constructor() {
        super();

        var connection = new signalR.HubConnectionBuilder()
            .withUrl("https://localhost:44357/NotificationHub")
            .build();
        connection
            .start()
            .then(function () { })
            .catch(function (err) {
                return console.error(err.toString());
            });
        connection.on("HourlySensor", (subId) => {
            this.callApi();


         
        });
        this.callApi();
    }
    callApi = () => {
        axios
            .get("https://localhost:44357/owner/notification/")
            .then((response) => response.data)
            .then(handleApiDataNotifications)
            .then(this.render)
            .catch((x) => console.log(x))
            .finally();
    };
    connectedCallback() { }
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

//style="background-image:url('${object.productphoto}')
function handleApiDataNotifications(data) {
    console.log(data);
    let allItems = "";
    for (object of data) {
        console.log(object);
        const item = `
        <li onclick="" class="" >
            <a href="https://localhost:44376/home/fieldOverview.html?id=${object.id}">
                <div class="itemDescription">
                    <span class="title">${object.title}</span>
                    <span class="price">At ${object.locationTitle}</span>
                    <span class="info">Soil humidity reaches low levels in this field, currently: ${object.currentSoilHum}%, while it should be above ${object.lowestSoilHum}%</span>
                    <span class="date">Before  ${determineNotation(Math.round((Date.now() - object.timestamp) / 1000))}</span>
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
let latLong
let map;
let locationSelected
function initMap() {
    if(initMap2){
        initMap2();
    }
    locationSelected = false
    const myLatlng = { lat: 39.0742, lng: 21.8243 };
    map = new google.maps.Map(document.getElementById("map"), {
        zoom: 8,
        center: myLatlng,

    });
    // Create the initial InfoWindow.
    let infoWindow = new google.maps.InfoWindow({
        content: "Mark your field area, for weather reports",
        position: myLatlng,
    });
    infoWindow.open(map);
    map.setMapTypeId(google.maps.MapTypeId.SATELLITE)
    // Configure the click listener.
    map.addListener("click", (mapsMouseEvent) => {
        // Close the current InfoWindow.
        locationSelected = true;
        console.log(locationSelected)
        latLong = JSON.parse(JSON.stringify(mapsMouseEvent.latLng))
        console.log(JSON.parse(JSON.stringify(mapsMouseEvent.latLng)))
        infoWindow.close();
        // Create a new InfoWindow.
        infoWindow = new google.maps.InfoWindow({
            position: mapsMouseEvent.latLng,
        });
        infoWindow.setContent(
            "Here!"
        );
        infoWindow.open(map);
    });
}

