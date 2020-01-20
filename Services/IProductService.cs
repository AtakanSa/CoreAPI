using ProductCatalog.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalog.Services
{
    public interface IProductService
    {
        List<Product> GetProducts();

        Product GetProductById(Guid postId);

        Product GetProductByCode(string code);

        bool UpdateProduct(Product postToUpdate);
        bool DeleteProduct(Guid postId);
    }
}
