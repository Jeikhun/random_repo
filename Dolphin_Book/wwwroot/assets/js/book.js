let plusBtn = document.querySelectorAll(".bi-plus-lg");
let minusBtn = document.querySelectorAll(".bi-dash-lg");

plusBtn.forEach(element => {
    element.addEventListener("click", (e)=>{
        // if(e.target.parentElement.parentElement.parentElement.lastElementChild.style.display=="block" ){

        //     console.log(e.target.parentElement.parentElement.parentElement.lastElementChild);
        //     e.target.parentElement.parentElement.parentElement.lastElementChild.style.display="none";
        //     e.target.parentElement.parentElement.firstElementChild.style.color = "black"
            
        // }
    if(e.target.parentElement.parentElement.parentElement.lastElementChild.style.display=="none"||e.target.parentElement.parentElement.parentElement.lastElementChild.style.display==""){
        console.log("Aaaa");
        e.target.parentElement.parentElement.parentElement.lastElementChild.style.display="block";
        e.target.parentElement.parentElement.firstElementChild.style.color = "rgb(12, 69, 245)"
        e.target.parentElement.lastElementChild.style.display = "initial"
            e.target.style.display = "none"
    }
    });
});


minusBtn.forEach(element => {
    element.addEventListener("click", (e)=>{
        if(e.target.parentElement.parentElement.parentElement.lastElementChild.style.display=="block" ){

            console.log(e.target.parentElement.parentElement.parentElement.lastElementChild);
            e.target.parentElement.parentElement.parentElement.lastElementChild.style.display="none";
            e.target.parentElement.parentElement.firstElementChild.style.color = "black"
            e.target.parentElement.firstElementChild.style.display = "initial"
            e.target.style.display = "none"
        }

    });
});


let burgerMenuList= document.querySelector(".burger-menu-list")
let burgerIcon = document.querySelector(".burger-icon");
let burgerCloseBtn = document.querySelector(".burger-close-btn");
burgerIcon.addEventListener("click", ()=>{
    
    burgerMenuList.style.left="0px"

})
document.s
burgerCloseBtn.addEventListener("click", ()=>{
    
    burgerMenuList.style.left="-600px"
    
})

let personIcon = document.querySelector(".bi-person");
let clientInfo = document.querySelector(".client-info");
personIcon.addEventListener("click", ()=>{
    if(clientInfo.style.display == "initial"){
        
        clientInfo.style.display = "none";
    }else{

        clientInfo.style.display = "initial";
    }
})

let accountBtn = document.querySelector(".account");

accountBtn.addEventListener("click", ()=>{
    if(clientInfo.style.display == "initial"){
        
        clientInfo.style.display = "none";
    }else{

        clientInfo.style.display = "initial";
    }


})

let searchInput = document.querySelector(".search").firstElementChild;
searchInput.addEventListener("keyup", () => {
    const bookAuthor = document.querySelectorAll(".book-carousel-detail :first-child");
    const bookName = document.querySelectorAll('.book-carousel-detail :nth-child(2)');
    const bookCard = document.querySelectorAll(".books-grid>div")
    console.log(bookCard[1]);
    for (let i = 0; i < bookAuthor.length; i++) {
        if (bookAuthor[i].textContent.toLowerCase().includes(searchInput.value.toLowerCase()) || bookName[i].textContent.toLowerCase().includes(searchInput.value.toLowerCase())) {
            bookCard[i].style.display = "initial";
        } else {
            bookCard[i].style.display = "none";
        }

    }

});

let basketBtn = document.querySelector(".bi-bag-check");
let basketContainer = document.querySelector(".basket-container");
basketBtn.addEventListener("click", () => {

    if (window.innerWidth >= 1280) {

        basketContainer.style.display = "flex";

    }
    else if (window.innerWidth < 1280) {

    }
})
let closeBasketBtn = document.querySelector(".burger-close-button");
closeBasketBtn.addEventListener("click", () => {

    basketContainer.style.display = "none";
})
if (window.innerWidth < 1280) {

    basketContainer.style.display = "none";
}


