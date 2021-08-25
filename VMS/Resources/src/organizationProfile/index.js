
const informationCarousel = () => {
    $('.information__carousel').owlCarousel({
        loop: true,
        margin: 0,
        nav: true,
        responsive: {
            0: {
                items: 3,
                slideBy: 1,
            },
        }
    })
}

export default { informationCarousel };