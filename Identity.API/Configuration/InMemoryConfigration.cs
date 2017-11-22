﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace Identity.API.Configuration
{
    public class InMemoryConfigration
    {
        public static IEnumerable<ApiResource> ApiResource()
        {
            return new[] {
                new ApiResource("socialnetwork","社交网络")
            };
        }

        public static IEnumerable<Client> Clients()
        {
            return new[] {
                new Client(){
                    ClientId="socialnetwork",
                    ClientSecrets=new[]{ new Secret("secret".Sha256())},
                    AllowedGrantTypes= GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    AllowedScopes=new []{ "socialnetwork" }
                }
            };
        }

        public static IEnumerable<TestUser> Users()
        {
            return new[] {
                new TestUser(){
                    SubjectId="1",
                    Username="justmine",
                    Password="justmine"
                }
            };
        }
    }
}
