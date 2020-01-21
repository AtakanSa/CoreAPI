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

        /// <summary>
        ///  Gets All Product Items in list
        /// </summary>
        /// <response code = "200">Succesfully fetched items</response>
        /// <returns></returns>
        [HttpGet(ApiRoutes.Posts.GetAll)]
        public IActionResult GetAll()
        {
            return Ok(_postService.GetProducts());
        }

        /// <summary>
        ///  Search Product by Name
        /// </summary>
        /// <remarks>
        /// Sample Request : 
        /// 
        ///     POST /api/v1/products/search/{productName}
        ///     {
        ///         "productName" : "name"
        ///     }
        /// </remarks>
        /// <response code = "200">Returns Item Object</response>
        /// <response code = "404">Not Found</response>
        /// <returns></returns>
        [HttpGet(ApiRoutes.Posts.Search)]
        public IActionResult GetProductByName([FromRoute]String productName)
        {
            return Ok(_postService.GetProductByName(productName));
        }

        /// <summary>
        ///  Deletes Product Item From List
        /// </summary>
        /// <remarks>
        /// Sample Request : 
        /// 
        ///     Delete /api/v1/products/{productId}
        ///     {
        ///         "productId" : "Id"
        ///     }
        /// </remarks>
        /// <response code = "204">Deleted Item</response>
        /// <response code = "404">Not Found</response>
        /// <returns></returns>
        [HttpDelete(ApiRoutes.Posts.Delete)]
        public IActionResult Delete([FromRoute]Guid postId)
        {
            var deleted = _postService.DeleteProduct(postId);

            if (deleted)
                return NoContent();

            return NotFound();
        }

        /// <summary>
        ///  Get Product Details by Id
        /// </summary>
        /// <remarks>
        /// Sample Request : 
        /// 
        ///     GET /api/v1/products/{productId}
        ///     {
        ///         "productId" : "Id"
        ///     }
        /// </remarks>
        /// <response code = "200">Success</response>
        /// <response code = "404">Not Found</response>
        /// <returns></returns>
        [HttpGet(ApiRoutes.Posts.Get)]
        public IActionResult Get([FromRoute]Guid postId)
        {
            var post = _postService.GetProductById(postId);

            if (post == null)
                return NotFound();

            return Ok(post);
        }
        /// <summary>
        /// Update an item using item's Id
        /// </summary>
        /// <remarks>
        /// Sample Request : 
        /// 
        ///     PUT /api/v1/products/{productId}
        ///     {
        ///         "Name" : "productName",
        ///         "Price" : 100,
        ///         "updatedAt" : "2020-01-21T07:54:54.845Z",
        ///         "picture" : "pictureUrl" ,
        ///         "code" : someCode
        ///     }
        /// </remarks>
        /// <response code = "200">Success</response>
        /// <response code = "404">Not Found</response>
        /// <returns></returns>
        [HttpPut(ApiRoutes.Posts.Update)]
        public IActionResult Update([FromRoute]Guid postId, [FromBody] UpdateProductRequest request)
        {
            if (!ModelState.IsValid)
            {
                string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                return BadRequest(new { error = messages });
            }
            var codeUniqness = _postService.GetProductByCode(request.Code);

            if (codeUniqness != null)
                return BadRequest(new { error = "Code must be unique !" });

            string pictureUrl;
            if (string.IsNullOrEmpty(request.Picture))
            {
                pictureUrl = "";
            }
            else
            {

                if (!Uri.IsWellFormedUriString(request.Picture, UriKind.RelativeOrAbsolute))
                    return BadRequest(new { error = "Picture URL is not well Formatted !" });
                pictureUrl = request.Picture;
            }

            var post = new Product
            {
                Id = postId,
                Picture = pictureUrl,
                Name = request.Name,
                Price = request.Price,
                Code = request.Code,
                UpdatedAt = request.UpdatedAt
            };

            var updated = _postService.UpdateProduct(post);

            if (updated)
                return Ok(post);

            return NotFound();

        }

        /// <summary>
        ///  Create new Product Item 
        /// </summary>
        /// <remarks>
        /// Sample Request : 
        /// 
        ///     POST /api/v1/products
        ///     {
        ///         "Name" : "productName",
        ///         "Price" : 200,
        ///         "updatedAt" : "2020-01-21T07:54:54.845Z",
        ///         "picture" : "pictureUrl" ,
        ///         "code" : someCode
        ///     }
        /// </remarks>
        /// <response code = "200">Success</response>
        /// <response code = "404">Not Found</response>
        /// <returns></returns>
        [HttpPost(ApiRoutes.Posts.Create)]
        public IActionResult Create([FromBody] CreateProductRequest postRequest)
        {
            if (!ModelState.IsValid)
            {
                string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                return BadRequest(new { error = messages });
            }
            

            var codeUniqness = _postService.GetProductByCode(postRequest.Code);

            if (codeUniqness != null)
                return BadRequest(new { error = "Code must be unique !" });

            string pictureUrl;
            if (string.IsNullOrEmpty(postRequest.Picture))
            {
                pictureUrl = "";
            }
            else
            {

                if (!Uri.IsWellFormedUriString(postRequest.Picture, UriKind.RelativeOrAbsolute))
                    return BadRequest(new { error = "Picture URL is not well Formatted !" });
                pictureUrl = postRequest.Picture;
            }
            

            var product = new Product
            {
                Name = postRequest.Name,
                Price = postRequest.Price,
                UpdatedAt = postRequest.UpdatedAt,
                Code = postRequest.Code,
                Picture = pictureUrl,
                Id = Guid.NewGuid()
            };


            _postService.GetProducts().Add(product);

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
