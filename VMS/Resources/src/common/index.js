export const smoothScrollTo = (element) => {
    const body = $(element);
    body.stop().animate({ scrollTop: 0 }, 500, 'swing');
}
