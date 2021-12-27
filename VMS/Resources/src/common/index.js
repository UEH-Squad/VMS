export const smoothScrollTo = (element) => {
    const body = $("html, body");
    body.stop().animate({ scrollTop: $(element).offset().top }, 500, 'swing');
}

export const floatBackToTop = (isFloat) => {
    if (!isFloat) {
        return;
    }

    const fauxDivider = document.querySelector('.faux-divider');
    const backToTop = document.querySelector('#backToTop');
    const observer = new IntersectionObserver((entries, observer) => {
        entries.forEach(entry => {
            if (entry.intersectionRatio !== 1) {    // not visible
                backToTop.classList.remove('btn-back-top');
                backToTop.classList.add('btn-float-back-top');
            } else {    // visible
                backToTop.classList.remove('btn-float-back-top');
                backToTop.classList.add('btn-back-top');
            }
        });
    }, { threshold: 1 });
    observer.observe(fauxDivider);
}

export const hookFileUploadEvent = async (previewImg, fileUploadRefId, discardBtn, imgContainerId, originalSrc) => {
    const discardContainer = document.getElementById(imgContainerId);
    if (discardContainer && discardBtn && previewImg) {
        discardBtn.removeEventListener("click", onDiscardBtnClicked(previewImg, originalSrc));
        discardBtn.addEventListener("click", onDiscardBtnClicked(previewImg, originalSrc));
    }

    const fileUpload = document.getElementById(fileUploadRefId);
    if (fileUpload && previewImg) {
        fileUpload.removeEventListener('change', onImageChanged(previewImg));
        fileUpload.addEventListener('change', onImageChanged(previewImg));
    }

    const bsCustomFileInput = await import('bs-custom-file-input');
    bsCustomFileInput.init();
}

const onImageChanged = (previewImg) => (event) => {
    previewImg.src = URL.createObjectURL(event.target.files[0]);
    previewImg.onload = () => {
        URL.revokeObjectURL(previewImg.src);
    };
};

const onDiscardBtnClicked = (previewImg, originalSrc) => () => {
    previewImg.src = originalSrc;
};