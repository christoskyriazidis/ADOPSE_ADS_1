export default class Dictionary {
    constructor() {
       
    }
    getCategoryDictionary=()=>{
        return axios.get("https://localhost:44374/category").then((response) => response.data).then(this.dictify)
    }
    getSubCategoryDictionary = (category) => {
        return axios.get("https://localhost:44374/category/"+category)
            .then((response) => response.data)
    }
    getTypeDictionary = () => {
        return axios.get("https://localhost:44374/type")
            .then((response) => response.data).then(this.dictify)
    }
    getManufacturerDictionary = () => {
        return axios.get("https://localhost:44374/manufacturer")
            .then((response) => response.data).then(this.dictify)
    }
    getStateDictionary = () => {
        return axios.get("https://localhost:44374/state")
            .then((response) => response.data).then(this.dictify)
    }
    getConditionDictionary = () => {
        return axios.get("https://localhost:44374/condition")
            .then((response) => response.data).then(this.dictify)
    }
    dictify = (array) => {
        const dictionary = new Map();
        for (let object of array) {
            dictionary.set(object.id, object.title);
        }
        return dictionary;

    }
    
    async init(category) {
        const [sub,cat,typ,man,sta,con] = await Promise.all([this.getSubCategoryDictionary(category),this.getCategoryDictionary(),this.getTypeDictionary(),this.getManufacturerDictionary(),this.getStateDictionary(),this.getConditionDictionary()]);
        return {sub,cat,typ,man,sta,con};
    }
      
}


