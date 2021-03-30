class PaginationComponent extends HTMLElement {
    static get observedAttributes() { return ['current-page', 'last-page', 'nextUrl', 'previousUrl','filter','current-url']; }
    constructor() {
        super();
        this.render();

    }
    attributeChangedCallback(name, oldValue, newValue) {
        switch (name) {
            case "current-page":
                this.render();
                break;
            case "filter":
                this.render();
                break;
            case "current-url":
                
                    this.render();
                
                break;
        }

    }
    get currentPage() {
        return this.getAttribute("current-page")
    }
    get lastPage() {
        return this.getAttribute("last-page")
    }

    get nextPageUrl() {
        return this.getAttribute("next-page")
    }
    get previousPageUrl() {
        return this.getAttribute("previous-page")
    }
    get lastPageUrl() {
        return this.getAttribute("last-page-url")
    }
    get filter() {
        return this.getAttribute("filter")
    }
    get currentUrl() {
        return this.getAttribute("current-url")
    }

    

    render() {
        console.log(this.currentPage);
        let currentPage = Number.parseInt(this.currentPage);
        let lastPage = Number.parseInt(this.lastPage);
        let previousPageUrl = this.previousPageUrl;
        let lastPageUrl = this.lastPageUrl;
        let nextPageUrl = this.nextPageUrl;
        let filter = this.filter;
        let firstUrl=this.currentUrl;

        if (filter) {
            lastPageUrl += filter;
            nextPageUrl += filter;
            previousPageUrl += filter;
            firstUrl += filter;
        }
        //console.log(currentPage,lastPage,previousPageUrl,lastPageUrl,nextPageUrl);
        //    ` <li class="dots"><span>...</span></li>
        //     <li class="currentPage"><span>${currentPage}</span></li>
        //     <li class="dots"><span>...</span></li>`
        let middle;

        console.log(currentPage == 4)
        switch (currentPage) {
            case 1:
                this.firstPageUrl = window.location.href;
                middle = ``;
                middle += checkNext(currentPage, lastPage, nextPageUrl);
                break;
            case 2:
                middle = `<li class="currentPage"><span>${currentPage}</span></li>`;
                middle += checkNext(currentPage, lastPage, nextPageUrl);
                break;
            case 3:
                middle = `<li class="previousPage" onclick="callPage('${previousPageUrl}')"><span>${currentPage - 1}</span></li>
                        <li class="currentPage"><span>${currentPage}</span></li>`;
                middle += checkNext(currentPage, lastPage, nextPageUrl);
                break;
            default:
                middle = `<li class="dots"><span>...</span></li>
                        <li class="previousPage" onclick="callPage('${previousPageUrl}')"><span>${currentPage - 1}</span></li>
                        <li class="currentPage"><span>${currentPage}</span></li>`;
                middle += checkNext(currentPage, lastPage, nextPageUrl);
                break;
        }
        if (lastPage == currentPage) {
            middle = `<li class="dots"><span>...</span></li>
                    <li class="previousPage"><span onclick="callPage('${previousPageUrl}')">${currentPage - 1}</span></li>`;
        }
        this.innerHTML = `
        <div class="paginationContainer">
            <ul>
                <li class="goToPrevious"><span onclick="callPage('${previousPageUrl}')">&lt;</span></li>
                <li class="firstPage ${currentPage == 1 ? "currentPage" : ""}"><span onclick="callPage('${firstUrl}')">1</span></li>
                ${middle}
                <li class="lastPage ${currentPage == lastPage ? "currentPage" : ""}"> <span onclick="callPage('${lastPageUrl}')">${lastPage}</span></li>
                <li class="goToNext"><span onclick="callPage('${nextPageUrl}')">&gt;</span></li>
            </ul>
        </div>
        <link rel="stylesheet" href="/styles/components/pagination/pagination.css">`
    }
}
const checkNext = (currentPage, lastPage, nextPageUrl) => {
    if (lastPage - currentPage >= 3) {
        return ` <li class="nextPage"><span onclick="callPage('${nextPageUrl}')">${currentPage + 1}</span></li>
                <li class="dots"><span>...</span></li>`
    }
    if (lastPage - currentPage >= 2) {
        return `<li class="nextPage"><span onclick="callPage('${nextPageUrl}')">${currentPage + 1}</span></li>`
    }
    return ``;
}
const checkLast = (currentPage) => {
    if (currentPage) {

    }
}


customElements.define("pagination-component", PaginationComponent)
{/* <li class="dots"><span>...</span></li>
        <li class="currentPageLeft"><span>4</span></li>
        <li class="currentPage"><span>5</span></li>
        <li class="currentPageRight"><span>6</span></li>
        <li class="dots"><span>...</span></li> */}

