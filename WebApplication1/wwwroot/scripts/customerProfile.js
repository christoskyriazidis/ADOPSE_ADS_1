let urlParams = new URLSearchParams(window.location.search);
import GenericResultInterface from "/scripts/modules/genericResultInterface.js"

axios.get("https://localhost:44374/customer/" + urlParams.get("id")).then(res => res.data).then(data => {
    document.querySelector(".username").innerHTML += data.username
    document.querySelector(".realName").innerHTML += data.name + data.lastName
    document.querySelector(".email").innerHTML += data.email;
    document.querySelector(".address").innerHTML += data.address
    document.querySelector(".profilePicture").style.backgroundImage = `url(${data.profileImg})`
    let customerId = { id: data.id }
    let searchController = new GenericResultInterface("customerAds",customerId);
    window.searchController = searchController;

})
