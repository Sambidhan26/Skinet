using Core.Entities;
using Core.Interfaces;
using Infrastrucutre.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController(IProductRepository repo) : ControllerBase
    {

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(string? brand, string? type, string? sort)
        {
            var products = await repo.GetProductsAsync(brand, type, sort);
            return Ok(products);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await repo.GetProductByIdAsync(id);
            if (product == null)
            {

                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            repo.AddProduct(product);

            if( await repo.SaveAllAsync())
            {
                return CreatedAtAction("GetProduct", new { id = product.Id }, product);
            }
            return BadRequest("Cannot Create this Product"); //CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Product>> UpdateProduct(int id, Product product)
        {
            if (product.Id != id || !ProductExists(id))
            {
                return BadRequest("Cannot update this Product");
            }
                repo.UpdateProduct(product);
                if (await repo.SaveAllAsync())
                {
                    return  NoContent();
                }
                else
                return BadRequest("Problem Cannot Update this Product");

        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Product>> DeleteProduct(int id)
        {
            var product = await repo.GetProductByIdAsync(id);

            if (product == null )
            {
                return NotFound();
            }
                repo.DeleteProduct(product);
            if (await repo.SaveAllAsync())
            {
                return NoContent();
            }
            else
                return BadRequest("Problem Cannot Delete this Product");

        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetProductBrands()
        {
            var brands = await repo.GetBrandAsync();
            return Ok(brands);
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
        {   
            var types = await repo.GetTypesAsync();
            return Ok(types);
        }

        private bool ProductExists(int id)
        {
            return repo.ProductExists(id);
        }

    }
}
