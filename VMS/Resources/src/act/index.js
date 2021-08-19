const showColor = () => {
    const actNotLikes = document.querySelectorAll(".act__notLike")
    const actLikes = document.querySelectorAll(".act__like")
    for (let i = 0; i < actNotLikes.length; i++) {
        actNotLikes[i].onclick = () => {
            actNotLikes[i].style.display = "none"
            actLikes[i].style.display = "inline-block"
        }

        actLikes[i].onclick = () => {
            actLikes[i].style.display = "none"
            actNotLikes[i].style.display = "inline-block"
        }
    }
}

export default { showColor};