class WeatherComponent extends HTMLElement {
    constructor() {
        super();
        this.callWeatherApi();
    }
    callWeatherApi() {
        axios.get().then(res => res.data)
            .then(data => {
                let days=""
                for(object of data){
                    days+=`
                <div class="widget">
                    <div class="center">
                        <div class="left">
                            <img src="/images/${object.description}.svg" class="icon">
                            <h5 class="weather-status">${object.description}</h5>
                        </div>
                        <div class="right">
                            <h5 class="city">${object.timezone}</h5>
                            <h5 class="degree">${object.temperature}&#176;c</h5>
                        </div>
                    </div>

                    <div class="bottom">
                        <div>
                            Wind Speed <span>${object.windSpeed} kmph</span>
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
    render(days) {
       
        this.innerHTML = `
        <div class="weatherWrapper">${days}</div>
        `
    }
}
customElements.define("weather-component", WeatherComponent);
