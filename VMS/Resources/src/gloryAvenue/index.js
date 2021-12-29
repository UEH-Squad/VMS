const rankingCarousel = () => {
    $('.owl-carousel').owlCarousel({
        margin: 0,
        nav: true,
        responsive: {
            0: {
                items: 1,
                slideBy: 1,
            },
            992: {
                items: 3,
                slideBy: 3,
                margin: 50
            },
        }
    })
}

export default {rankingCarousel};