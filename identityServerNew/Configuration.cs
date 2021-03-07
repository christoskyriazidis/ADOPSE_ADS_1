using IdentityModel;
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
                        "username","email","sta8ero","kinito","role",ClaimTypes.DateOfBirth,ClaimTypes.Role
                    }
                }
                //new IdentityResources.Email
            };

        //gia api identify
        public static IEnumerable<ApiResource> GetApis() =>
            //kai kala kanoume register ena api pou exei onoma apione google
            //edw vazoume claims gia to access token..
            new List<ApiResource> {
                new ApiResource("ApiOne","mpourdela", new string[]{"username","role",ClaimTypes.DateOfBirth,ClaimTypes.Role}) ,
                new ApiResource("ApiTwo") ,
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
                        IdentityServer4.IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServer4.IdentityServerConstants.StandardScopes.Profile,
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

                    AllowedGrantTypes=GrantTypes.Implicit,

                    RedirectUris ={"https://localhost:44364/home/signin"},
                    PostLogoutRedirectUris ={"https://localhost:44364/Home/Index"},
                    
                    AllowedCorsOrigins={"https://localhost:44364"},

                    AllowedScopes =
                    {
                        IdentityServer4.IdentityServerConstants.StandardScopes.OpenId,
                        "ApiOne",
                        "credentials"
                    },

                    AccessTokenLifetime=1,
                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,
                }
            };
    }
}
