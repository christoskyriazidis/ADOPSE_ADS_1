import Dictionary from "/scripts/modules/dictionary.js"
export default class CategoriesController {

    category;
    subcategory;
    constructor() {
        let dict = new Dictionary();
        const urlParams = new URLSearchParams(window.location.search);
        const category = urlParams.get('category');
        if (!category) {
            axios.get("https://localhost:44374/category")
                .then((response) => response.data)
                .then((data) => {
                    const catList = document.querySelector(".categories")
                    for (let object of data) {
                        catList.innerHTML += `<li id="${object.id}" onclick="categoriesController.selectCategory(${object.id})" style="background-image:url()">${object.title}</li>`
                    }
                })
        }
        else {
            this.category= urlParams.get('category');
            axios.get("https://localhost:44374/category/" + this.category)
                .then((response) => response.data)
                .then((data) => {

                    const catList = document.querySelector(".categories")
                    catList.innerHTML = "";
                    for (let object of data) {
                        catList.innerHTML += `<li id="${object.id}" onclick="categoriesController.selectSubCategory(${object.id})" style="background-image:url()">${object.title}</li>`
                    }
                })
        }

    }
    selectCategory = (id) => {
        window.location.href += "?category=" + id

    }
    selectSubCategory = (id) => {
        this.subcategory = "&subcategory=" + id
        window.location.href = "/home/search/index.html?"+ this.subcategory;
    }
}