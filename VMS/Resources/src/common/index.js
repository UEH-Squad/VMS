export const smoothScrollTo = (element) => {
    const body = $("html, body");
    body.stop().animate({ scrollTop: $(element).offset().top }, 500, 'swing');
}

export const hookFileUploadEvent = async (previewImg, fileUploadRefId) => {
    let fileUpload = document.getElementById(fileUploadRefId);

    if (fileUpload !== null && previewImg !== null) {
        fileUpload.addEventListener('change', function (event) {
            previewImg.src = URL.createObjectURL(event.target.files[0]);
            previewImg.onload = function () {
                URL.revokeObjectURL(previewImg.src);
            }
        });
    }

    const bsCustomFileInput = await import('bs-custom-file-input');
    bsCustomFileInput.init();
}