(function ($) {
  "use strict";

  var $window = $(window);

  $window.on('load', function () {
    $window.trigger("resize");

  });

  // Preloader
  $('.loader').fadeOut();
  $('.loader-mask').delay(350).fadeOut('slow');


  // Init
  initOwlCarousel();


  $window.on('resize', function () {
    hideSidenav();
    initIsotope();
  });

  //Widget-Live-Remove

  window.addEventListener('load', function () {
    const widgetSettingElement = document.querySelector(".widget-setting");

    if (widgetSettingElement && widgetSettingElement.firstElementChild && widgetSettingElement.firstElementChild.firstElementChild) {
      const targetElement = widgetSettingElement.firstElementChild.firstElementChild.nextElementSibling;

      targetElement.classList.add("widget-live-remove");
    }
  });

  /*Time detect*/
  function updateDate() {
    const dateElement = document.getElementById("currentDate");
    const currentDate = new Date();
    const options = { weekday: 'long', month: 'long', day: 'numeric', year: 'numeric' };
    const formattedDate = currentDate.toLocaleDateString('en-US', options);
    dateElement.textContent = formattedDate;
  }

  // Call the updateDate function initially to display the date
  updateDate();

  // Update the date every second (1000 milliseconds)
  setInterval(updateDate, 1000);

  
  /* Mobile Detect
  -------------------------------------------------------*/
  if (/Android|iPhone|iPad|iPod|BlackBerry|Windows Phone/i.test(navigator.userAgent || navigator.vendor || window.opera)) {
    $("html").addClass("mobile");
    $('.dropdown-toggle').attr('data-toggle', 'dropdown');
  }
  else {
    $("html").removeClass("mobile");
  }

  /* IE Detect
  -------------------------------------------------------*/
  if (Function('/*@cc_on return document.documentMode===10@*/')()) { $("html").addClass("ie"); }



  /* Sticky Navigation
  -------------------------------------------------------*/
  $window.scroll(function () {
    scrollToTop();
    stickyNav();
  });

  var $stickyNav = $('.nav--sticky');
  var $nav = $('.nav');

  function stickyNav() {
    if ($window.scrollTop() > 2) {
      $stickyNav.addClass('sticky');
      $nav.addClass('sticky');
    } else {
      $stickyNav.removeClass('sticky');
      $nav.removeClass('sticky');
    }
  }


  /* Mobile Navigation
  -------------------------------------------------------*/
  var $sidenav = $('#sidenav'),
    $mainContainer = $('#main-container'),
    $navIconToggle = $('.nav-icon-toggle'),
    $navHolder = $('.nav-holder'),
    $contentOverlay = $('.content-overlay'),
    $htmlContainer = $('html'),
    $sidenavCloseButton = $('#sidenav-close-button');


  $navIconToggle.on('click', function (e) {
    e.stopPropagation();
    $(this).toggleClass('nav-icon-toggle--is-open');
    $sidenav.toggleClass('sidenav--is-open');
    $contentOverlay.toggleClass('content-overlay--is-visible');
    // $htmlContainer.toggleClass('oh');
  });

  function resetNav() {
    $navIconToggle.removeClass('nav-icon-toggle--is-open');
    $sidenav.removeClass('sidenav--is-open');
    $contentOverlay.removeClass('content-overlay--is-visible');
    // $htmlContainer.removeClass('oh');
  }

  function minWidth(width) {
    return window.innerWidth >= width;
  }

  function hideSidenav() {
    if (minWidth(992)) {
      resetNav();
    }
  }



  $contentOverlay.on('click', function () {
    resetNav();
  });

  $sidenavCloseButton.on('click', function () {
    resetNav();
  });


  var $dropdownTrigger = $('.nav-dropdown-trigger'),
    $navDropdownMenu = $('.nav-dropdown-menu'),
    $navDropdown = $('.nav-dropdown');


  if ($('html').hasClass('mobile')) {

    $('body').on('click', function () {
      $navDropdownMenu.addClass('hide-dropdown');
    });

    $navDropdown.on('click', '> a', function (e) {
      e.preventDefault();
    });

    $navDropdown.on('click', function (e) {
      e.stopPropagation();
      $navDropdownMenu.removeClass('hide-dropdown');
    });
  }


  /* Sidenav Menu
  -------------------------------------------------------*/

  $('.sidebar-toggles').on('click', function (e) {
    var $this = $(this).children().first().next();

    $this.parent().siblings().removeClass('sidenav-menu--is-open');
    $this.parent().siblings().find('li').removeClass('sidenav-menu--is-open');
    $this.parent().find('li').removeClass('sidenav-menu--is-open');
    $this.parent().toggleClass('sidenav-menu--is-open');

    if ($this.next().hasClass('show')) {
      $this.next().removeClass('show').slideUp(350);
    } else {
      $this.parent().parent().find('li .sidenav-menu-dropdown').removeClass('show').slideUp(350);
      $this.next().toggleClass('show').slideToggle(350);
    }
  });


  /* Nav Search
  -------------------------------------------------------*/
  (function () {
    var $navSearchForm = $('.nav-search-form'),
      $navSearchTrigger = $('.nav-search-trigger'),
      $navSearchInput = $('.nav-search-input'),
      $navSearchClose = $('.nav-search-close');

    $navSearchTrigger.on('click', function (e) {
      e.preventDefault();
      $navSearchForm.animate({ opacity: 'toggle' }, 500);
      $navSearchInput.focus();
    });

    $navSearchClose.on('click', function (e) {
      e.preventDefault();
      $navSearchForm.animate({ opacity: 'toggle' }, 500);
    });

    function closeSearch() {
      $navSearchForm.fadeOut(200);
    }

    $(document.body).on('click', function (e) {
      closeSearch();
    });

    $navSearchInput.add($navSearchTrigger).on('click', function (e) {
      e.stopPropagation();
    });
  })();




  /* Sticky sidebar
  -------------------------------------------------------*/
  (function () {
    var $stickyCol = $('.sticky-col');
    if ($stickyCol.length > 0) {
      $stickyCol.stick_in_parent({
        offset_top: 40
      });
    }
  })();


  /* Isotope
  -------------------------------------------------------*/
  function initIsotope() {
    var $isotopeGrid = $('#isotope-grid');
    if ($isotopeGrid.length > 0) {
      $isotopeGrid.imagesLoaded(function () {
        $isotopeGrid.isotope({
          layoutMode: 'masonry'
        });
        $isotopeGrid.isotope();
      });
    }
  }


  /* Tabs
  -------------------------------------------------------*/
  $('.tabs__trigger').on('click', function (e) {
    var currentAttrValue = $(this).attr('href');
    $('.tabs-content-trigger ' + currentAttrValue).stop().fadeIn(1000).siblings().hide();
    $(this).parent('li').addClass('tabs-item--active').siblings().removeClass('tabs-item--active');
    e.preventDefault();
  });


  /* Owl Carousel
  -------------------------------------------------------*/



  function initOwlCarousel() {
    $(".owl-carousel-slider").owlCarousel({
      items: 1,
      loop: true,
      nav: true,
      dots: true,
      lazyLoad: true,
      navSpeed: 500,
      navText: ['<i class="fas fa-chevron-left"></i>', '<i class="fas fa-chevron-right"></i>']
    });

    // Hero
    $(".owl-hero-grid").owlCarousel({
      center: false,
      items: 1,
      loop: true,
      nav: true,
      dots: true,
      margin: 0,
      lazyLoad: true,
      navSpeed: 500,
      navText: ['<i class="ui-arrow-left">', '<i class="ui-arrow-right">']
    });

    // Single Image
    $("#owl-single").owlCarousel({
      items: 1,
      loop: true,
      nav: true,
      dots: false,
      lazyLoad: true,
      navSpeed: 500,
      navText: ['<i class="ui-arrow-left">', '<i class="ui-arrow-right">']
    });

    // Single Post Gallery
    $("#owl-single-post-gallery").owlCarousel({
      items: 1,
      loop: true,
      nav: true,
      dots: true,
      lazyLoad: true,
      navSpeed: 500,
      navText: ['<i class="ui-arrow-left">', '<i class="ui-arrow-right">']
    });

    // Custom nav
    var owlNav = $('#owl-posts').owlCarousel();
    $(".carousel-nav__btn--prev").on('click', function () {
      owlNav.trigger('prev.owl.carousel');
    });

    $(".carousel-nav__btn--next").on('click', function () {
      owlNav.trigger('next.owl.carousel');
    });
  };



  /* Scroll to Top
  -------------------------------------------------------*/
  function scrollToTop() {
    var scroll = $window.scrollTop();
    var $backToTop = $("#back-to-top");
    if (scroll >= 50) {
      $backToTop.addClass("show");
    } else {
      $backToTop.removeClass("show");
    }
  }

  $('a[href="#top"]').on('click', function () {
    $('html, body').animate({ scrollTop: 0 }, 1000, "easeInOutQuint");
    return false;
  });

})(jQuery);