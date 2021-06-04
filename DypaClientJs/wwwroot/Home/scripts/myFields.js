const generateXwrafi = (data) => {
    let template = ""
    console.log(data)
    for (const object of data) {
        template += `
        <div class="previewContainer">
            <a href="https://localhost:44376/home/fieldOverview.html?id=${object.id}">
                <div class="outterContainer">
                    <div class="leftInner">
                        <div class="leftInnerUpper">
                            <span class="fieldTitle">${object.title}</span>
                            <span class="fieldLocation">${object.locationTitle}</span>
                        </div>
                        <div class="leftInnerLower">
                            <span class="presetIcon">
                                <img src="/images/${object.imgUrl.substring(0, 2)}.svg" alt=""></span>
                            <span class="presetTitle">${object.presetTitle}</span>
                        </div>
                    </div>
                    <div class="rightInner">
                        <div class="rightInnerLeft">
                            <span class="temperature">🌡️ <div class="filler"></div>${object.temp} &#176;C</span>
                            <span class="windSpeed">💨<div class="filler"></div>${object.wind_speed} km/h</span>
                            <span class="humidity">💧<div class="filler"></div>${object.humidity}%</span>
                            <span class="pressure">🕛<div class="filler"></div>${object.pressure}mbar</span>
                        </div>
                        <div class="rightInnerRight">
                            <span ><img class="weatherIcon" src="${object.icon==null?"":"/images/"+object.icon.substring(0, 2)}.svg" alt=""></span>
                        </div>
                    </div>
                </div>
            </a>
        </div>`
    }
    document.querySelector("main").innerHTML=template
}
console.log("haahah")
axios.get("https://localhost:44357/owner/xorafi").then(res=>res.data).then(generateXwrafi)
const field={
    id:15,
    title:"Κατσαβραχα",
    location:"Ψαχνα Ευβοιας, Ελλαδα",
    presetTitle:"Καλαμποκια",
    temperature:16,
    windSpeed:4.5,
    humidity:65,
    pressure:1001,
    currentWeather:"04d",
    presetImage:"02d"
}
const data=[field,field,field,field,field,field]
// generateXwrafi(data);
