const rankingCarousel = () => {
    $('.owl-carousel').owlCarousel({
        margin: 0,
        nav: true,
        responsive: {
            0: {
                items: 1,
                slideBy: 1,
            },
            768: {
                items: 2,
                slideBy: 2,
                margin: 20
            },
            1200: {
                items: 3,
                slideBy: 3,
                margin: 50
            },
        }
    })
}

export default {rankingCarousel};