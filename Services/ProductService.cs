using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProductCatalog.Domain;
using ProductCatalog.Services;

namespace ProductCatalog.Service
{
    public class ProductService : IProductService
    {
        private readonly List<Product> _posts;

        public ProductService()
        {
            _posts = new List<Product>();
            for (int i = 0; i < 5; i++)
            {
                _posts.Add(new Product
                {
                    Id = Guid.NewGuid(),
                    Name = $"Post Name {i}"
                });



            }
        }
        public List<Product> GetPosts()
        {
            return _posts;

        }
        public Product GetPostById(Guid postId)
        {
            
            return _posts.SingleOrDefault(x => x.Id == postId);
        }

        public bool UpdatePost(Product postToUpdate)
        {
            var exists = GetPostById(postToUpdate.Id) != null;

            if (!exists)
                return false;

            var index = _posts.FindIndex(x => x.Id == postToUpdate.Id);
            _posts[index] = postToUpdate;
            return true;
        }

        public bool DeletePost(Guid postId)
        {
            var post = GetPostById(postId);

            if (post == null)
                return false;

            _posts.Remove(post);
            return true;
        }

        public Product GetProductByCode(string code)
        {
            return _posts.SingleOrDefault(x => x.Code == code);
        }
    }
}
