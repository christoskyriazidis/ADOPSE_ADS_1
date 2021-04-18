import GenericResultInterface from "/scripts/modules/genericResultInterface.js"
import Dictionary from "/scripts/modules/dictionary.js"
window.dictionary=new Dictionary();
let searchController = new GenericResultInterface('search');
window.searchController = searchController;
const searchbtn = document.querySelector(".submitSearch")
searchbtn.addEventListener("click", searchController.setFilters)
document.querySelector(".searchButton").addEventListener("click",searchController.setFilters)
console.log(searchController)

// let dict = new Dictionary();
// dict.init().then(maps => {
//     maps.cat.forEach(fillCategories)
//     maps.sta.forEach(fillState)
//     maps.man.forEach(fillManufacturer)
//     maps.typ.forEach(fillType)
//     maps.con.forEach(fillCondition)
//     dictMaps = maps
// }).then(() => {
//     searchController.callCurrentLink()
// })

