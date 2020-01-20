using ProductCatalog.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalog.Services
{
    public interface IProductService
    {
        List<Product> GetPosts();

        Product GetPostById(Guid postId);

        Product GetProductByCode(string code);

        bool UpdatePost(Product postToUpdate);
        bool DeletePost(Guid postId);
    }
}
