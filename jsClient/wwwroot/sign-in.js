const btnSignIn = document.querySelector('#btn-signIn')
const btnSignOut = document.querySelector('#btn-signOut')
const btnApi = document.querySelector('#btn-callApi')
const btnChat = document.querySelector('#btn-chat');

btnSignIn.addEventListener('click', signIn)
btnApi.addEventListener('click', callApi)
btnSignOut.addEventListener('click', signOut)
btnChat.addEventListener('click', () => window.location.href = '/home/chat')


var me = null;

var config = {
    userStore: new Oidc.WebStorageStateStore({ store: window.localStorage }),
    authority: "https://localhost:44305/",
    client_id: "client_id_js",
    response_type: "id_token token",
    redirect_uri: "https://localhost:44364/Home/SignIn",
    post_logout_redirect_uri:"https://localhost:44364/Home/Index",
    scope: "openid ApiOne credentials",
}

var userManager = new Oidc.UserManager(config);

function signIn() {
    userManager.signinRedirect();
}
function signOut() {
    userManager.signoutRedirect();
}

userManager.getUser().then(user=>{
    console.log("user:",user);
    if (user) {
        //vazoume san default header to token, global fasi
        axios.defaults.headers.common["Authorization"] = "Bearer " + user.access_token;
        me = user;
    }
});


function callApi(){
    axios.get("https://localhost:44374/secret")
    .then(res => {
        console.log(res)
    })
     .catch(err => {
            alert(err)
        console.error(err); 
    })
}
function callMalaka() {
    axios.get("https://localhost:44374/malakas")
    .then(res => {
        console.log(res)
    })
    .catch(err => {
        alert(err)
        console.error(err); 
    })
}


//epeidh exoume global token header dn xroiazete na to valoume k se auto to request/response
var refreshing = false;

axios.interceptors.response.use(
    function (response) { return response; },
    function (error) {
        console.log("axios error:", error.response);

        var axiosConfig = error.response.config;

        //if error response is 401 try to refresh token
        if (error.response.status === 401) {
            console.log("axios error 401");

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










////functions for generate this things
//var createState = function () {
//    return "SessionValueMakeItABitLongerasdfhjsadoighasdifjdsalkhrfakwelyrosdpiufghasidkgewr";
//};

//var createNonce = function () {
//    return "NonceValuedsafliudsayatroiewewryie123";
//};

//function signIn() {
//    var redirectUri = "https://localhost:44364/Home/SignIn";
//    var responseType = "id_token token";
//    var scope = "openid ApiOne";
//    var authUrl =
//        "/connect/authorize/callback" +
//        "?client_id=client_id_js" +
//        "&redirect_uri=" + encodeURIComponent(redirectUri) +
//        "&response_type=" + encodeURIComponent(responseType) +
//        "&scope=" + encodeURIComponent(scope) +
//        "&nonce=" + createNonce() +
//        "&state=" + createState();
//    var returnUrl = encodeURIComponent(authUrl);

//    window.location.href = "https://localhost:44305/Auth/Login?ReturnUrl=" + returnUrl;

//}


