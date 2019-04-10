using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SnackStore.Models;

namespace SnackStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductsContext _context;
        public ProductsController(ProductsContext context)
        {
            _context = context;

            if (_context.ProductsItems.Count() == 0)
            {
                _context.ProductsItems.Add(new ProductsItem{ Name = "Item 1" });
                _context.SaveChanges();
            }

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductsItem>>> GetProductsItems()
        {
            return await _context.ProductsItems.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductsItem>> GetProductsItem(long id)
        {
            var ProductItem = await _context.ProductsItems.FindAsync(id);

            if (ProductItem == null)
            {
                return NotFound();
            }

            return ProductItem;
        }

        [HttpPost]
        public async Task<ActionResult<ProductsItem>> PostProductsItem(ProductsItem item)
        {
            _context.ProductsItems.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProductsItem), new { Id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductItem(long id, ProductsItem item)
        {
            if (id != item.Id)
            {
                return BadRequest();
            }

            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }


    }

}