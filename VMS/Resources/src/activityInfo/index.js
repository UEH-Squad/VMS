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
                slideBy: 4
            },
        }
    })
}

export default { otherAct };