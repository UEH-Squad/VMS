const otherAct = () => {
    $('.owl-carousel').owlCarousel({
        loop: false,
        margin: 10,
        nav: true,
        dots: false,
        responsive: {
            0: {
                items: 2,
                slideBy: 2,
                nav: false
            },
            576: {
                items: 3,
                slideBy: 3,
                nav: false
            },
            1200: {
                items: 4,
                slideBy: 4,
                mouseDrag: false
            },
        }
    })
}

export default { otherAct };