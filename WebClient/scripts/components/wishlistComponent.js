// const template = document.createElement('template');
// template.innerHTML = html

class WishlistComponent extends HTMLElement {
    constructor() {
        super();
        console.log('hi')
        Ajax.request("GET", "https://6037e3024e3a9b0017e927dd.mockapi.io/yy")
            .then(handleApiData)
            .then((html) => this.innerHTML = html)
            .catch(x => console.log(x))
            .finally()
        
    }
    connectedCallback() {

    }
    static render(html) {
        this.innerHTML = html
    }
}

//style="background-image:url('${object.productphoto}')
function handleApiData(data) {
    let allItems="";
    for (object of data) {
        const item = `
        <li>
            <a href="#">
                <span class="itemImage" style='background-image:url(${object.productphoto})' alt=""></span>
                <div class="itemDescription">
                    <span class="title">${object.title}</span>
                    <span class="info">${object.description}</span>
                    <span class="price">${object.price}</span>
                </div>
            </a>
        </li>
        `
        allItems += item;
    }
    const html = `
    <div class="wishlistContainer">
        <div class="wishlistContent">
            <ul class="wishlistItems">
                ${allItems}
            </ul>
        </div>
        <br>
        <div class="wishlistMore">
            <a href="#">Expand this list</a>
        </div>
    </div>
    <style>@import "styles/components/wishlist/wishlist.css"</style>
    `
    return html;
}
customElements.define("wishlist-component", WishlistComponent)