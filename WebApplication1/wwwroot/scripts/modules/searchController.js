import Dictionary from "/scripts/modules/dictionary.js"
export default class SearchController {
    link = ""
    resourceServer = "https://localhost:44374/"
    endpoint = "ad/"
    pageNumberString = "?PageNumber="
    pageSizeString = "&PageSize="
    filters = ""
    search = ""
    link = ""
    pageSize = 5;
    currentPageNumber = 1;
    lastPageNumber;
    dictMaps;
    allFilters = null;
    constructor() {
        let dict = new Dictionary();
        dict.init().then(maps => {
            maps.cat.forEach(this.fillCategories)
            maps.sta.forEach(this.fillState)
            maps.man.forEach(this.fillManufacturer)
            maps.typ.forEach(this.fillType)
            maps.con.forEach(this.fillCondition)
            console.log(maps);
            this.dictMaps = maps
        }).then(() => {
            this.setLink(1);
        })

    }

    setLink = (num) => {
        this.currentPageNumber = num;
        const pageSizeParam = this.pageSizeString + this.pageSize;
        const pageNumberParam = this.pageNumberString + this.currentPageNumber
        this.link = this.resourceServer + this.endpoint + pageNumberParam + pageSizeParam + this.filters + this.search

        this.callCurrentLink()
    }

    setFilters = () => {
        this.filters = "";
        var regex1 = /[0-9]{1,2}/;

        this.allFilters = {
            category: {
                group: document.getElementsByName("category"),
                strings: "category=",
            },
            manufacturer: {
                group: document.getElementsByName("manufacturer"),
                strings: "manufacturer=",
            },
            type: {
                group: document.getElementsByName("type"),
                strings: "type=",
            },
            condition: {
                group: document.getElementsByName("condition"),
                strings: "condition=",
            },
            state: {
                group: document.getElementsByName("state"),
                strings: "state=",
            },
        }
        //deep copy

        let originalAllFilters = JSON.parse(JSON.stringify(this.allFilters))
        for (let filter in this.allFilters) {
            for (let option of this.allFilters[filter].group) {
                if (option.checked) {
                    this.allFilters[filter].strings += `${option.id.match(regex1)}_`;
                }
            }
            if (this.allFilters[filter].strings == originalAllFilters[filter].strings) {
                this.allFilters[filter].strings = "";
            } else {
                console.log(this.filters)
                this.filters += this.allFilters[filter].strings.substring(0, this.allFilters[filter].strings.length - 1) + "&"
            }
        }
        console.log(this.filters)
        if (this.filters == "") {
            this.endpoint = "ad/"

        } else {
            this.endpoint = "filter/"
            this.filters = "&" + this.filters.substring(0, this.filters.length - 1)
        }
        this.currentPageNumber = 1;
        console.log(this.allFilters)

        this.setLink(1);

    }

    callCurrentLink = () => axios.get(this.link).then((response) => response.data).then((data) => {
        console.log(this.link, data.totalPages)
        this.currentPageNumber;
        this.populateContentArea(data)


    }).catch(console.log)
    callNext() {
        if (this.lastPageNumber > this.currentPageNumber) {

            this.setLink(++this.currentPageNumber);
        }
    }
    callPrevious() {
        if (this.currentPageNumber > 1) {
            this.setLink(--this.currentPageNumber);
        }
    }
    callLast() {
        this.lastPageNumber;
        this.setLink(this.lastPageNumber);
    }
    callFirst() {
        this.setLink(1);
    }
    set link(something) { this.link = something; console.log(something) }
    populateContentArea = (data) => {
        
        this.lastPageNumber = data['totalPages']
        document.querySelector(".contentContainer").innerHTML = '';
        let allAds = ""
        for (let object of data.result) {
            
            axios.get(`https://localhost:44374/customer/${object.customer}`)
                .then((response) => response.data)
                .then((customer) =>
                    `<ad-component title="${object.title}"
                    customer-image="${customer.profileImg}"
                    customer-name="${customer.username}"
                    customer-rating="${customer.rating}"
                    customer-id="${object.customer}"
                    condition="${this.dictMaps.con.get(object.condition)}"
                    price="${object.price}"
                    item-image="${object.img}"
                    id="${object.id}"></ad-component>`
                )
                .then(ads => document.querySelector(".contentContainer").innerHTML += ads)
                .catch(console.log)
        }

        const pagers = document.querySelectorAll('pagination-component')



        for (let pager of pagers) {
            console.log(this.currentPageNumber);
            pager.setAttribute("current-page", this.currentPageNumber)
            pager.setAttribute("last-page", data['totalPages'])
        }




    }

    fillCategories = (value, id) => {
        const categories = document.querySelector(".categoryGroup")
        categories.innerHTML += `<input type="radio" name="category"   id="cat${id}">
                             <label for="cat${id}">${value}</label><br>`
    }
    fillType = (value, id) => {
        const types = document.querySelector(".typeGroup")
        types.innerHTML += `<input type="checkbox" name="type" id="typ${id}">
                        <label for="typ${id}">${value}</label><br>`
    }

    fillManufacturer = (value, id) => {
        const manufacturers = document.querySelector(".manufacturerGroup")
        manufacturers.innerHTML += `<input type="checkbox" name="manufacturer" id="man${id}">
                                 <label for="man${id}">${value}</label><br>`
    }
    fillCondition = (value, id) => {

        const conditions = document.querySelector(".conditionGroup")
        conditions.innerHTML += `<input type="checkbox" name="condition" id="con${id}">
                              <label for="con${id}">${value}</label><br>`
    }
    fillState = (value, id) => {
        console.log(value);
        const states = document.querySelector(".stateGroup")
        states.innerHTML += `<input type="checkbox" name="state" id="sta${id}">
                         <label for="sta${id}">${value}</label><br>`
    }
}