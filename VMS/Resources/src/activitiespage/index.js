const otherAct = () => {
    $('.owl-carousel').owlCarousel({
        loop: true,
        margin: 10,
        nav: true,
        navText: [
            '<span class="material-icons">navigate_before</span>',
            '<span class="material-icons">navigate_next</span>'
        ],
        dots: false,
        responsive: {
            0: {
                items: 1
            },
        }
    })
}

export default { otherAct };