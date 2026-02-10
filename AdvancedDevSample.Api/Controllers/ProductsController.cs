using AdvancedDevSample.Application.DTOs;
using AdvancedDevSample.Application.Exceptions;
using AdvancedDevSample.Application.Services;
using AdvancedDevSample.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AdvancedDevSample.Domain.Interfaces.Products;

namespace AdvancedDevSample.Api.Controllers
{
    [ApiController]
    [Route("api/products")]
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductsController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAll()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        public async Task<ActionResult<ProductDto>> GetProduct(Guid id)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(id);
                return Ok(product);
            }
            catch (ApplicationServiceException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<IActionResult> Create([FromBody] CreateProductRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var id = await _productService.CreateProductAsync(request);
                return CreatedAtAction(nameof(GetProduct), new { id }, null);
            }
            catch (ApplicationServiceException ex)
            {
                return BadRequest(ex.Message); // Provider not found
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _productService.UpdateProductAsync(id, request);
                return NoContent();
            }
            catch (ApplicationServiceException ex)
            {
                return NotFound(ex.Message);
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _productService.DeleteProductAsync(id);
                return NoContent();
            }
            catch (ApplicationServiceException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPatch("{id}/price")]
        public async Task<IActionResult> ChangePrice(Guid id, [FromBody] ChangePriceRequest request)
        {
            try
            {
                await _productService.ChangeProductPriceAsync(id, request.NewPrice);
                return NoContent();
            }
            catch (ApplicationServiceException ex) 
            {
                return NotFound(ex.Message);
            }
            catch (DomainException ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{id}/discount")]
        public async Task<IActionResult> ApplyDiscount(Guid id, [FromQuery] decimal discount)
        {
             try
            {
                await _productService.ApplyProductDiscountAsync(id, discount);
                return NoContent();
            }
             catch (ApplicationServiceException ex) 
            {
                return NotFound(ex.Message);
            }
             catch (DomainException ex) 
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpPatch("{id}/activate")]
         public async Task<IActionResult> Activate(Guid id)
        {
             try
            {
                await _productService.ActivateProductAsync(id);
                return NoContent();
            }
             catch (ApplicationServiceException ex) 
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPatch("{id}/desactivate")]
         public async Task<IActionResult> Desactivate(Guid id)
        {
             try
            {
                await _productService.DesactivateProductAsync(id);
                return NoContent();
            }
             catch (ApplicationServiceException ex) 
            {
                return NotFound(ex.Message);
            }
        }
    }
}
