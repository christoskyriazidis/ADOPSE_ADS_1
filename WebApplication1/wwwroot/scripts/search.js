import SearchController from "/scripts/modules/searchController.js"
let searchController = new SearchController();
window.searchController = searchController;
const searchbtn = document.querySelector(".submitSearch")
searchbtn.addEventListener("click", searchController.setFilters)
searchController.setLink(1)
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

