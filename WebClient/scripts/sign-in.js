var config = {
    userStore: new Oidc.WebStorageStateStore({ store: window.localStorage }),
    authority: "https://localhost:44305/",
    client_id: "client_id_js",
    response_type: "code",
    redirect_uri: "http://127.0.0.1:5501/home/signin.html",
    post_logout_redirect_uri: "http://127.0.0.1:5501/Home/Index.html",
    scope: "openid ApiOne credentials",
}
const btnSignIn = document.querySelector('#btn-signIn')
const btnSignOut = document.querySelector('#btn-signOut')

btnSignIn.addEventListener('click', signIn)
btnSignOut.addEventListener('click', signOut)
var userManager = new Oidc.UserManager(config);

userManager.getUser().then(user => {
    //console.log("user:",user);
    if (user) {
        console.log(user);
        //vazoume san default header to token, global fasi
        axios.defaults.headers.common["Authorization"] = "Bearer " + user.access_token;
        me = user;
        btnSignOut.style.display = "flex";
        username.innerHTML = me.profile.username;
    }
    else {
        btnSignIn.style.display = "flex"
    }
});
var refreshing = false;
axios.interceptors.response.use(
    function (response) { return response; },
    function (error) {
        console.log("axios error:", error.response);

        var axiosConfig = error.response.config;

        //if error response is 401 try to refresh token
        if (error.response.status === 401) {
            console.log("axios error 401");
            signIn();
            // if already refreshing don't make another request
            if (!refreshing) {
                console.log("starting token refresh");
                refreshing = true;

                // do the refresh
                return userManager.signinSilent().then(user => {
                    console.log("new user:", user);
                    //update the http request and client
                    axios.defaults.headers.common["Authorization"] = "Bearer " + user.access_token;
                    axiosConfig.headers["Authorization"] = "Bearer " + user.access_token;
                    //retry the http request
                    return axios(axiosConfig);
                });
            }
        }

        return Promise.reject(error);
    });
function callApi() {
    axios.get("https://localhost:44374/ad")
        .then(res => {
            console.log(res)
        })
        .catch(err => {


            if (err.status === 401) {
                console.error(err);
                signIn();
            }
        })
}
userManager.events.addUserLoaded(function () {
    alert("hello");
});

function signIn() {
    userManager.signinRedirect();
}
function signOut() {
    userManager.signoutRedirect();
}
