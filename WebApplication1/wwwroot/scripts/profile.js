import ProfileController from "/scripts/modules/profileController.js"
let profileController = new ProfileController();
window.profileController = profileController;
if(document.querySelector(".submitAd")){
    const postBtn = document.querySelector(".submitAd")
    postBtn.addEventListener("click", profileController.postAd)
}
if(document.querySelector(".editAd")){
    const editAd = document.querySelector(".editAd")
    editAd.addEventListener("click",profileController.editAd)
    const changeImage=document.querySelector(".changeImage")
    changeImage.addEventListener("click",profileController.changeImage)
}
