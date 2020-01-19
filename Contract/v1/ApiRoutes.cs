﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalog.Contract.v1
{
    public static class ApiRoutes
    {
        public const string Version = "v1";

        public const string Root = "api";

        public const string Base = Root + "/" + Version;


        public class Posts { 

        public const string GetAll = Base + "/posts";
        public const string Get = Base + "/posts/{postId}";
        public const string Update = Base + "/posts/{postId}";
        public const string Delete = Base + "/posts/{postId}";
        public const string Create = Base + "/posts";
            // public static readonly string Get = "api/v1/posts/{postId}";
        }
    }
}
