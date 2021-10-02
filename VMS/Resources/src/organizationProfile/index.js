
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

const organizeCarousel = () => {
    $('.organize__carousel').owlCarousel({
        loop: true,
        margin: 0,
        nav: true,
        responsive: {
            0: {
                items: 4,
                slideBy: 4,
                mouseDrag: false
            }
        }
    })
}

const editProfileCarousel = () => {
    
    const carousel = $('.editProfile__carousel');
    carousel.owlCarousel('destroy');
    carousel.owlCarousel({
        loop: true,
        margin: 0,
        nav: true,
        responsive: {

            0: {
                items: 4,
                slideBy: 1,
            },

        }
    })
}

const addOutsideClickMenuHandler = (dotnetHelper, methodName) => {
    window.addEventListener("click", (e) => {
        if (!$(e.target).hasClass('d-block')) {
            const elementToHide = document.querySelector("ul[class*='d-block']");
            if (elementToHide) {
                elementToHide.classList.remove('d-block');
                dotnetHelper.invokeMethodAsync(methodName);
            }
        }
    });
}

export default { informationCarousel, organizeCarousel, editProfileCarousel, addOutsideClickMenuHandler };