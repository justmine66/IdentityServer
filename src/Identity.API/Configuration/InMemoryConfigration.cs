using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Test;
using IdentityServer4;
using IdentityModel;
using System.Security.Claims;

namespace Identity.API.Configuration
{
    public class InMemoryConfigration
    {
        public static IEnumerable<ApiResource> ApiResource()
        {
            return new[] {
                new ApiResource("socialnetwork.api","社交网络"){ UserClaims=new []{ "email"} }
            };
        }

        public static IEnumerable<Client> Clients()
        {
            return new[] {
                new Client(){
                    ClientId="socialnetwork",
                    ClientSecrets=new[]{ new Secret("secret".Sha256())},
                    AllowedGrantTypes= GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    AllowedScopes=new []{ "socialnetwork.api" }
                },
                new Client(){
                    ClientId="web.client",
                    ClientSecrets=new[]{ new Secret("secret".Sha256())},
                    AllowedGrantTypes= GrantTypes.Implicit,
                    AllowedScopes=new []{
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "socialnetwork.api"
                    },
                    RedirectUris={ "http://localhost:5003/signin-oidc" },
                    PostLogoutRedirectUris={ "http://localhost:5003/signout-callback-oidc" },
                    AllowAccessTokensViaBrowser=true
                },
                new Client
                {
                    ClientId = "web.code.client",
                    ClientName = "web Code Client",
                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    RedirectUris = { "http://localhost:5003/signin-oidc" },
                    PostLogoutRedirectUris = { "http://localhost:5003/signout-callback-oidc" },
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "socialnetwork.api"
                    },
                    AllowOfflineAccess = true,
                    AllowAccessTokensViaBrowser = true
                }
            };
        }

        public static IEnumerable<TestUser> Users()
        {
            return new[] {
                new TestUser(){
                    SubjectId="1",
                    Username="justmine",
                    Password="password",
                    Claims = new [] {
                        new Claim("email", "justmine@qq.com"),
                        new Claim("name", "justmine")
                    }
                }
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email()
            };
        }
    }
}
