// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;

namespace IdentityServerV4
{
    public static class Config
    {
        public static IEnumerable<ApiResource> ApiResources => new ApiResource[]
        {
            new ApiResource("resource_catalog"){Scopes = {"catalog_fullpermission"}},
            new ApiResource("resource_file_stock"){Scopes = {"file_stock_fullpermission"}},
            new ApiResource("resource_basket"){Scopes = {"basket_fullpermission"}},
            new ApiResource("resource_discount"){Scopes = {"discount_fullpermission", "discount_read_permission", "discount_write_permission"}},
            new ApiResource("resource_order"){Scopes = {"order_fullpermission"}},
            new ApiResource(IdentityServerConstants.LocalApi.ScopeName),

        };

        public static IEnumerable<IdentityResource> IdentityResources =>
                   new IdentityResource[]
                   {
                       new IdentityResources.Email(),
                       new IdentityResources.OpenId(), //Must
                       new IdentityResources.Profile(),
                       new IdentityResource
                       {
                           Name = "roles",
                           DisplayName = "Roles",
                           Description = "User roles",
                           UserClaims = new[] { "role" }
                       }
                   };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("catalog_fullpermission", "Full access to Catalog API"),
                new ApiScope("file_stock_fullpermission", "Full access to Photo Stock API"),
                new ApiScope("basket_fullpermission", "Full access to Basket API"),
                new ApiScope("discount_fullpermission", "Full access to Discount API"),
                new ApiScope("discount_read_permission", "Read access to Discount API"),
                new ApiScope("discount_write_permission", "Write access to Discount API"),
                new ApiScope("order_fullpermission", "Write access to Order API"),
                new ApiScope(IdentityServerConstants.LocalApi.ScopeName, "Full access to IS4"),

            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientName = "Asp.Net Core MVC",
                    ClientId = "WebMvcClient",
                    ClientSecrets = {new Secret("secret".Sha256())},
                    AllowedGrantTypes = GrantTypes.ClientCredentials, // it has no refresh-token
                    AllowedScopes = {"catalog_fullpermission", "file_stock_fullpermission", IdentityServerConstants.LocalApi.ScopeName}
                },
                new Client
                {
                    ClientName = "Asp.Net Core MVC",
                    ClientId = "WebMvcClientForUser",
                    AllowOfflineAccess = true,
                    ClientSecrets = {new Secret("secret".Sha256())},
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword, // it generated refresh-token
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.OfflineAccess, // even if the user has no internet, we can obtain a new token using refresh-token, you really wanna allow offline access? :) think twice more
                        IdentityServerConstants.LocalApi.ScopeName,
                        "roles",
                        "basket_fullpermission",
                        "discount_read_permission",
                        "order_fullpermission",
                    },
                    AccessTokenLifetime = 1 * 60 * 60,
                    RefreshTokenExpiration = TokenExpiration.Absolute,
                    AbsoluteRefreshTokenLifetime = (int) (DateTime.Now.AddDays(60) - DateTime.Now).TotalSeconds,
                    RefreshTokenUsage = TokenUsage.ReUse,
                }
            };
    }
}