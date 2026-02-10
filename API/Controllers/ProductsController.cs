using Core.Entities;
using Core.Interfaces;
using Core.Specification;
using Infrastrucutre.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController(IGenericRepository<Product> repo) : ControllerBase
    {

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(string? brand, string? type, string? sort)
        {
            var spec = new ProductSpecification(brand, type, sort);
            var products = await repo.ListAsync(spec);

            return Ok(products);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await repo.GetByIdAsync(id);
            if (product == null)
            {

                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            repo.Add(product);

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
                repo.Update(product);
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
            var product = await repo.GetByIdAsync(id);

            if (product == null )
            {
                return NotFound();
            }
                repo.Remove(product);
            if (await repo.SaveAllAsync())
            {
                return NoContent();
            }
            else
                return BadRequest("Problem Cannot Delete this Product");

        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
        {
            var spec = new BrandListSpecification();
            return Ok(await repo.ListAsync(spec));
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
        {
            var spec = new TypeListSpecification();
            return Ok(await repo.ListAsync(spec));
        }

        private bool ProductExists(int id)
        {
            return repo.Exists(id);
        }

    }
}
