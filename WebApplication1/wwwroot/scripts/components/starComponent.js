class StarComponent extends HTMLElement {
  constructor() {
    super();
    
    
  }
  get starValue() {
    return this.getAttribute("star-value");
  }


  renders=()=>{
    console.log(value);

    const stars = this.starValue / 2;
    let starList = "";
    for (let i = 1; i <= stars; i++) {
      starList += "<li></li>";
    }
    if (this.starValue % 2 == 0) {
    } else if (this.starValue != 1) {
      starList += "<li style='width:10%;'></li>";
    }
    this.innerHTML = `
    <div class="outterLayer">
    <div class="innerLayer">
      <ul>
        ${starList}     
      </ul>
    </div>
  </div>
    `;
  }
}
customElements.define("stars-component", StarComponent);
