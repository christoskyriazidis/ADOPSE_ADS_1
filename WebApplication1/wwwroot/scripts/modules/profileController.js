import Dictionary from "/scripts/modules/dictionary.js"
export default class ProfileController {
    dict;
    eventSet = false;
    constructor() {
        this.dict = new Dictionary();
        this.dict.init().then(maps => {
            maps.cat.forEach(this.fillCategories)
            maps.man.forEach(this.fillManufacturer)
            maps.typ.forEach(this.fillType)
            maps.con.forEach(this.fillCondition)
            if (document.querySelector(".editAd")) {
                maps.sta.forEach(this.fillState)
            }
            this.dictMaps = maps
        }).then(() => {
            const cats = document.querySelector("#categoryGroup")
            cats.selectedIndex = -1;
            cats.addEventListener("change", () => {
                this.getSubCategory(cats.options[cats.selectedIndex].value)
            })
        })

    }
    getSubCategory = (id) => {
        this.dict.getSubCategoryDictionary(id)
            .then(data => {
                const subcategories = document.querySelector("#subCategoryGroup")
                subcategories.innerHTML = ""
                for (let object of data) {
                    subcategories.innerHTML += `<option value="${object.id}" >${object.title}</option>`
                }
            })

    }
    fillState = (value, id) => {
        const categories = document.querySelector("#stateGroup")
        categories.innerHTML += `<option value="${id}" >${value}</option>`
    }
    fillCategories = (value, id) => {
        const categories = document.querySelector("#categoryGroup")

        categories.innerHTML += `<option value="${id}">${value}</option>`
    }
    fillType = (value, id) => {
        const types = document.querySelector("#typeGroup")
        types.innerHTML += `<option value="${id}">${value}</option>`
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
        formData.append("Description", document.querySelector(".description").value);
        formData.append("Type", document.querySelector("#typeGroup").options[document.querySelector("#typeGroup").selectedIndex].value);
        formData.append("Category", document.querySelector("#categoryGroup").options[document.querySelector("#categoryGroup").selectedIndex].value);
        formData.append("SubCategoryId", document.querySelector("#subCategoryGroup").options[document.querySelector("#subCategoryGroup").selectedIndex].value);
        formData.append("Condition", document.querySelector("#conditionGroup").options[document.querySelector("#conditionGroup").selectedIndex].value);
        formData.append("Manufacturer", document.querySelector("#manufacturerGroup").options[document.querySelector("#manufacturerGroup").selectedIndex].value);
        formData.append("Price", document.querySelector(".price").value);
        console.log(formData);
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
    editAd = () => {
        const ad = new Ad(
            urlParams.get("id"),
            document.querySelector(".title").value,
            document.querySelector(".description").value,
            document.querySelector("#typeGroup").options[document.querySelector("#typeGroup").selectedIndex].value,
            document.querySelector("#categoryGroup").options[document.querySelector("#categoryGroup").selectedIndex].value,
            document.querySelector("#subCategoryGroup").options[document.querySelector("#subCategoryGroup").selectedIndex].value,
            document.querySelector("#conditionGroup").options[document.querySelector("#conditionGroup").selectedIndex].value,
            document.querySelector("#manufacturerGroup").options[document.querySelector("#manufacturerGroup").selectedIndex].value,
            document.querySelector("#stateGroup").options[document.querySelector("#stateGroup").selectedIndex].value,
            document.querySelector(".price").value
        )
        axios.put(`https://localhost:44374/ad`, ad)
            .then(response => {
                console.log(response)
            }).catch(error => {
                console.log(error.response.data)
            });



    }
    changeImage = () => {
        var formData = new FormData();
        formData.append("Img", document.querySelector(".image").files[0]);
        formData.append("adId", urlParams.get("id"));

        axios.put("https://localhost:44374/ad/image", formData, {
            headers: {
                'Content-Type': 'multipart/form-data'
            }
        }).then(response => {
            console.log(response)
        }).catch(error => {
            console.log(error)
        });
    }


}

class Ad {
    id;
    title;
    description;
    type;
    category;
    subcategoryid;
    condition;
    manufacturer;
    state;
    price;
    file;
    constructor(id, title, description, type, category, subcategoryid, condition, manufacturer, state, price, file) {
        this.id = id;
        this.title = title;
        this.description = description
        this.type = type
        this.category = category
        this.subcategoryid = subcategoryid;
        this.condition = condition;
        this.manufacturer = manufacturer;
        this.state = state;
        this.price = price;
        this.file = file;
    }
}


