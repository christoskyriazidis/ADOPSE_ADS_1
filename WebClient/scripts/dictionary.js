class Dictionary {
    construction() {

    }
    static getCategoryDictionary = () => {
        return axios.get("https://localhost:44374/category")
            .then((response) => response.data)
    }
}
