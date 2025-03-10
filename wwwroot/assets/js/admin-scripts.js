$(document).ready(function () {
  // =======================================================================//
  // ------------------------------ POPUP ----------------------------------
  // =======================================================================//
  $("[data-popup-open]").click(function () {
    var targeted_popup_class = $(this).attr("data-popup-open");
    $('[data-popup="' + targeted_popup_class + '"]').fadeIn(350);
  });
  $("[data-popup-close]").click(function () {
    var targeted_popup_class = $(this).attr("data-popup-close");
    $('[data-popup="' + targeted_popup_class + '"]').fadeOut(350);
  });

  // =======================================================================//
  // ------------------------------ nav toggle -----------------------------
  // =======================================================================//
  $("nav li ul").hide();
  $("nav li > a").click(function () {
    $(this).next("ul").slideToggle("200");
  });
});


  // =======================================================================//
  // ------------------------------ Sweet Aletr ---------------------
  // =======================================================================//
  document.addEventListener("DOMContentLoaded", (event) => {
    document.addEventListener("click", function(e) {
        const link = e.target.closest('a[data-swal]');
        if (link) {
            e.preventDefault();
            const message = link.getAttribute("data-swal");
            Swal.fire({
              title: message,
              icon: 'warning', 
              confirmButtonText: 'نعم'
          });
        }
    });
});

  // =======================================================================//
  // ------------------------------ User Dropmenu ---------------------
  // =======================================================================//

document.addEventListener('DOMContentLoaded', function() {
  const userMenu = document.getElementById('userMenu');
  const dropdownMenu = document.getElementById('dropdownMenu');
  
  userMenu.addEventListener('click', function(e) {
    dropdownMenu.classList.toggle('show');
    e.stopPropagation();
  });
  
  document.addEventListener('click', function(e) {
    if (!userMenu.contains(e.target)) {
      dropdownMenu.classList.remove('show');
    }
  });
});