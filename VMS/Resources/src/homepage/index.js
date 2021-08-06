﻿const playVideo = (src) => {
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


export default { playVideo, filterCarousel, logoBanerCarousel};