const activityCarousel = () => {
    $('.activity__carousel').owlCarousel({
        loop: true,
        margin: 0,
        nav: true,
        responsive: {
            0: {
                items: 3,
                slideBy: 3,
            }
        }
    })
}

const organizeCarousel = () => {
    $('.organize__carousel').owlCarousel({
        loop: true,
        margin: 0,
        nav: true,
        responsive: {
            0: {
                items: 4,
                slideBy: 4,
            }
        }
    })
}

export default { activityCarousel, organizeCarousel };