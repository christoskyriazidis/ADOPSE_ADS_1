class NavbarComponent extends HTMLElement {
    static get observedAttributes() { return ['logged'] }
    get logged() {
        return this.getAttribute("logged");
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
                    <li><a  href="#" class="my-account" onclick="attachMyAccountDropdown(event)">My Account</a></li>
                    <li><a href="#" onclick="attachWishlist(event)" class="wishlist">Wishlist</a></li>
                    <li><a href="#" onclick="attachNotifications(event)" class="notification">Notifications</a></li>
                    <li><a href="#" class="hide">Chats</a></li>
                `
                break;
                //const btnSignOut = document.querySelector('#btn-signOut')
                //btnSignOut.addEventListener('click', signOut)
            } default: {
                listItems = `
                    <li><a href="#" onclick="signIn()" >Log In</a></li>
                    <li><a class="attention" href="https://localhost:44305/Auth/Register">Sign up</a></li>
                `
                break;
                // const btnSignIn = document.querySelector('#btn-signIn')
                // btnSignIn.addEventListener('click', signIn)
            }


        }

        this.innerHTML = `
        <style>@import "/styles/components/navbar/navbar.css";</style>
       
        <nav>
        <div class="primary-nav">
            <div class="brandLogo">
            <a href="/home/index.html"></a>
            </div>
            <div class="navListContainer">
                <ul class="loggedIn hidden">
                    ${listItems}
                </ul>
            </div>
        </div>
        
        <div class="secondary-nav">
            <div class="secondNavListContainer">
                <ul>
                    <div class="vl"> </div>
                    <li><a href="/home/search/index.html">Ads</a></li>
                    <div class="vl"> </div>
                    <li><a href="/home/categories/index.html">Categories</a></li>
                    <div class="vl"> </div>
                    <li><a href="/home/sellers/index.html">Sellers</a></li>
                    <div class="vl"> </div>
                    <li><a href="/home/about/index.html">About</a></li>
                    <div class="vl"> </div>
                </ul>
            </div>
        </div>
    </nav>
    <div class="my-account-dropdown fresh">
        <ul>
            <li><a href="/home/profile/index.html?id=me">My profile</a></li>
            <li><a href="/home/profile/myAds/index.html">My ads</a></li>
            <li><a href="/home/profile/addAd/index.html">Create an ad</a></li>
            <li><a href="#">Account settings</a></li>
            <li><a href="#" onclick="signOut()">Logout</a></li>
        </ul>
        </div>
        `
    }

}
const attachWishlist = (event) => {
    console.log("haha");

    if (!document.querySelector("wishlist-component")) {
        let wishlist = document.createElement("wishlist-component")
        document.querySelector("navbar-component").appendChild(wishlist);

        document.body.addEventListener("click", (event) => {
            if (!((
                event.pageY > wishlistComponent.offsetTop &&
                event.pageY < wishlistComponent.offsetTop + wishlistComponent.offsetHeight &&
                event.pageX < wishlistComponent.offsetLeft + wishlistComponent.offsetWidth &&
                event.pageX > wishlistComponent.offsetLeft)
                || event.target.classList.contains("wishlist"))) {
                wishlist.style.display = 'none'
            }
        })

    } else {
        document.querySelector("wishlist-component").style.display = "block";
    }
    let x = document.querySelector(".wishlist").getBoundingClientRect().left;
    let y = document.querySelector(".wishlist").getBoundingClientRect().bottom;
    const wishlistComponent = document.querySelector("wishlist-component");
    wishlistComponent.style.display = "block";
    wishlistComponent.style.left = x + 'px';
    wishlistComponent.style.top = y + 'px';


}
const attachMyAccountDropdown = (event) => {


    if (document.querySelector(".my-account-dropdown").classList.contains("fresh")) {
        document.querySelector(".my-account-dropdown").classList.remove("fresh")
        document.querySelector(".my-account-dropdown")
        document.body.addEventListener("click", (event) => {
            if (!((
                event.pageY > myAccountDropdown.offsetTop &&
                event.pageY < myAccountDropdown.offsetTop + myAccountDropdown.offsetHeight &&
                event.pageX < myAccountDropdown.offsetLeft + myAccountDropdown.offsetWidth &&
                event.pageX > myAccountDropdown.offsetLeft)
                || event.target.classList.contains("my-account"))) {
                   document.querySelector(".my-account-dropdown").style.display = 'none'
            }
        })

    } else {
        document.querySelector(".my-account-dropdown").style.display = "block";
    }
    let x = document.querySelector(".my-account").getBoundingClientRect().left;
    let y = document.querySelector(".my-account").getBoundingClientRect().bottom;
    const myAccountDropdown = document.querySelector(".my-account-dropdown");
    myAccountDropdown.style.display = "block";
    myAccountDropdown.style.left = x + 'px';
    myAccountDropdown.style.top = y + 'px';


}
const attachNotifications = (event) => {


    if (!document.querySelector("notification-component")) {

        let notification = document.createElement("notification-component")
        document.querySelector("navbar-component").appendChild(notification);
        document.body.addEventListener("click", (event) => {
            if (!((
                event.pageY > notificationComponent.offsetTop &&
                event.pageY < notificationComponent.offsetTop + notificationComponent.offsetHeight &&
                event.pageX < notificationComponent.offsetLeft + notificationComponent.offsetWidth &&
                event.pageX > notificationComponent.offsetLeft)
                || event.target.classList.contains("notification"))) {
                notification.style.display = 'none'
            }
        })

    } else {

        document.querySelector("notification-component").style.display = "block";
    }
    let x = document.querySelector(".notification").getBoundingClientRect().left;
    let y = document.querySelector(".notification").getBoundingClientRect().bottom;
    const notificationComponent = document.querySelector("notification-component");
    notificationComponent.style.display = "block";
    notificationComponent.style.left = x + 'px';
    notificationComponent.style.top = y + 'px';


}
const myAccountDropdown = ``
customElements.define("navbar-component", NavbarComponent)
