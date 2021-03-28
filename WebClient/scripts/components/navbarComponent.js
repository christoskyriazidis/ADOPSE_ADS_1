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
                    <li><button style="" onclick="signOut()" >signOut</button></li>
                    <li><a class="hide" href="#">My account</a></li>
                    <li><a href="#" onclick="attachWishlist(event)" class="wishlist">wishlist</a></li>
                    <li><a href="#" class="notifications hide">Notifications</a></li>
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
                <h1>awesome brand logo</h1>
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
                    <li><a href="./search/index.html">Ads</a></li>
                    <div class="vl"> </div>
                    <li><a href="">Categories</a></li>
                    <div class="vl"> </div>
                    <li><a href="">About</a></li>
                    <div class="vl"> </div>
                </ul>
            </div>
        </div>
    </nav>

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

customElements.define("navbar-component", NavbarComponent)
