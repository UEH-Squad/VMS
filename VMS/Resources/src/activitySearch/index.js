const filterBox = (className,placeHolder,dropdownParentNode) => {
    $(document).ready(function () {
        $(`${className}`).select2({
            placeholder: `${placeHolder}`,
            allowClear: true,
            dropdownCssClass: "my-dropdown",
            dropdownParent: $(`${dropdownParentNode}`),
        });
    });
}

const cascading = (parentClass, childClass) => {
    const parentSelect = document.querySelector(`${parentClass}`);
    const childSelect = document.querySelector(`${childClass}`);

    //turn all city id into keys of cityObject
    var cityObject = {};
    for (var i = 1; i <= parentSelect.options.length - 1; i++) {
        cityObject[`${parentSelect.options[i].value}`] = [`${parentSelect.options[i].innerText}`];
    }
    //turn all district id into an array
    var allDistrictArray = [];
    var allDistrictName = []; //this array contains names of district in Vietnamese
    for (var i = 1; i <= childSelect.options.length - 1; i++) {
        allDistrictArray = allDistrictArray.concat([`${childSelect.options[i].value}`]);
        allDistrictName = allDistrictName.concat([`${childSelect.options[i].innerText}`]);
    }

    //filter district due to city name, turn value of cityObject into arrays
    for (var key in cityObject) {
        for (var i = 0; i < allDistrictArray.length; i++) {
            if (allDistrictArray[i].includes(key)) {
                cityObject[`${key}`] = cityObject[`${key}`].concat(allDistrictArray[i]);
            }
        }
    }
    //reset html to render cascading dropdown
    parentSelect.options.length = 1;
    childSelect.options.length = 1;

    for (var city in cityObject) {
        //render city selection
        parentSelect.options[parentSelect.options.length] = new Option(`${cityObject.[city][0]}`, city);
    }

    parentSelect.onchange = (event) => {
        //empty district dropdown
        childSelect.options.length = 0;

        //render correct district
        const districtOfCityArray = cityObject.[event.target.value];
        if (event.target.value) {
            renderDistrict(districtOfCityArray, allDistrictArray, allDistrictName, childSelect);
        }
    }

}

var renderDistrict = (districtOfCityArray, allDistrictArray,allDistrictName, childSelect) => {
    for (var j = 1; j <= districtOfCityArray.length - 1; j++) {
        for (var i = 0; i < allDistrictArray.length; i++) {
            if (allDistrictArray[i] === districtOfCityArray[j]) {
                childSelect.options[j] = new Option(allDistrictName[i], allDistrictArray[i]);
            }
        }
    }
}

export default { filterBox, cascading };