using System;
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

        public const string GetAll = Base + "/products";
        public const string Search = Base + "/products/search/{productName}";
        public const string Get = Base + "/products/{postId}";
        public const string Update = Base + "/products/{postId}";
        public const string Delete = Base + "/products/{postId}";
        public const string Create = Base + "/products";
            // public static readonly string Get = "api/v1/posts/{postId}";
        }
    }
}
