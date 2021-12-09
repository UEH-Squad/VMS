const otherAct = (count) => {
    const isLoop = count >= 3;
    $('.owl-carousel').owlCarousel({
        loop: isLoop,
        margin: 10,
        nav: true,
        dots: false,
        responsive: {
            0: {
                items: 3,
                slideBy: 3,
                nav: false
            },
            992: {
                items: 4,
                slideBy: 4,
                mouseDrag: false
            },
        }
    })
}

export default { otherAct };