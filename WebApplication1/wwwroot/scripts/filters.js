let finalQuery=null;
const createQueryString = () => {
    var regex1 = /[0-9]{1,2}/;
    const categoryGroup = document.getElementsByName("category");
    const manufacturerGroup = document.getElementsByName("manufacturer");
    const typeGroup = document.getElementsByName("type");
    const conditionGroup = document.getElementsByName("condition");
    const stateGroup = document.getElementsByName("state");
    let categoryString = "category="
    let typeString = "type="
    let manufacturerString = "manufacturer="
    let conditionString = "condition="
    let stateString = "state="
    for (category of categoryGroup) {
        if (category.checked) {
            categoryString += `${category.id.match(regex1)}_`;
        }
    }
    for (manufacturer of manufacturerGroup) {
        if (manufacturer.checked) {
            manufacturerString += `${manufacturer.id.match(regex1)}_`;
        }
    }
    for (type of typeGroup) {
        if (type.checked) {
            typeString += `${type.id.match(regex1)}_`;
        }
    }
    for (state of stateGroup) {
        if (state.checked) {
            stateString += `${state.id.match(regex1)}_`;
        }
    }
    for (condition of conditionGroup) {
        if (condition.checked) {
            conditionString += `${condition.id.match(regex1)}_`;
        }
    }
    if (categoryString == "category=") {
        categoryString = ""
    }
    if (typeString == "type=") {
        typeString = ""
    }
    if (manufacturerString == "manufacturer=") {
        manufacturerString = ""
    }
    if (stateString == "state=") {
        stateString = ""
    }
    if (conditionString == "condition=") {
        conditionString = ""
    }
    const allParams = [categoryString, typeString, manufacturerString, stateString, conditionString]
    finalQuery = "&";
    for (param of allParams) {
        if (param != "") {
            param = param.substring(0, param.length - 1);
            param += "&"
            finalQuery += param;
        }
    }

    finalQuery = finalQuery.substring(0, finalQuery.length - 1)
    console.log(lastCall);
    for (pager of document.querySelectorAll("pagination-component")) {
        pager.setAttribute("filter", finalQuery)
        
    }
    callPage(lastCall + finalQuery)
}