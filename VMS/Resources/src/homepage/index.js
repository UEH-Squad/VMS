import counterUp from 'counterup2';

const playVideo = (src) => {
    const video = document.querySelector('.video-header__source');
    video.src = src;
    video.play();
}

const filterCarousel = () => {
    $('.filter__carousel').owlCarousel({
        loop: true,
        margin: 0,
        nav: true,
        responsive: {
            0: {
                items: 3,
                slideBy: 3
            },
            768: {
                items: 4,
                slideBy: 4
            },
            992: {
                items: 6,
                slideBy: 6
            },
            1200: {
                items: 6,
                slideBy: 6
            },
        }
    })
}

const logoBanerCarousel = () => {
    $('.logoBaner__carousel').owlCarousel({
        loop: true,
        margin: 0,
        nav: true,
        responsive: {
            0: {
                items: 1
            },
            992: {
                items: 1
            }
        }
    })
}

const setUserLocation = () => {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(setPosition);
    } else {
        return null;
    }
}

const getUserLocation = () => {
    const location = localStorage.getItem('UserLocation');
    if (location) {
        return JSON.parse(location);
    }
    return null;
}

const setPosition = position => {
    var result = {
        Latitude: position.coords.latitude,
        Longitude: position.coords.longitude,
    }
    localStorage.setItem('UserLocation', JSON.stringify(result));
}

const increaseNumber = () => {
    $(document).ready(() => {
        const els = document.querySelectorAll('.counter');
        const numbers = [];
        for (let i = 0; i < els.length; i++) {
            numbers.push(els[i].innerHTML);
            els[i].innerHTML = 0;
        }
        const observer = new IntersectionObserver(entries => {
            if (entries[0].isIntersecting === true) {
                for (let i = 0; i < els.length; i++) {
                    els[i].innerHTML = numbers[i];
                }
                [].forEach.call(els, counterUp);
            }
        }, { threshold: [1] });
        observer.observe(document.querySelector(".my-quoteBaner"));

    });



}

const rankCarousel = () => {
    const carousel = $('.rank__owlcrousel');
    carousel.owlCarousel('destroy');
    carousel.owlCarousel({
        loop: true,
        margin: 0,
        nav: true,
        autoplay: true,
        autoplayTimeout: 5000,
        autoplaySpeed: 2000,
        autoplayHoverPause: true,
        navSpeed: 2000,
        dotsSpeed: 2000,
        responsiveClass: true,
        responsive: {
            0: {
                items: 1,
                stagePadding: 0,
            },
            992: {
                items: 1,
                stagePadding: 100,
            },
            1200: {
                items: 1,
                stagePadding: 135,
            },
            1400: {
                items: 1,
                stagePadding: 160,
            },
        }
    })
}

const addOutsideClickNavBarHandler = () => {
    const hideNavBar = () => {
        if ($('#navbarSupportedContent').hasClass('show')) {
            $('#navbarSupportedContent').removeClass('show');
        }

        if (!$('my-navbar__hamburger-wrapper__btn').hasClass('collapsed')) {
            $('my-navbar__hamburger-wrapper__btn').addClass('collapsed');
        }
    };

    window.addEventListener('click', (e) => {
        if (!document.getElementById('navbarSupportedContent').contains(e.target)) {
            hideNavBar();
        }
    });

    let previousUrl = '';
    const observer = new MutationObserver(function (mutations) {
        if (location.href !== previousUrl) {
            previousUrl = location.href;
            hideNavBar();
        }
    });

    const config = { subtree: true, childList: true };
    observer.observe(document, config);
}

export default { playVideo, filterCarousel, logoBanerCarousel, getUserLocation, setUserLocation, increaseNumber, rankCarousel, addOutsideClickNavBarHandler };