const filterBox = (className,placeHolder) => {
    $(document).ready(function () {
        $(`${className}`).select2({
            placeholder: `${placeHolder}`,
            allowClear: true,
            dropdownCssClass: "my-dropdown",
            dropdownParent: $('.filter-item')
        });
    });
}

export default { filterBox };