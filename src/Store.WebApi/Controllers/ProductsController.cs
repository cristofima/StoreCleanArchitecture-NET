using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.ApplicationCore.DTOs;
using Store.ApplicationCore.Exceptions;
using Store.Infrastructure.Persistence.Commands;
using Store.Infrastructure.Persistence.Queries;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Store.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces("application/json")]
    public class ProductsController : Controller
    {
        private readonly ISender mediator;

        public ProductsController(ISender mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Get all products
        /// </summary>
        /// <response code="200">Returns the products</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ProductResponse>))]
        public async Task<ActionResult> GetProductsAsync()
        {
            return Ok(await mediator.Send(new GetAllProductsQuery()));
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
        public async Task<ActionResult> GetProductByIdAsync(int id)
        {
            try
            {
                var product = await mediator.Send(new GetProductByIdQuery(id));
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
        public async Task<ActionResult> CreateAsync(CreateProductRequest request)
        {
            var product = await mediator.Send(new AddProductCommand(request));
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
        public async Task<ActionResult> UpdateAsync(int id, UpdateProductRequest request)
        {
            try
            {
                var product = await mediator.Send(new UpdateProductCommand(id, request));
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
        public async Task<ActionResult> DeleteAsync(int id)
        {
            try
            {
                await mediator.Send(new DeleteProductByIdCommand(id));
                return NoContent();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }
    }
}