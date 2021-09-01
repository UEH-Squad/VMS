export const smoothScrollTo = (element) => {
    const body = $("html, body");
    body.stop().animate({ scrollTop: $(element).offset().top }, 500, 'swing');
}
