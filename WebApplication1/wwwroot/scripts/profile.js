import ProfileController from "/scripts/modules/profileController.js"
let profileController = new ProfileController();
window.profileController = profileController;
const postBtn = document.querySelector(".submitAd")
postBtn.addEventListener("click", profileController.postAd)
