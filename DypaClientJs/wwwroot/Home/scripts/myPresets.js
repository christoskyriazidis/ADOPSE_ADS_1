
const makePresets = (data) => {
    let presets = ""
    for (const object of data) {
        presets += `
        <div class="outterContainer">
            <div class="left">
                <span class="title">${object.title}</span>
            </div>
            <div class="middle">
                <span class="lowestNormalHumidity">Lowest normal humidity: <div class="filler"></div> ${object.lowestNormalSoilMoisture}%</span>
                <span class="optimalHumidity">Optimal humidity: <div class="filler"></div> ${object.optimalSoilMoisture}%</span>
                <span class="upperNormalHumidity">Highest normal humidity: <div class="filler"></div> ${object.upperNormalSoilMoisture}%</span>
            </div>
            <div class="right">
                <span class="litersOfWaterPerRootPerWeekSummer">Avg L/Root/Week summer: <div class="filler"></div>${object.weeklyRootWaterSummer}L/w/r</span>
                <span class="litersOfWaterPerRootPerWeekWinter">Avg L/Root/Week winter: <div class="filler"></div>${object.weeklyRootWaterWinter}L/w/r</span>
            </div>
        </div>
`
    }
    document.querySelector("main").innerHTML += presets;
}
axios.get("https://localhost:44357/owner/category").then(res => res.data)
    .then(makePresets)
const postPreset = () => {

    var formData = new FormData();
    formData.append("Image", document.querySelector("#image").files[0]);
    formData.append("Title", document.querySelector("#presetDescriptionAdd").value);
    formData.append("OptimalSoilMoisture", document.querySelector("#osh").value);
    formData.append("LowestNormalSoilMoisture", document.querySelector("#lnsh").value);
    formData.append("UpperNormalSoilMoisture", document.querySelector("#hnsh").value);
    formData.append("WeeklyRootWaterWinter", document.querySelector("#wwrw").value);
    formData.append("WeeklyRootWaterSummer", document.querySelector("#wwrs").value);
    for(const object of formData.entries()){
        console.log(object);
    }
    axios.post("https://localhost:44357/category", formData, {
        headers: {
            'Content-Type': 'multipart/form-data'
        }
    }).then(data=>{
        alert("Preset uploaded")
    }).catch(console.error)
    const title = document.querySelector("#title")
}