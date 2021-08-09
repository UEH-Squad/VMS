const playVideo = (src) => {
    const video = document.querySelector('.my-homepage-video');

    let timer = null,
        totalTime = 0,
        time = new Date();

    video.addEventListener("play", () => {
        timer = window.setInterval(() => {
            totalTime += new Date().getTime() - time.getTime();

            if (totalTime >= 5 * 1000) {
                document.querySelector('.my-slogan').style.display = 'none';
                clearInterval(timer);
            }
        }, 10);
    });

    if (video) {
        video.src = src;
        video.play();
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

export default { playVideo, filterCarousel, logoBanerCarousel, getUserLocation, showResult };