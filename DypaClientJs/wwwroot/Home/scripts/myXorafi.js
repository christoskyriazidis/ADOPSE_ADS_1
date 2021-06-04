
const fillXorafi = (data) => {
    console.log(data)
    document.querySelector(".fieldAreaInfo").innerHTML = data.xorafi.title
    document.querySelector(".fieldLocationInfo").innerHTML = data.xorafi.locationTitle
    document.querySelector(".fieldPresetInfo").innerHTML = data.xorafi.presetTitle
    document.querySelector(".fieldAcresInfo").innerHTML = data.xorafi.acres
    document.querySelector(".fieldRootsInfo").innerHTML = data.xorafi.plantRoots
    document.querySelector(".fieldWaterSupplyInfo").innerHTML = data.xorafi.waterSupply
    document.querySelector("#fieldDescription").value = data.xorafi.title
    document.querySelector("#fieldAcres").value = data.xorafi.acres
    document.querySelector("#fieldPlantRoots").value = data.xorafi.plantRoots
    document.querySelector("#fieldWaterSupply").value = data.xorafi.waterSupply
    const q = `https://maps.google.com/maps?q=${data.xorafi.latitude},${data.xorafi.longitude}&center=${data.xorafi.latitude},${data.xorafi.longitude}&maptype=satellite&key=AIzaSyBYNtt3TEftA6RmWg7PlntfcT7OZ6KJN84&output=embed&zoom=9`
    document.querySelector("#gmap_canvas").src = q
}
const fillSensorReport = (data) => {
  
    document.querySelector(".sensorReportIcon").src = "/images/" + data.icon.substring(0, 2) + ".svg"
    document.querySelector(".fieldTemperatureInfo").innerHTML = data.temp
    document.querySelector(".fieldHumidityInfo").innerHTML = data.humidity
    document.querySelector(".fieldPressureInfo").innerHTML = data.pressure
    document.querySelector(".fieldWindInfo").innerHTML = data.wind_speed
    document.querySelector(".fieldTimestampInfo").innerHTML = (new Date(parseInt(data.timestamp))).toLocaleTimeString()
}
const fillCalendar = (data) => {
    let row = ""
    let iterator = 0;
    for (const object of data) {
        
        if (iterator ==7) {
            iterator=0
            row += "<tr>"
        }
        row += `
                <td>
                    <div class="weatherCell ">
                        <h2></h2>
                        <img src="/images/${object.icon.substring(0, 2)}.svg" class="weatherIcon">
                        <div class="bottom">
                            <h5 class="degree">${Math.round(object.minTemp)}&#176;C</h5>
                            <h5 class="degree">${Math.round(object.wind_speed)} Km/h</h5>
                        </div>
                    </div>
                </td>`
        if (iterator ==7) {
            iterator=0;
            row += "</tr>"
        }
        iterator++
    }

    console.log("wtc", data)
    document.querySelector("#weatherCalendar").innerHTML += row
}
const fillLogs = (data) => {
    let row = ""
    for (const object of data) {
        row += `
            <tr>
            <td>${(new Date(parseInt(object.timestamp))).toLocaleString()}</td>
            <td>${object.pressure} mbar</td>
            <td>${object.temp} C</td>
            <td>${object.humidity}%</td>
            <td>${object.wind_speed} m/s</td>
            <td>${object.humidity}%</td>
            </tr>
            `
    }

    document.querySelector("#customers").innerHTML += row;
}
const urlParams = new URLSearchParams(window.location.search);
const idParam = urlParams.get('id');
document.querySelector(".weatherArea").innerHTML += `<weather-component fieldId=${idParam}></weather-component>`
axios.get("https://localhost:44357/xorafi/" + idParam).then(res => res.data).then(fillXorafi)
axios.get("https://localhost:44357/xorafi/hourly/" + idParam).then(res => res.data).then(fillSensorReport)
axios.get("https://localhost:44357/xorafi/hourly/all/" + idParam).then(res => res.data).then(fillLogs)
axios.get(`https://localhost:44357/xorafi/monthly?xorafiid=${idParam}&pagenumber=1`).then(res => res.data).then(fillCalendar)
const deleteField = () => {
    axios.delete()
}