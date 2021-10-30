using Microsoft.AspNetCore.Mvc;
using Store.ApplicationCore.DTOs;
using Store.ApplicationCore.Interfaces;
using System.Collections.Generic;

namespace Store.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : Controller
    {
        private readonly IProductRepository productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        [HttpGet]
        public ActionResult<List<ProductResponse>> GetProducts()
        {
            return Ok(this.productRepository.GetProducts());
        }

        [HttpGet("{id}")]
        public ActionResult GetProductById(int id)
        {
            var product = this.productRepository.GetProductById(id);
            if (product != null)
            {
                return Ok(product);
            }

            return NotFound();
        }

        [HttpPost]
        public ActionResult Create(CreateProductRequest request)
        {
            var product = this.productRepository.CreateProduct(request);
            return Ok(product);
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, UpdateProductRequest request)
        {
            var product = this.productRepository.UpdateProduct(id, request);
            if (product != null)
            {
                return Ok(product);
            }

            return NotFound();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            this.productRepository.DeleteProductById(id);
            return NoContent();
        }
    }
}