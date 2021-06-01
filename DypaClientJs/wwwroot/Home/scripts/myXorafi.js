
const fillXorafi = (data) => {
    console.log(data.xorafi)
    document.querySelector(".fieldAreaInfo").innerHTML = data.xorafi.title
    document.querySelector(".fieldAcresInfo").innerHTML = data.xorafi.acres
    document.querySelector(".fieldRootsInfo").innerHTML = data.xorafi.plantRoots
    document.querySelector(".fieldWaterSupplyInfo").innerHTML = data.xorafi.waterSupply
}
const urlParams = new URLSearchParams(window.location.search);
const idParam = urlParams.get('id');
document.querySelector(".weatherArea").innerHTML += `<weather-component fieldId=${idParam}></weather-component>`
axios.get("https://localhost:44331/xorafi/" + idParam).then(res => res.data).then(fillXorafi)