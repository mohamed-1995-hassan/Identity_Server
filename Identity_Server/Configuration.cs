using IdentityModel;
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
        public static IEnumerable<ApiResource> GetApis() => new List<ApiResource>
        {
            new ApiResource("ApiOne", new string[]
            {
                "Api.gramma",
            }),
            new ApiResource("ApiTwo")
        };
        public static IEnumerable<Client> GetClients() => new List<Client>
        {
            new Client
            {
                ClientId = "client_id",
                ClientSecrets = {new Secret("client_secret".ToSha256())},
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AllowedScopes = { "ApiOne" }
            },
            new Client
            {
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
                ClientId = "client_id_js",
                AllowedGrantTypes = GrantTypes.Implicit,
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
            }
        };
    }
}
