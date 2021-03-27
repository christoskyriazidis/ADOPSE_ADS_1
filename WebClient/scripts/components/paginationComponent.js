class PaginationComponent extends HTMLElement {
    static get observedAttributes() { return ['current-page', 'last-page']; }
    constructor() {
        super();
        this.render();
        console.log("hello");
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
    render() {
        const currentPage = Number.parseInt(this.currentPage);
        const lastPage = Number.parseInt(this.lastPage);
        let middle;
        if (currentPage == 1) {
            if (lastPage > 4)
                middle = `
                <li class="currentPageRight"><a href="">${currentPage + 1}</a></li>
                <li class="dots"><span>...</span></li>
                `
            else {
                middle = switchLast(lastPage);
            }

        } if (currentPage == 2) {


            middle = `
                <li class="currentPage"><a href="">${currentPage}</a></li>
                <li class="currentPageRight"><a href="">${currentPage + 1}</a></li>
                <li class="dots"><span>...</span></li>
                `



        } if (currentPage == 3) {

            middle = `
            <li class="currentPageLeft"><a href="">${currentPage - 1}</a></li>
            <li class="currentPage"><a href="">${currentPage}</a></li>
            <li class="currentPageRight"><a href="">${currentPage + 1}</a></li>
            <li class="dots"><span>...</span></li>
            `

        } if (currentPage >= 4 && currentPage < lastPage - 3) {

            middle = `
            <li class="dots"><span>...</span></li>
            <li class="currentPageLeft"><a href="">${currentPage - 1}</a></li>
            <li class="currentPage"><a href="">${currentPage}</a></li>
            <li class="currentPageRight"><a href="">${currentPage + 1}</a></li>
            <li class="dots"><span>...</span></li>
            `
        } if (currentPage >= 4 && lastPage - currentPage == 2) {

            middle = `
            <li class="dots"><span>...</span></li>
            <li class="currentPageLeft"><a href="">${currentPage - 1}</a></li>
            <li class="currentPage"><a href="">${currentPage}</a></li>
            <li class="currentPageRight"><a href="">${currentPage + 1}</a></li>
            `
        } if (currentPage >= 4 && lastPage - currentPage == 1) {

            middle = `
            <li class="dots"><span>...</span></li>
            <li class="currentPageLeft"><a href="">${currentPage - 1}</a></li>
            <li class="currentPage"><a href="">${currentPage}</a></li>
            `
        } if (currentPage >= 4 && lastPage - currentPage == 0) {

            middle = `
            <li class="dots"><span>...</span></li>
            <li class="currentPageLeft"><a href="">${currentPage - 1}</a></li>
            `
        }
        this.innerHTML = `
        <div class="paginationContainer" current-page="" last-page="">
            <ul>
                <li class="goToPrevious"><a href="">&lt;</a></li>
                <li class="firstPage"><a href="">1</a></li>
                ${middle}
                <li class="lastPage"><a href="">${lastPage}</a></li>
                <li class="goToNext"><a href="">&gt;</a></li>
            </ul>
        </div>
        <link rel="stylesheet" href="/styles/components/pagination/pagination.css">`
    }
}
const switchLast = (lastPage) => {
    switch (lastPage) {
        case 2:
            return ``;
        case 3:
            return `<li class="currentPageRight"><a href="">${currentPage + 1}</a></li>`
        case 4:
            return `<li class="currentPage"><a href="">${currentPage}</a></li>
                    <li class="currentPageRight"><a href="">${currentPage + 1}</a></li>
                    <li class="dots"><span>...</span></li>`
        case 5:
            return `<li class="currentPageLeft"><a href="">${currentPage - 1}</a></li>
            <li class="currentPage"><a href="">${currentPage}</a></li>
            <li class="currentPageRight"><a href="">${currentPage + 1}</a></li>`
    }
}
customElements.define("pagination-component", PaginationComponent)
{/* <li class="dots"><span>...</span></li>
        <li class="currentPageLeft"><a href="">4</a></li>
        <li class="currentPage"><a href="">5</a></li>
        <li class="currentPageRight"><a href="">6</a></li>
        <li class="dots"><span>...</span></li> */}

