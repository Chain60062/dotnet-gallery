function darken_screen(yesno) {
  if (yesno == true) {
    document.querySelector(".screen-darken").classList.add("active");
  } else if (yesno == false) {
    document.querySelector(".screen-darken").classList.remove("active");
  }
}

function close_offcanvas() {
  darken_screen(false);
  document.querySelector(".mobile-offcanvas.show").classList.remove("show");
  document.body.classList.remove("offcanvas-active");
}

function show_offcanvas() {
  darken_screen(true);
  document.getElementById("nav").classList.add("show");
  document.body.classList.add("offcanvas-active");
}

document.addEventListener("DOMContentLoaded", function () {
  document.getElementById("btn-open").addEventListener("click", function (e) {
    e.preventDefault();
    show_offcanvas();
  });

  document.getElementById("btn-close").addEventListener("click", function (e) {
    e.preventDefault();
    close_offcanvas();
  });

  document
    .querySelector(".screen-darken")
    .addEventListener("click", function (event) {
      close_offcanvas();
    });
});
/*
 * Topnav
 */
// function myFunction() {
//   let topnav = document.getElementById("myTopnav");
//   if (topnav.className === "topnav") {
//     topnav.className += " responsive";
//   } else {
//     topnav.className = "topnav";
//   }
// }
