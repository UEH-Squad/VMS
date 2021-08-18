function showColor() {
    console.log("ALo")
    let notLike = document.querySelector(".act__notLike");
    if (notLike.style.display !== "none") {
        document.querySelector(".act__notLike").style.display = "none";
        document.querySelector(".act__like").style.display = "inline-block";
    } else {
        document.querySelector(".act__notLike").style.display = "inline-block";
        document.querySelector(".act__like").style.display = "none";
    }
}

export default { showColor };