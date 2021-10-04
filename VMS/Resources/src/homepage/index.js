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
                items: 4,
                slideBy: 4,
            },
            1200: {
                items: 6,
                slideBy: 6,
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
                items: 1,
            },
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
    //const handleCounterUp = el => {
    //    new Waypoint({
    //        element: document.querySelector('.playCounter'),
    //        handler: function () {              
    //            counterUp(el);
    //            this.destroy();
    //        },
    //        offset: 'bottom-in-view',
    //    });
    //};

    $(document).ready(() => {
        const numbers = document.querySelectorAll('.counter');
        const nub1 = numbers[0].innerHTML;
        const nub2 = numbers[1].innerHTML;
        const nub3 = numbers[2].innerHTML;
        const nub4 = numbers[3].innerHTML;
        for (let i = 0; i < numbers.length; i++) {
            numbers[i].innerHTML = 0;
        }
        const observer = new IntersectionObserver(entries => {
            if (entries[0].isIntersecting === true) {
                numbers[0].innerHTML = nub1;
                numbers[1].innerHTML = nub2;
                numbers[2].innerHTML = nub3;
                numbers[3].innerHTML = nub4;
                const els = document.querySelectorAll('.counter');
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
        autoplaySpeed: 1500,
        navSpeed: 1500,
        dotsSpeed: 1500,
        responsiveClass: true,
        responsive: {
            0: {
                items: 1,
                stagePadding: 50,
            },
            1200: {
                items: 1,
                stagePadding: 140,
            },
            1400: {
                items: 1,
                stagePadding: 180,
            },
        }
    })
}

export default { playVideo, filterCarousel, logoBanerCarousel, getUserLocation, setUserLocation, increaseNumber, rankCarousel };