using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;

namespace Identity_Server
{
    public static class Configuration
    {
        public static IEnumerable<IdentityResource> GetIdentityResource() =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                //new IdentityResources.Profile(),
                new IdentityResource
                {
                    Name = "rc.scope",
                    UserClaims =
                    {
                        "gramma"
                    }
                }
            };
        public static IEnumerable<ApiScope> GetApiScopes(){
            return new List<ApiScope>
            {
                new ApiScope("ApiOne", "ApiOne"),
                new ApiScope("res1", "res1"),
                new ApiScope("res2", "res2"),
            };
         }

        public static IEnumerable<ApiResource> GetApis() => new List<ApiResource>
        {
            new ApiResource("ApiOne", new string[] { "Api.gramma" })
            {
                ApiSecrets = {new Secret("client_secret".ToSha256())},
                Scopes = { "ApiOne" },
            },
            new ApiResource("ApiTwo"),
            new ApiResource("res1", "res2")
            {
                Scopes = 
                {
                    "client_scope", "ApiOne", "profile", "openid"
                }
            }
        };
        public static IEnumerable<Client> GetClients() => new List<Client>
        {
            new Client
            {
                Enabled = true,
                ClientId = "client_id",
                ClientSecrets = {new Secret("client_secret".ToSha256())},
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AllowedScopes = { "ApiOne", "profile", "openid" },
                //FrontChannelLogoutUri = "https://localhost:7077/signout-oidc",
                //PostLogoutRedirectUris = new List<string>() { "https://localhost:7077/signout-callback-oidc" },
                AccessTokenLifetime = 1200, //20 Minutes
                RequirePkce = true,
                AllowPlainTextPkce = false,
                AllowOfflineAccess = false,
                RequireConsent = false,
                //RedirectUris = { "http://localhost:4200" },
            },
            new Client
            {
                Enabled = false,
                ClientId = "client_id_mvc",
                ClientSecrets = {new Secret("client_secret_mvc".ToSha256())},
                AllowedGrantTypes = GrantTypes.Code,
                AllowedScopes =
                {
                    "ApiOne" ,
                    "ApiTwo",
                    IdentityServer4
                    .IdentityServerConstants
                    .StandardScopes
                    .OpenId,    
                    //IdentityServer4
                    //.IdentityServerConstants
                    //.StandardScopes
                    //.Profile,
                    "rc.scope"
                },
                RedirectUris = { "http://localhost:5131/signin-oidc" },

                //put all claims in id token
                //AlwaysIncludeUserClaimsInIdToken = true,
                RequireConsent = false
            },
            new Client
            {
                Enabled = false,
                ClientId = "client_id_js",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                RedirectUris = { "https://localhost:7276/signin" },
                AllowedScopes =
                {
                    IdentityServer4
                    .IdentityServerConstants
                    .StandardScopes
                    .OpenId,
                    "ApiOne"
                },
                AllowAccessTokensViaBrowser = true
            },
            new Client
            {
                Enabled = false,
                ClientId = "angular_spa2",
                ClientSecrets = { new Secret("client_secret".Sha256()) },
                AllowedGrantTypes = GrantTypes.Code,
                RequirePkce = true,
                RedirectUris = { "http://localhost:4200" },
                PostLogoutRedirectUris = { "http://localhost:4200/" },
                AllowedScopes = { "openid" ,"client_scope" },
                AllowOfflineAccess = true,
                RequireConsent = false,
                RequireClientSecret = false,
                AlwaysIncludeUserClaimsInIdToken = true,
            }
        };
    }
}
