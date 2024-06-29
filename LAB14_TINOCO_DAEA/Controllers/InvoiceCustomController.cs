using LAB14_TINOCO_DAEA.Models;
using LAB14_TINOCO_DAEA.Models.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LAB14_TINOCO_DAEA.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class InvoiceCustomController : ControllerBase
    {
        private readonly MarketContext _context;

        public InvoiceCustomController(MarketContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<ActionResult<Invoice>> InsertInvoice([FromBody] RequestInvoiceV1 requestInvoiceV1)
        {
            var customer = await _context.Customers.FindAsync(requestInvoiceV1.CustomerID);

            if (customer == null)
            {
                return NotFound();
            }

            var invoice = new Invoice
            {
                CustomerID = requestInvoiceV1.CustomerID,
                Date = requestInvoiceV1.Date,
                InvoiceNumber = requestInvoiceV1.InvoiceNumber,
                Total = requestInvoiceV1.Total,
                Customer = customer
            };

            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(InsertInvoice), new { id = invoice.InvoiceID }, invoice);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<ActionResult<List<Invoice>>> InsertInvoiceList([FromBody] RequestInvoiceV2 requestInvoiceV2)
        {
            try
            {
                var customer = await _context.Customers.FindAsync(requestInvoiceV2.CustomerID);

                if (customer == null)
                {
                    return NotFound();
                }

                List<Invoice> invoiceList = requestInvoiceV2.requestInvoiceV3s.Select(x => new Invoice
                {
                    Date = x.Date,
                    Total = x.Total,
                    InvoiceNumber= x.InvoiceNumber,
                    CustomerID = customer.CustomerID,
                }).ToList();

                await _context.SaveChangesAsync();

                return CreatedAtAction("InsertInvoiceList", new { message = "Invoices insert correctly" }, invoiceList);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
            
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<ActionResult<Invoice>> InsertInvoiceDetail([FromBody] RequestInvoiceV4 requestInvoiceV4)
        {
            //Validando que la factura exista
            var invoice = await _context.Invoices.FindAsync(requestInvoiceV4.InvoiceID);

            if (invoice == null)
            {
                return NotFound();
            }

            var details = requestInvoiceV4.DetailV1s.ToList();

            //Para capturar los nuevos details
            List<Detail> newDetails= new();

            foreach (var invoiceDetail in details)
            {
                Detail detail = new();

                detail.InvoiceID = invoice.InvoiceID;

                int idProduct = invoiceDetail.ProductID;
                var product = await _context.Products.FindAsync(idProduct);
                if (product == null)
                {
                    return Problem();
                }

                detail.ProductID = invoiceDetail.ProductID;
                detail.Price = product.Price;
                detail.Amount = invoiceDetail.Amount;
                detail.SubTotal = detail.Amount * detail.Price;

                _context.Details.Add(detail);

                newDetails.Add(detail);
                invoice.Total += detail.SubTotal;
            }

            //Actualizar el total de la factura
            _context.Invoices.Update(invoice);
            await _context.SaveChangesAsync();

            return CreatedAtAction("InsertInvoiceDetail", new { id = invoice.InvoiceID }, newDetails);
        }
    }
}
