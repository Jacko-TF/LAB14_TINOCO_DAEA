using LAB14_TINOCO_DAEA.Models.Request;
using LAB14_TINOCO_DAEA.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LAB14_TINOCO_DAEA.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CustomerCustomController : ControllerBase
    {
        private readonly MarketContext _context;

        public CustomerCustomController(MarketContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<Customer>> InsertCustomer(RequestCustomerV1 requestCustomerV1)
        {
            Customer customer = new();
            customer.FirstName = requestCustomerV1.FirstName;
            customer.DocumentNumber = requestCustomerV1.DocumentNumber;
            customer.LasttName = requestCustomerV1.LasttName;
            customer.Email = requestCustomerV1.Email;

            if (_context.Customers == null)
            {
                return Problem("Entity set 'MarketContext.Customers'  is null.");
            }
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("InsertCustomer", new { id = customer.CustomerID }, customer);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCustomer(RequestCustomerV2 requestCustomerV2)
        {
            var id = requestCustomerV2.CustomerId;

            if (_context.Customers == null)
            {
                return NotFound();
            }
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            customer.Active = false;
            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut]
        public async Task<ActionResult<Customer>> UpdateCustomerDocumentNumberAndEmail([FromBody] RequestCustomerV3 requestCustomerV3)
        {
            var customer = await _context.Customers.FindAsync(requestCustomerV3.CustomerID);

            if (customer == null || customer.Active == false)
            {
                return NotFound();
            }

            customer.DocumentNumber = requestCustomerV3.DocumentNumber;
            customer.Email = requestCustomerV3.Email;

            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("UpdateCustomerDocumentNumberAndEmail", new { id = customer.CustomerID }, customer);
        }
    }
}
