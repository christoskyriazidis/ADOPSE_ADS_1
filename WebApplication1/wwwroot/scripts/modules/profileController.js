import Dictionary from "/scripts/modules/dictionary.js"
export default class ProfileController {
    constructor() {
        let dict = new Dictionary();
        dict.init().then(maps => {
            maps.cat.forEach(this.fillCategories)

            maps.man.forEach(this.fillManufacturer)
            maps.typ.forEach(this.fillType)
            maps.con.forEach(this.fillCondition)
            this.dictMaps = maps
        }).then(() => {
            
        })

    }
    fillCategories = (value, id) => {
        const categories = document.querySelector("#categoryGroup")
        categories.innerHTML += `<option value="${id}" >${value}</option>`
    }
    fillType = (value, id) => {
        const types = document.querySelector("#typeGroup")
        types.innerHTML += `<option value="${id}" >${value}</option>`
    }

    fillManufacturer = (value, id) => {
        const manufacturers = document.querySelector("#manufacturerGroup")
        manufacturers.innerHTML += `<option value="${id}" >${value}</option>`
                                
    }
    fillCondition = (value, id) => {

        const conditions = document.querySelector("#conditionGroup")
        conditions.innerHTML += `<option value="${id}" >${value}</option>`
                            
    }
   
    postAd = () => {
        var formData = new FormData();
        formData.append("Img", document.querySelector(".image").files[0]);
        formData.append("Title", document.querySelector(".title").value);
        formData.append("Description",  document.querySelector(".description").value);
        formData.append("Type", document.querySelector("#typeGroup").options[document.querySelector("#typeGroup").selectedIndex].value);
        formData.append("Category", document.querySelector("#typeGroup").options[document.querySelector("#typeGroup").selectedIndex].value);
        formData.append("Condition", document.querySelector("#typeGroup").options[document.querySelector("#typeGroup").selectedIndex].value);
        formData.append("Manufacturer", document.querySelector("#typeGroup").options[document.querySelector("#typeGroup").selectedIndex].value);
        formData.append("Price", document.querySelector(".price").value);
        console.log(formData.forEach(console.log));
        axios.post("https://localhost:44374/ad", formData, {
            headers: {
                'Content-Type': 'multipart/form-data'
            }
            
        }).then(response => {
            console.log(response)
        }).catch(error => {
            console.log(error.response.data)
            });
    }
}

class Ad {
    title;
    description;
    type;
    category;
    condition;
    manufacturer;
    price;
    file;

}


