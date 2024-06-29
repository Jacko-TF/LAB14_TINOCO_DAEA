using LAB14_TINOCO_DAEA.Models;
using LAB14_TINOCO_DAEA.Models.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LAB14_TINOCO_DAEA.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductCustomController : ControllerBase
    {
        private readonly MarketContext _context;

        public ProductCustomController(MarketContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<ActionResult<Product>> InsertProduct([FromBody] RequestProductV1 requestProduct)
        {
            Product product = new();
            product.Name = requestProduct.Name;
            product.Price = requestProduct.Price;

            if (_context.Products == null)
            {
                return Problem("Entity set 'MarketContext.Products'  is null.");
            }
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("InsertProduct", new { id = product.ProductID }, product);
        }

        [Authorize(Roles = "Administrator")]
        [HttpDelete]
        public async Task<IActionResult> DeleteProduct(RequestProductV2 requestProduct)
        {
            var id = requestProduct.ProductID;

            if (_context.Products == null)
            {
                return NotFound();
            }
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            product.Active = false;
            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [Authorize(Roles = "Administrator")]
        [HttpPut]
        public async Task<ActionResult<Customer>> UpdateProductPrice([FromBody] RequestProductV3 requestProductV3)
        {
            var product = await _context.Products.FindAsync(requestProductV3.ProductId);

            if (product == null || product.Active == false)
            {
                return NotFound();
            }

            product.Price = requestProductV3.Price;

            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("UpdateProductPrice", new { id = product.ProductID }, product);
        }

        [Authorize(Roles = "Administrator")]
        [HttpDelete]
        public async Task<IActionResult> DeleteProductList(List<RequestProductV2> requestProductV2s)
        {

            if (_context.Products == null)
            {
                return NotFound();
            }

            foreach (var productID in requestProductV2s)
            {
                var product = await _context.Products.FindAsync(productID.ProductID);
                if (product == null)
                {
                    return NotFound();
                }
                product.Active = false;
                _context.Products.Update(product);
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
