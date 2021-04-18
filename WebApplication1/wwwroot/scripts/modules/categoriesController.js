import Dictionary from "/scripts/modules/dictionary.js"
export default class CategoriesController {

    category;
    subcategory;
    constructor() {
    }
    fillMainPage = () => {
        axios.get("https://localhost:44374/category")
            .then((response) => response.data)
            .then((data) => {
                const catList = document.querySelector(".categories")
                for (let object of data) {
                    catList.innerHTML += `<li id="${object.id}" onclick="categoriesController.jumpTo(${object.id})" style="background-image:url(${object.imageUrl})"><span>${object.title}</span></li>`
                }
            })
    }
    jumpTo = (id) => {
        window.location.href = "/home/categories/index.html?category=" + id;
    }
    fillCategories = () => {
        let dict = new Dictionary();
        const urlParams = new URLSearchParams(window.location.search);
        const category = urlParams.get('category');
        console.log(category);
        if (!category) {
            axios.get("https://localhost:44374/category")
                .then((response) => response.data)
                .then((data) => {
                    const catList = document.querySelector(".categories")
                    for (let object of data) {
                        catList.innerHTML += `<li id="${object.id}" onclick="categoriesController.selectCategory(${object.id})" style="background-image:url(${object.imageUrl})"><span>${object.title}</span></li>`
                    }
                })
        }
        else {
            document.querySelector(".title").innerHTML = "Select a subcategory"
            this.category = urlParams.get('category');
            axios.get("https://localhost:44374/category/" + this.category)
                .then((response) => response.data)
                .then((data) => {

                    const catList = document.querySelector(".categories")
                    catList.innerHTML = "";
                    for (let object of data) {
                        catList.innerHTML += `<li id="${object.id}" onclick="categoriesController.selectSubCategory(${object.id})" style="background-image:url(${object.imageUrl})"><span>${object.title}</span></li>`
                    }
                })
        }
    }
    selectCategory = (id) => {
        if(!window.location.href.match(/\?/)){
            window.location.href += "?category=" + id
        }else{
            window.location.href += "&category=" + id
        }
        

    }
    selectSubCategory = (id) => {
        this.subcategory = "&subcategory=" + id
        console.log(this.subcategory);
        const urlParams = new URLSearchParams(window.location.search);
        const category = urlParams.get('category');
        console.log()
        window.location.href = "/home/search/index.html?category=" + category + this.subcategory+(urlParams.get('title')?`&title=${urlParams.get('title')}`:"");
    }
}