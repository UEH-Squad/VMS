import { result } from "lodash";

const playVideo = (src) => {
    const video = document.querySelector('.video-header__source');

    let timer = null,
        totalTime = 0;

    if (video) {
        video.src = src;
        video.play();

        video.addEventListener("play", () => {
            timer = window.setInterval(() => {
                totalTime += 1000;
                if (totalTime >= 6 * 1000) {
                    document.querySelector('.video-header__note').style.display = 'none';
                    clearInterval(timer);
                }
            }, 1000);
        });
    }
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
        Lat: position.coords.latitude,
        Long: position.coords.longitude,
    }
    localStorage.setItem('UserLocation', JSON.stringify(result));
}

export default { playVideo, filterCarousel, logoBanerCarousel, setUserLocation, getUserLocation, showResult };