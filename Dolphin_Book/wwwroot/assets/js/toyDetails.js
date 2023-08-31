 
let burgerMenuList= document.querySelector(".burger-menu-list")
let burgerIcon = document.querySelector(".burger-icon");
let burgerCloseBtn = document.querySelector(".burger-close-btn");
let wrapper = document.querySelector(".wrapper");
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

accountBtn.addEventListener("click", (e)=>{
    if(clientInfo.style.display == "initial"){
        
        clientInfo.style.display = "none";
    }else{

        clientInfo.style.display = "initial";
        console.log(e);
    }


})
 
// wrapper.addEventListener("click", ()=>{
//     console.log('aaa');
//         clientInfo.style.display = "none";
    
// })

let basketBtn = document.querySelector(".bi-bag-check");
let basketContainer = document.querySelector(".basket-container");
basketBtn.addEventListener("click", ()=>{
    
    if(window.innerWidth>=1280){
        
        basketContainer.style.display = "flex";
        
    }
    else if(window.innerWidth<1280){

    }
})
let closeBurgerBtn = document.querySelector(".burger-close-button");
closeBurgerBtn.addEventListener("click", ()=>{
    
    basketContainer.style.display="none";
})
if(window.innerWidth<1280){
    
    basketContainer.style.display="none";
}
// document.addEventListener('click', function handleClickOutsideBox(event) {
    
  
//     if (!clientInfo.contains(event.target)) {
//       clientInfo.style.display = 'none';
//     }
//   });