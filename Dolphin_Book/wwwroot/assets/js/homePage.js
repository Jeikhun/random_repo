let slideIndex = 1;
showSlides(slideIndex);

// Next/previous controls
function plusSlides(n) {
  showSlides(slideIndex += n);
}

// Thumbnail image controls
function currentSlide(n) {
  showSlides(slideIndex = n);
}

function showSlides(n) {
  let i;
  let slides = document.getElementsByClassName("mySlides");
  let dots = document.getElementsByClassName("dot");
  if (n > slides.length) {slideIndex = 1}
  if (n < 1) {slideIndex = slides.length}
  for (i = 0; i < slides.length; i++) {
    slides[i].style.display = "none";
  }
  for (i = 0; i < dots.length; i++) {
    dots[i].className = dots[i].className.replace(" active", "");
  }
  slides[slideIndex-1].style.display = "block";
  dots[slideIndex-1].className += " active";
}
 $('.owl-carousel').owlCarousel({
    // loop: false,
//       margin: 0,
//       autoplay: false,
//       nav: true,
//       dots: true,
//       autoplayTimeout: 2000,
//       stagePadding: 0,
      loop: false,
      
      autoplay: false,
      margin: 90,
      nav: true,
      autoplayTimeout: 2000,
      navText: ["<div id='prev-arrowo' class='nav-button owl-prev'>‹</div>", "<div id='next-arrowo' class='nav-button owl-next'>›</div>"],
      stagePadding: 100,
      responsive: {
        0: {
          items: 1
        },
        600: {
          items: 2
        },
        1000: {
          items: 5
        }
      }
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

console.log(window.innerWidth);
console.log(screen.availWidth.valueOf);



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
// document.addEventListener('click', function handleClickOutsideBox(event) {


//     if (!clientInfo.contains(event.target)) {
//       clientInfo.style.display = 'none';
//     }
//   });