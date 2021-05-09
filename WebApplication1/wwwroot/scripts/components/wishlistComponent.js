// const template = document.createElement('template');
// template.innerHTML = html

class WishlistComponent extends HTMLElement {
    constructor() {
        super();
      
        
        axios.get("https://localhost:44374/wishlist")
            .then((response)=>response.data)
            .then(handleApiData)
            .then(this.render)
            .catch(x => console.log(x))
            .finally()

    }
    connectedCallback() {

    }
    render=(html)=>{
       
        this.innerHTML = html
       
    }
}

//style="background-image:url('${object.productphoto}')
function handleApiData(data) {
    
    let allItems = "";
    for (object of data) {
        const item = `
        <li>
            <a href="https://localhost:44366/home/ad/index.html?id=${object.adId}">
                <span class="itemImage" style='background-image:url(${object.img})' alt=""></span>
                <div class="itemDescription">
                    <span class="title">${object.title}</span>
                    <span class="info">${object.username}</span>
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
            <a href="/home/profile/wishlist/index.html">Expand this list</a>
        </div>
    </div>
    <style>@import "/styles/components/wishlist/wishlist.css"</style>
    `
    return html;
}
customElements.define("wishlist-component", WishlistComponent)