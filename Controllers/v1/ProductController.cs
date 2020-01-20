using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Contract.v1;
using ProductCatalog.Contract.v1.Requests;
using ProductCatalog.Contract.v1.Responses;
using ProductCatalog.Domain;
using ProductCatalog.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ProductCatalog.Contract.v1.ApiRoutes;

namespace ProductCatalog.Controllers.v1
{
    public class ProductController : Controller
    {

        private readonly IProductService _postService;
        public ProductController(IProductService postService)
        {
            _postService = postService;
        }

        [HttpGet(ApiRoutes.Posts.GetAll)]
        public IActionResult GetAll()
        {
            return Ok(_postService.GetPosts());
        }
        [HttpDelete(ApiRoutes.Posts.Delete)]
        public IActionResult Delete([FromRoute]Guid postId)
        {
            var deleted = _postService.DeletePost(postId);

            if (deleted)
                return NoContent();

            return NotFound();
        }

        [HttpGet(ApiRoutes.Posts.Get)]
        public IActionResult Get([FromRoute]Guid postId)
        {
            var post = _postService.GetPostById(postId);

            if (post == null)
                return NotFound();

            return Ok(post);
        }

        [HttpPut(ApiRoutes.Posts.Update)]
        public IActionResult Update([FromRoute]Guid postId, [FromBody] UpdateProductRequest request)
        {
            var post = new Product {
                Id = postId,
                Name = request.Name
            };

            var updated = _postService.UpdatePost(post);

            if (updated)
                return Ok(post);

            return NotFound();


           
        }

        [HttpPost(ApiRoutes.Posts.Create)]
        public IActionResult Create([FromBody] CreateProductRequest postRequest)
        {
            if(!ModelState.IsValid)
            {
                string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                return BadRequest(new { error = messages });
            }

            var codeUniqness = _postService.GetProductByCode(postRequest.Code);

            if (codeUniqness != null)
                return BadRequest(new { error = "Code must be unique !" });

           // if (postRequest.Price < 0 && postRequest.Price > 1000)
             //   return BadRequest(new { error = "Price must be lower then 100 higher then 0" });

            string pictureUrl;
            if (string.IsNullOrEmpty(postRequest.Picture))
            {
                pictureUrl = "";
            }
            else
            {
                pictureUrl = postRequest.Picture;
            }

            var product = new Product
            {
                Name = postRequest.Name,
                Price = postRequest.Price,
                UpdatedAt = DateTime.UtcNow,
                Code = postRequest.Code,
                Picture = pictureUrl,
                Id = Guid.NewGuid()
            };


           

            _postService.GetPosts().Add(product);

            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUrl = baseUrl + "/" + ApiRoutes.Posts.Get.Replace("{postId}", product.Id.ToString());

            var response = new ProductResponse 
            { 
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                UpdatedAt = product.UpdatedAt,
                Code = product.Code,
                Picture = product.Picture
            };
            return Created(locationUrl, response);



        }
    }
}
