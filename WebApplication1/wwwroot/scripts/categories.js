import CategoriesController from "/scripts/modules/categoriesController.js"
let categoriesController = new CategoriesController();
categoriesController.fillCategories();
window.categoriesController = categoriesController;