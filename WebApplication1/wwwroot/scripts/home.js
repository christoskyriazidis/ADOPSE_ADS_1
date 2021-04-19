import CategoriesController from "/scripts/modules/categoriesController.js"
import SearchController from "/scripts/modules/searchController.js"
import GenericResultInterface from "/scripts/modules/genericResultInterface.js"
import Dictionary from "/scripts/modules/dictionary.js"
let categoriesController = new CategoriesController();
categoriesController.fillMainPage();
window.categoriesController = categoriesController;
let searchController = new GenericResultInterface('home');
window.searchController = searchController;
document.querySelector(".searchBtn").addEventListener("click",()=> {
    searchController.setSearchQuery()
    window.location.href = "/home/categories/index.html?"+searchController.search;
})
