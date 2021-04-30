using IdentityModel.Client;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace WpfClientt.services {
    public class OpenIdConnectClient {

        private HttpClient client;
        private string state;
        private string code_verifier;
        private DiscoveryDocumentResponse discovery;

        internal OpenIdConnectClient(HttpClient client, DiscoveryDocumentResponse discovery) {
            this.client = client;
            this.discovery = discovery;
        }


        public async Task<string> PrepareAuthorizationRequestUrl() {
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            state = randomDataBase64url(32);
            code_verifier = randomDataBase64url(32);
            parameters.Add("state", state);
            parameters.Add("redirect_uri", "http://localhost/sample-wpf-app");
            parameters.Add("code_challenge", base64urlencodeNoPadding(sha256(code_verifier)));
            parameters.Add("code_challenge_method", "S256");
            parameters.Add("scope", "ApiOne openid");
            parameters.Add("response_type", "code");
            parameters.Add("client_id", "wpf");

            return CreateAuthorizationURL(discovery.AuthorizeEndpoint,parameters);
        }

        public async Task RetrieveAndSetAccessToken(String redirectUrl) {
            NameValueCollection queryValues = HttpUtility.ParseQueryString(redirectUrl.Substring( redirectUrl.IndexOf("?") + 1 ));
            if (!queryValues.Get("state").Equals(state)) {
                throw new ApplicationException("The state sent to the authorization server does not match.");
            }

            AuthorizationCodeTokenRequest tokenRequest = new AuthorizationCodeTokenRequest() {
                Address = discovery.TokenEndpoint,
                Code = queryValues.Get("code"),
                CodeVerifier = code_verifier,
                ClientId = "wpf",
                RedirectUri = "http://localhost/sample-wpf-app",
                Method = HttpMethod.Post,
                GrantType = "code"
            };

            TokenResponse tokenResponse = await client.RequestAuthorizationCodeTokenAsync(tokenRequest);

            if (tokenResponse.IsError) {
                throw new ApplicationException($"Error occured during code exhange.Error:{tokenResponse.Error}" );
            }

            client.SetBearerToken(tokenResponse.AccessToken);
        }

        public async Task<UserInfoResponse> GetUserInfo() {
            UserInfoResponse response = await client.GetUserInfoAsync(new UserInfoRequest() {
                Address = discovery.UserInfoEndpoint,
                Token = client.DefaultRequestHeaders.Authorization.ToString().Replace("Bearer ", "")
            }) ;

            return response;
        }

        private string CreateAuthorizationURL(string authorizationEndPoint, IDictionary<string, string> parameters) {
            if (parameters.Count == 0) {
                return authorizationEndPoint;
            }
            StringBuilder builder = new StringBuilder(authorizationEndPoint);
            builder.Append($"?{parameters.First().Key}={parameters.First().Value}");
            parameters.Remove(parameters.First().Key);
            foreach (KeyValuePair<string, string> parameter in parameters) {
                builder.Append($"&{parameter.Key}={parameter.Value}");
            }
            return builder.ToString();
        }

        private byte[] sha256(string code_verifier) {
            byte[] bytes = Encoding.ASCII.GetBytes(code_verifier);
            SHA256Managed sha256 = new SHA256Managed();
            return sha256.ComputeHash(bytes);
        }

        private string base64urlencodeNoPadding(byte[] buffer) {
            string base64 = Convert.ToBase64String(buffer);

            // Converts base64 to base64url.
            base64 = base64.Replace("+", "-");
            base64 = base64.Replace("/", "_");
            // Strips padding.
            base64 = base64.Replace("=", "");

            return base64;
        }

        private string randomDataBase64url(uint length) {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] bytes = new byte[length];
            rng.GetBytes(bytes);
            return base64urlencodeNoPadding(bytes);
        }

    }
}
