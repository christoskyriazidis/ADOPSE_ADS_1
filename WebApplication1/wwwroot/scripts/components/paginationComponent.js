class PaginationComponent extends HTMLElement {
    static get observedAttributes() { return ['current-page', 'last-page', 'filter',]; }
    constructor() {
        super();
        console.log("ahsfhaaiowfhaowufhawoiufhwaoi")

        this.render();
    }
    attributeChangedCallback(name, oldValue, newValue) {
        this.render();
    }
    get currentPage() {
        return this.getAttribute("current-page")
    }
    get lastPage() {
        return this.getAttribute("last-page")
    }
    get filter() {
        return this.getAttribute("filter")
    }
    render() {
        let currentPage = Number.parseInt(this.currentPage);
        let lastPage = Number.parseInt(this.lastPage);
        let nextPageUrl = this.nextPageUrl;


        //    ` <li class="dots"><span>...</span></li>
        //     <li class="currentPage"><span>${currentPage}</span></li>
        //     <li class="dots"><span>...</span></li>`
        let middle;


        switch (currentPage) {
            case 1:

                middle = ``;
                middle += checkNext(currentPage, lastPage);
                break;
            case 2:
                if (lastPage > 3) {
                    middle = `
                   
                    <li class="currentPage"><span>${currentPage}</span></li>`;
                    middle += checkNext(currentPage, lastPage, nextPageUrl);
                    break;
                }
                if (lastPage == 3) {
                    middle = `
                    
                    <li class="currentPage"><span>${currentPage}</span></li>`;
                    middle += checkNext(currentPage, lastPage, nextPageUrl);
                    break;
                }


            case 3:

                if (lastPage == 3) {
                    middle = `
                    <li class="previousPage"  onclick="searchController.callPrevious()"><span>${currentPage - 1}</span></li>
                    `
                    middle += checkNext(currentPage, lastPage);
                } if (lastPage > 3) {
                    middle = `
                    <li class="previousPage"  onclick="searchController.callPrevious()"><span>${currentPage - 1}</span></li>
                    <li class="currentPage"><span>${currentPage}</span></li>`
                    middle += checkNext(currentPage, lastPage);
                }

                break;
            default:
                middle = `<li class="dots"><span>...</span></li>
                        <li class="previousPage"  onclick="searchController.callPrevious()"><span>${currentPage - 1}</span></li>
                        <li class="currentPage"><span>${currentPage}</span></li>`;
                middle += checkNext(currentPage, lastPage);
                break;
        }
        if (lastPage == currentPage) {
            if (lastPage > 4) {

                middle = `<li class="dots"><span>...</span></li>
                <li class="previousPage"  onclick="searchController.callPrevious()"><span >${currentPage - 1}</span></li>`;
            }
            if (lastPage == 3) {

                middle = `
                <li class="previousPage"  onclick="searchController.callPrevious()"><span >${currentPage - 1}</span></li>`;
            }

        }
        if (lastPage == 1) {
            this.innerHTML = `
            <div div class="paginationContainer" >
            <ul>
                <li class="firstPage ${currentPage == 1 ? " currentPage" : ""}" ><span  onclick="searchController.callFirst()">1</span></li>
            </ul >
        </div >
        <link rel="stylesheet" href="/styles/components/pagination/pagination.css">`
        }
        if (lastPage == 2) {
            this.innerHTML = `
                <div div class="paginationContainer" >
                    <ul>
                        <li class="goToPrevious"><span  onclick="searchController.callPrevious()">&lt;</span></li>
                        <li class="firstPage ${currentPage == 1 ? " currentPage" : ""}"><span  onclick="searchController.callFirst()">1</span></li>
                        <li class="lastPage ${currentPage == lastPage ? " currentPage" : ""}" > <span onclick="searchController.callLast()">${lastPage}</span></li >
                        <li class="goToNext""><span  onclick="searchController.callNext()">&gt;</span></li>
                    </ul >
                </div >
                <link rel="stylesheet" href="/styles/components/pagination/pagination.css">`
        }
        if (lastPage == 3) {
            this.innerHTML = `
            <div div class="paginationContainer" >
                <ul>
                    <li class="goToPrevious"><span onclick="searchController.callPrevious()">&lt;</span></li>
                    <li class="firstPage ${currentPage == 1 ? " currentPage" : ""}"><span onclick="searchController.callFirst()">1</span></li>
                    ${middle}
                    <li class="lastPage ${currentPage == lastPage ? " currentPage" : ""}" > <span  onclick="searchController.callLast()">${lastPage}</span></li >
                    <li class="goToNext"><span  onclick="searchController.callNext()">&gt;</span></li>
                </ul >
            </div >
            <link rel="stylesheet" href="/styles/components/pagination/pagination.css">`
        }
        if (lastPage > 3) {
            console.log("ahahah");
            this.innerHTML = `
            <div div class="paginationContainer" >
                <ul>
                    <li class="goToPrevious"><span onclick="searchController.callPrevious()">&lt;</span></li>
                    <li class="firstPage ${currentPage == 1 ? " currentPage" : ""}"><span onclick="searchController.callFirst()">1</span></li>
                    ${middle}
                    <li class="lastPage ${currentPage == lastPage ? " currentPage" : ""}" > <span  onclick="searchController.callLast()">${lastPage}</span></li >
                    <li class="goToNext"><span  onclick="searchController.callNext()">&gt;</span></li>
                </ul >
            </div >
            <link rel="stylesheet" href="/styles/components/pagination/pagination.css">`
        }

    }
}

const checkNext = (currentPage, lastPage) => {
    if (lastPage - currentPage >= 3) {
        return ` <li class="nextPage" ><span  onclick="searchController.callNext()">${currentPage + 1}</span></li>
                    <li class="dots"><span>...</span></li>`
    }
    if (lastPage - currentPage >= 2) {
        return `<li class="nextPage"><span  onclick="searchController.callNext()">${currentPage + 1}</span></li>`
    }
    return ``;
}
const checkLast = (currentPage) => {
    if (currentPage) {

    }
}


customElements.define("pagination-component", PaginationComponent) /* <li class="dots"><span>...</span></li>
        <li class="currentPageLeft"><span>4</span></li>
        <li class="currentPage"><span>5</span></li>
        <li class="currentPageRight"><span>6</span></li>
        <li class="dots"><span>...</span></li> */

