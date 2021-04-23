import Dictionary from "/scripts/modules/dictionary.js"
export default class GenericResultInterface {
    dictionary;
    contextHandler;
    link = ""
    resourceServer = "https://localhost:44374/"
    endpoint = "ad/"
    pageNumberString = "?PageNumber="
    pageSizeString = "&PageSize="
    category;
    filters = ""
    search = ""
    link = ""
    pageSize = 30;
    sortby = "&sortby="
    sortField = "idH"
    currentPageNumber = 1;
    lastPageNumber;
    allFilters = null;
    urlParams;
    constructor(intent) {
        this.urlParams = new URLSearchParams(window.location.search);
        switch (intent) {
            case 'search':
                this.handleSearch();
                break;
            case 'home':
                this.handleMain();
                break;
            case 'seller':
                this.handleSellers();
                break;
            case 'myads':
                break;
            case 'notifications':
                break;
            case 'wishlist':
        }
    }
    async getDictionary(cb) {
        let dict = new Dictionary();
        dict.init(this.category).then(maps => {
            this.dictionary = maps
            console.log(this.dictionary);
            cb(this.dictionary)
        })
    }
    setLink = (num) => {
        
        this.currentPageNumber = num;
        const pageSizeParam = this.pageSizeString + this.pageSize;
        const pageNumberParam = this.pageNumberString + this.currentPageNumber
        this.sortField=document.querySelector(".sorting").value
        this.link = this.resourceServer + this.endpoint + pageNumberParam + pageSizeParam + this.filters + this.search + this.sortby + this.sortField
        axios.get(this.link)
            .then((response) => response.data)
            .then((data) => {
                this.contextHandler(data)
                //this.populateSearchArea(data)
            }).catch(console.log)
    }
    populateSearchArea = (data) => {
        document.querySelector(".hits").innerHTML="Total results: "+data.totalAds
        this.lastPageNumber = data['totalPages']
        document.querySelector(".contentContainer").innerHTML = '';
        let allAds = ""
        for (let object of data.result) {
            document.querySelector(".contentContainer").innerHTML +=
            `<ad-component title="${object.title}"
            customer-image="${object.profileimg}"
            customer-id="${object.customer}"
            customer-name="${object.username}"
            customer-rating="${object.rating}"
            customer-reviews="${object.reviews}"
            condition="${this.dictionary.con.get(object.condition)}"
            price="${object.price}"
            item-image="${object.img}"
            id="${object.id}"></ad-component>`
            // axios.get(`https://localhost:44374/customer/${object.customer}`)
            //     .then((response) => response.data)
            //     .then((customer) =>
            //         `<ad-component title="${object.title}"
            //     customer-image="${object.profileImg}"
            //     customer-id="${object.customer}"
            //     customer-name="${object.username}"
            //     customer-rating="${object.rating}"
            //     customer-reviews="${object.reviews}"
            //     condition="${this.dictionary.con.get(object.condition)}"
            //     price="${object.price}"
            //     item-image="${object.img}"
            //     id="${object.id}"></ad-component>`
            //     )
            //     .then(ads => document.querySelector(".contentContainer").innerHTML += ads)
            //     .catch(console.log)
        }
        const pagers = document.querySelectorAll('pagination-component')
        for (let pager of pagers) {
            console.log(this.currentPageNumber);
            pager.setAttribute("current-page", this.currentPageNumber)
            pager.setAttribute("last-page", data['totalPages'])
        }
    }
    setFilters = () => {
        this.filters = "";
        var regex1 = /[0-9]{1,2}/;
        this.allFilters = {
            subcategory: {
                group: document.getElementsByName("category"),
                strings: "subcategoryId=",
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
            this.endpoint = "ad/"
            this.filters = "&" + this.filters.substring(0, this.filters.length - 1)
        }
        this.currentPageNumber = 1;
        console.log(this.allFilters)

        this.setSearchQuery()
        this.setLink(1);

    }
    setSearchQuery = () => {
        if (document.querySelector("#searchBox").value) {
            this.search = "&title=" + (document.querySelector("#searchBox").value.toLowerCase().replace(/[^a-z0-9 ]/g, '').replace(/\s+/g, '+').replace(/[+]+$/, ""));

        } else if (this.urlParams.get("title") != "null") {
            //this.search = "&title="+this.urlParams.get("title");
        }
      
    }
    handleSearch = () => {
        this.endpoint = "ad/"
        const search = this.urlParams.get('title');
        const category = this.urlParams.get('category');
        if (category) {
            this.category = category;
        }
        if (search) {
            this.search = "&title=" + search;
            this.setSearchQuery();
            document.querySelector("#searchBox").value = this.urlParams.get("title")
        }
        this.contextHandler = this.populateSearchArea;
        const subcategory = this.urlParams.get('subcategory');
        console.log(this.category)
        if (!this.category) {
            window.location.href = "/home/categories/index.html"
        } else {
            if (!subcategory) {
                window.location.href = "/home/categories/index.html?category=" + this.category
            } else {
                this.subcategory = subcategory
                console.log(this.subcategory);
                document.querySelector("navbar-component").setAttribute("filters", "?category=" + this.category + "&subcategory=" + this.subcategory + this.search)

            }
        }
        console.log(category);
        this.getDictionary((maps) => {
            console.log(maps);
            maps.sub.forEach(this.fillCategories)
            maps.sta.forEach(this.fillState)
            maps.man.forEach(this.fillManufacturer)
            maps.typ.forEach(this.fillType)
            maps.con.forEach(this.fillCondition)
            document.querySelector("#cat" + this.subcategory).checked = true;
            this.setFilters()
        })
    }
    handleMain = () => {
        this.contextHandler = this.populateSearchArea;
        let dict = new Dictionary();
        this.getDictionary((maps) => {
            this.dictionary = maps;
            this.setLink(1);
        })
    }
    handleSellers = () => {
        let customerList = ""
        axios.get("https://localhost:44374/customer")
            .then(response => response.data)
            .then((data) => {
                for (let object of data.result) {
                    console.log(object);
                    customerList += `
                    <customer-component 
                        id="${object.id}" 
                        image="${object.profileImg}"
                        fname="${object.name}"
                        lname="${object.lastName}"
                        username="${object.username}"
                        rating="${object.rating}"
                        address="${object.address}"
                    ></customer-component>
                `

                }
                const pagers = document.querySelectorAll('pagination-component')
                for (let pager of pagers) {
                    console.log(this.currentPageNumber);
                    pager.setAttribute("current-page", this.currentPageNumber)
                    pager.setAttribute("last-page", data['totalPages'])
                }
                document.querySelector(".customerContent").innerHTML = customerList
            })

    }
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
    fillCategories = (object) => {
        const categories = document.querySelector(".categoryGroup")
        console.log(this.subcategory, object.id);
        if (this.subcategory == object.id) {
            categories.innerHTML += `<input type="radio" name="category" checked="true" id="cat${object.id}">
                             <label for="cat${object.id}">${object.title}</label><br>`
        } else {
            categories.innerHTML += `<input type="radio" name="category"  id="cat${object.id}">
            <label for="cat${object.id}">${object.title}</label><br>`
        }

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
class PaginatorController {
    constructor() {

    }
}