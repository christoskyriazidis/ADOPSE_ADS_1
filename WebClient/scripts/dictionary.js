class Dictionary {
    constructor() {
       
    }
    getCategoryDictionary = () => {
        return axios.get("https://localhost:44374/category")
            .then((response) => response.data).then(this.dictify)
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
    
    async init() {
        const [cat,typ,man,sta,con] = await Promise.all([this.getCategoryDictionary(),this.getTypeDictionary(),this.getManufacturerDictionary(),this.getStateDictionary(),this.getConditionDictionary()]);
        return {cat,typ,man,sta,con};
    }
      
}


