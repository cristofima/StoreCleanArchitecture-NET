using Microsoft.AspNetCore.Mvc;
using Store.ApplicationCore.DTOs;
using Store.ApplicationCore.Exceptions;
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
            try
            {
                var product = this.productRepository.GetProductById(id);
                return Ok(product);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
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
            try
            {
                var product = this.productRepository.UpdateProduct(id, request);
                return Ok(product);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                this.productRepository.DeleteProductById(id);
                return NoContent();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }
    }
}