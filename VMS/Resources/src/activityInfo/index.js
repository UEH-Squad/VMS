const otherAct = (count) => {
    const isLoop = count >= 3;
    $('.owl-carousel').owlCarousel({
        loop: isLoop,
        margin: 10,
        nav: true,
        navText: [
            '<span class="material-icons">navigate_before</span>',
            '<span class="material-icons">navigate_next</span>'
        ],
        dots: false,
        responsive: {
            0: {
                items: 3,
                slideBy: 3,
            },
            1200: {
                items: 4,
                slideBy: 3,
            },
        }
    })
}

export default { otherAct };