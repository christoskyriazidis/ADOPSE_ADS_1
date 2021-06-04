
class WeatherComponent extends HTMLElement {
    constructor() {
        super();
        this.callWeatherApi();
    }
    static get observedAttributes() { return ['fieldId']; }
    attributeChangedCallback(name, oldValue, newValue) {
        console.log("hi")
       
    }
    days=[
        "Sunday",
        "Monday",
        "Tuesday",
        "Wednesday",
        "Thursday",
        "Friday",
        "Saturday"
    ]
    get fieldId(){
        return this.getAttribute("fieldId")
    }
    callWeatherApi=()=> {
        console.log(this.fieldId)
        axios.get(`https://localhost:44357/xorafi/weekly?xorafiId=${this.fieldId}&pageNumber=1`).then(res => res.data)
            .then(data => {
                let days=""
                data=data.reverse();
                for(let object of data){
                    days+=`
                <div class="widget ${(new Date(Date.now())).getDay()!=(new Date(object.timestamp*1000)).getDay()?"todayDay":""}todayDay">
                    <h5 class="day">${this.days[(new Date(object.timestamp*1000)).getDay()]}</h5>
                    <div class="center">
                        
                        <div class="left">
                            <img src="/images/${object.icon.substring(0,2)}.svg" class="icon">
                            <h5 class="weather-status">${object.description}</h5>
                        </div>
                        <div class="right">
                            
                            <h5 class="degree">${Math.floor(object.minTemp)}&#176;c - ${Math.floor(object.maxTemp)}&#176;c</h5>
                        </div>
                    </div>

                    <div class="bottom">
                        <div>
                            Wind Speed <span>${object.wind_speed} kmph</span>
                        </div>
                        <div>
                            Humidity <span>${object.humidity}%</span>
                        </div>
                        <div>
                            Pressure <span>${object.pressure} mb</span>
                        </div>
                    </div>
                </div>`
                }
                return days;
            }
            ).then(this.render)
    }
    render=(days)=> {
        console.log(this.innerHTML)
        this.innerHTML = `
        <div class="weatherWrapper">${days}</div>
        `
    }
}

customElements.define("weather-component", WeatherComponent);
