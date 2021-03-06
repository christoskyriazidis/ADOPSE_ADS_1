using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace identityServerNew
{
    public static class Configuration
    {
        public static IEnumerable<IdentityResource> GetIdentityResources() =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource
                {
                    Name="credentials",
                    UserClaims =
                    {
                        "username",ClaimTypes.Email,"Mobile","role",ClaimTypes.Role,ClaimTypes.NameIdentifier,"lastName",ClaimTypes.StreetAddress,"name","lastName"
                    }
                }
            };

        //gia api identify
        public static IEnumerable<ApiResource> GetApis() =>
            //kai kala kanoume register ena api pou exei onoma apione google
            //edw vazoume claims gia to access token..
            new List<ApiResource> {
                new ApiResource("ApiOne","mpourdela", new string[]{"username",ClaimTypes.Email,"Mobile","role",ClaimTypes.Role,ClaimTypes.NameIdentifier,"lastName",ClaimTypes.StreetAddress,"name","lastName"}) ,
                new ApiResource("ApiTwo") ,
                new ApiResource("ApiDypa","DypaApi",new string[]{"username",ClaimTypes.Email,"Mobile","role",ClaimTypes.Role,ClaimTypes.NameIdentifier,"lastName",ClaimTypes.StreetAddress,"name","lastName"}) 
            };

        public static IEnumerable<Client> GetClients() =>
            new List<Client>
            {
                new Client
                {
                    ClientId="client_id",
                    ClientSecrets={new Secret("client_secret".ToSha256())},
                    //ClientCredentials flow google
                    AllowedGrantTypes=GrantTypes.ClientCredentials,
                    //exei access mono sto api one
                    AllowedScopes={ "ApiOne"}
                },
                new Client
                {
                    ClientId="client_id_mvc",
                    ClientSecrets={new Secret("client_secret_mvc".ToSha256())},
                    //ClientCredentials flow google
                    AllowedGrantTypes=GrantTypes.Code,
                    //exei acces mono sto api 2
                    RedirectUris ={ "https://localhost:44389/signin-oidc" },
                    AllowedScopes={ 
                        "ApiOne",
                        "ApiTwo",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "credentials"
                    },
                    RequireConsent=false,
                    //vazei ola ta claims sto id token.. AlwaysIncludeUserClaimsInIdToken=true
                    AlwaysIncludeUserClaimsInIdToken=true,
                    AllowOfflineAccess=true
                },
                new Client
                {
                    ClientId="client_id_js",

                    AllowedGrantTypes=GrantTypes.Code,
                    RequirePkce=true,
                    //automato
                    RequireClientSecret=false,
                    RedirectUris ={"https://localhost:44366/home/signin.html"},
                    PostLogoutRedirectUris ={"https://localhost:44366/Home/Index.html"},
                    
                    AllowedCorsOrigins={"https://localhost:44366"},
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        "ApiOne",
                        "credentials"
                    },
                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,
                },
                new Client {
                    ClientId = "wpf",

                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,

                    RedirectUris = { "http://localhost/sample-wpf-app" },
                    AllowedCorsOrigins = { "http://localhost" },

                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        "credentials",
                        "ApiOne"

                    },
                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,
                },
                new Client
                {
                    ClientId="client_id_js_Dypa",
                    AllowedGrantTypes=GrantTypes.Code,
                    RequirePkce=true,

                    RequireClientSecret=false,
                    RedirectUris ={"https://localhost:44376/home/signin.html"},
                    PostLogoutRedirectUris ={"https://localhost:44376/Home/Index.html"},

                    AllowedCorsOrigins={"https://localhost:44376"},
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        "ApiDypa",
                        "credentials"
                    },
                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,
                }
            };
    }
}
