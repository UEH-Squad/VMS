
const filterBox = (className, placeHolder, dropdownParentNode) => {
    const alwaysShowPlaceholder = (value) => {
        if (!value.id) {
            return value.text;
        }
        else {
            return `${placeHolder}`;
        }
    }
    $(document).ready(function () {
        $(`${className}`).select2({
            placeholder: `${placeHolder}`,
            allowClear: true,
            templateSelection: alwaysShowPlaceholder,
            dropdownCssClass: "my-dropdown",
            dropdownParent: $(`${dropdownParentNode}`),
        });
    });
}


export default { filterBox };