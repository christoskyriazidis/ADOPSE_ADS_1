class AdContainerComponent extends HTMLElement {
  get title() {
    return this.getAttribute("title");
  }
  get photo() {
    return this.getAttribute("photo");
  }
  get price() {
    return this.getAttribute("price");
  }
  get condition() {
    return this.getAttribute("condition");
  }
  get type() {
    return this.getAttribute("type");
  }
  get itemImage() {
    return this.getAttribute("item-image");
  }
  get id() {
    return this.getAttribute("id");
  }
  get customerId() {
    return this.getAttribute("customer-id");
  }
  get customerName() {
    return this.getAttribute("customer-name");
  }
  get customerRating() {
    return this.getAttribute("customer-rating");
  }
  get customerReviews() {
    return this.getAttribute("customer-reviews");
  }
  get customerImage() {
    return this.getAttribute("customer-image");
  }
  get case() {
    return this.getAttribute("case");
  }
  constructor() {
    super();
    this.render();
  }
  //style='background-image:url(${this.customerImage})
  render = () => {
    this.innerHTML = `
            <div class="adContainer">
                <div class="itemData">
                    <a href="https://localhost:44366/home/ad/index.html?id=${
                      this.id
                    }">
                    <span class="adType" style="margin-left:auto;${this.type=="BUY"?"background-color:#51c977":this.type=="SELL"?"background-color:#e6c55b":""}">${this.type}</span>
                        <span class="image" style='background-image:url(${
                          this.itemImage
                        })'></span>
                        <div class="itemInfo">
                            <span class="title" >${this.title}</span>
                            <span class="condition">${this.condition}</span>
                            <span class="price">${this.price}$</span>
                           
                        </div>

                    </a>
                 
                </div>
                <hr>
                ${
                  this.case == "myads"
                    ? `<button class="editButton"><a href="https://localhost:44366/home/profile/editAd/index.html?id=${this.id}">edit me !</a></button>
                <button class="deleteButton" onclick="myadsController.deleteAd(${this.id})">delete me!</button>`
                    : `<div class="sellerData">
                <a href = "https://localhost:44366/home/profile/index.html?id=${
                  this.customerId
                }">
                    <span class="sellerAvatar" style="background-image:url('${
                      this.customerImage
                    }')"  name="avatar" ></span>
                        <label label for= "avatar" > ${
                          this.customerName
                        }</label >
                        <span class="filler"  ></span>
                        <span class="sellerReview" >${starMethod(
                          this.customerRating
                        )} </span>  <p> (${this.customerReviews})</p>
                </a>
                </div>`
                }
                
            </div >
            `;
  };
}

customElements.define("ad-component", AdContainerComponent);
//<span class="sellerAvatar favourite" style='background-image:url(https://cdn4.iconfinder.com/data/icons/avatars-xmas-giveaway/128/girl_female_woman_avatar-512.png)'></span>
