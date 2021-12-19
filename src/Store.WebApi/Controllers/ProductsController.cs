using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.ApplicationCore.DTOs;
using Store.ApplicationCore.Exceptions;
using Store.ApplicationCore.Interfaces;
using System.Collections.Generic;
using System.Net.Mime;

namespace Store.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces("application/json")]
    public class ProductsController : Controller
    {
        private readonly IProductRepository productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        /// <summary>
        /// Get all products
        /// </summary>
        /// <response code="200">Returns the products</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ProductResponse>))]
        public ActionResult GetProducts()
        {
            return Ok(this.productRepository.GetProducts());
        }

        /// <summary>
        /// Get a product by id
        /// </summary>
        /// <param name="id">Product id</param>
        /// <response code="200">Returns the existing product</response>
        /// <response code="404">If the product doesn't exist</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SingleProductResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Create a product
        /// </summary>
        /// <param name="request">Product data</param>
        /// <response code="201">Returns the created product</response>
        /// <response code="400">If the data doesn't pass the validations</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(SingleProductResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult Create(CreateProductRequest request)
        {
            var product = this.productRepository.CreateProduct(request);
            return StatusCode(201, product);
        }

        /// <summary>
        /// Update a product by id
        /// </summary>
        /// <param name="id">Product id</param>
        /// <param name="request">Product data</param>
        /// <response code="200">Returns the updated product</response>
        /// <response code="400">If the data doesn't pass the validations</response>
        /// <response code="404">If the product doesn't exist</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SingleProductResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Delete a product by id
        /// </summary>
        /// <param name="id">Product id</param>
        /// <response code="204">If the product was deleted</response>
        /// <response code="404">If the product doesn't exist</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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