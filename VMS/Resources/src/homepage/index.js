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

const getUserLocation = () => {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(showPosition);
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

let showPosition = position => {
    var result = {
        Lat: position.coords.latitude,
        Long: position.coords.longitude,
    }
    return result;
}

const increaseNumber1 = () => {
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

export default { playVideo, filterCarousel, logoBanerCarousel, getUserLocation, showResult, increaseNumber1 };