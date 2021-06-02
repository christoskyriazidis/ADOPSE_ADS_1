
const fillXorafi = (data) => {
    document.querySelector(".fieldAreaInfo").innerHTML = data.xorafi.title
    document.querySelector(".fieldAcresInfo").innerHTML = data.xorafi.acres
    document.querySelector(".fieldRootsInfo").innerHTML = data.xorafi.plantRoots
    document.querySelector(".fieldWaterSupplyInfo").innerHTML = data.xorafi.waterSupply
    document.querySelector("#fieldDescription").value=data.xorafi.title
    document.querySelector("#fieldAcres").value=data.xorafi.acres
    document.querySelector("#fieldPlantRoots").value=data.xorafi.plantRoots
    document.querySelector("#fieldWaterSupply").value=data.xorafi.waterSupply
    const q=`https://maps.google.com/maps?q=${data.xorafi.latitude},${data.xorafi.longitude}&center=${data.xorafi.latitude},${data.xorafi.longitude}&maptype=satellite&key=AIzaSyBYNtt3TEftA6RmWg7PlntfcT7OZ6KJN84&output=embed&zoom=9`
    document.querySelector("#gmap_canvas").src=q
}
const fillSensorReport=(data)=>{
    console.log(data)
    document.querySelector(".sensorReportIcon").src="/images/"+data.icon.substring(0,2)+".svg"
    document.querySelector(".fieldTemperatureInfo").innerHTML = data.temp
    document.querySelector(".fieldHumidityInfo").innerHTML = data.humidity
    document.querySelector(".fieldPressureInfo").innerHTML = data.pressure
    document.querySelector(".fieldWindInfo").innerHTML = data.wind_speed
    document.querySelector(".fieldTimestampInfo").innerHTML = (new Date(parseInt(data.timestamp))).toLocaleTimeString()
}

const urlParams = new URLSearchParams(window.location.search);
const idParam = urlParams.get('id');
document.querySelector(".weatherArea").innerHTML += `<weather-component fieldId=${idParam}></weather-component>`
axios.get("https://localhost:44331/xorafi/" + idParam).then(res => res.data).then(fillXorafi)
axios.get("https://localhost:44331/xorafi/hourly/" + idParam).then(res => res.data).then(fillSensorReport)