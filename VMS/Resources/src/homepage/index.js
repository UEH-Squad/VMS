const slick = (slidesToShow, slidesToScroll, isInfinite) => {
    $('.slick-container').slick({
        infinite: isInfinite,
        slidesToShow: 3,
        slidesToScroll: slidesToScroll,
        prevArrow: document.getElementById('prev-btn'),
        nextArrow: document.getElementById('next-btn'),
        mobileFirst: true,
        responsive: [
            {
                breakpoint: 992,
                settings: {
                    slidesToShow: slidesToShow
                }
            },
            {
                breakpoint: 576,
                settings: {
                    slidesToShow: 5,
                    slidesToScroll: slidesToScroll + 2
                }
            }
        ]
    });
}

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

const getUserLocation = () => {
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(showPosition);
        } else {
            return null;
        }
    }

let showPosition = position => {
    var result = {
        Lat: position.coords.latitude,
        Long: position.coords.longitude,
    }
    return result;
}

export default { slick, playVideo, getUserLocation };