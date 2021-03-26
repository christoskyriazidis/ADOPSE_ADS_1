//check spa routing

document.querySelector(".wishlist").addEventListener("click", (event) => {
    
    if (!document.querySelector("wishlist-component")) {
        let wishlist = document.createElement("wishlist-component")
        document.body.appendChild(wishlist);
    }
    
    let x = event.clientX;
    let y = event.clientY;

    const wishlist=document.querySelector("wishlist-component");
    wishlist.focus();
    wishlist.style.display="block";
    wishlist.style.left = x + 'px';
    wishlist.style.top = y + 'px';
    wishlist.addEventListener("focusout",()=>{
        wishlist.style.display="none";
    })
})
