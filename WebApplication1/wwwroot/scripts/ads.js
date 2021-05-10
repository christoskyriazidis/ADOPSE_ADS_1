let urlParams = new URLSearchParams(window.location.search);
import Dictionary from "/scripts/modules/dictionary.js"
// document.querySelector(".sendEmail").addEventListener("click", () => {
//     confirm("Do you want to send email ?")
// })
document.querySelector(".addToWishlist").addEventListener("click", () => {
    axios.post(`https://localhost:44374/wishlist/${urlParams.get("id")}`)
        .then(() => {
            alert("Ad added to wishlist succesfuly!")
        })
        .catch(() => {
            alert("Something seems to be wrong :\\ maybe you already are subscribed here")
        })
})
let customer;
//delete thingy
//axios.delete(`https://localhost:44374/wishlist`,{data:{adids:[199055]}})
axios.get(`https://localhost:44374/ad/${urlParams.get("id")}`)
    .then((response) => response.data)
    .then(data => {
        customer = data.customer;
        window.customer=customer
        document.querySelector(".description").innerHTML += data.description
        document.querySelector(".postedOn").innerHTML += data.createDate
        document.querySelector(".adLocation").innerHTML += data.address
        document.querySelector(".adPrice").innerHTML += data.price + "€"
        document.querySelector(".adTitle").innerHTML = `<h1>${data.title}</h1>`
        document.querySelector(".imageContainer").style.backgroundImage = `url(${data.img.replace("small", "full")})`;
        const dict=new Dictionary();
        window.dict=dict;
        //
        dict.getSubCategoryDictionary(data.category).then(map=>dict.dictify(map)).then(map=>{document.querySelector(".adSubcategory").innerHTML+=(map.get(data.subCategoryId))})
        document.querySelector(".sellerDetails").innerHTML = `
        <a href = "https://localhost:44366/home/profile/index.html?id=${data.customer}">
                    <span class="sellerAvatar" style="background-image:url('${data.profileImg}')"
                        name="avatar"></span>
                    <label label for="avatar"> ${data.username}</label>
                    <span class="filler"></span>
                    <span class="sellerReview">${starMethod(data.rating)}</span>(${data.reviews})
                    </a>
                `
        // document.querySelector(".description").innerHTML=data.description
        // document.querySelector(".description").innerHTML=data.description
    })
    .catch(x => console.log(x))
    .finally()


