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
const showResult = () => {
    const counters = document.querySelectorAll('.counter');
    const boxInfo = document.querySelector('.BoxInformation');
    const boxInfoY = boxInfo.offsetTop;

    const showAnimation = () => {
        const viewportY = window.pageYOffset + window.innerHeight;

        if (viewportY >= boxInfoY) {
            counters.forEach(each => IncreaseNumber(each));
        }
    }
    const event = () => {
        window.addEventListener('scroll', showAnimation);
    }
    event();
}

const IncreaseNumber = (counter) => {
    const speed = 200;
    const target = parseInt(counter.dataset.target);
    const updateCount = () => {
        const count = +counter.innerText;
        const increasement = target / speed;
        if (count < target) {
            counter.innerText = Math.ceil(count + increasement);
            setTimeout(updateCount, 200);
        }
        else {
            counter.innerText = target;
        }
    }

    updateCount();
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
    const handleCounterUp = el => {
        new Waypoint({
            element: el,
            handler: function () {
                counterUp(el);
                this.destroy();
            },
            offset: 'bottom-in-view',
        });
    };

    $(document).ready(() => {
        const els = document.querySelectorAll('.counter');
        [].forEach.call(els, handleCounterUp);
    });
}

export default { playVideo, filterCarousel, logoBanerCarousel, getUserLocation, setUserLocation, increaseNumber };