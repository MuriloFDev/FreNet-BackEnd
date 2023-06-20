using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.Auxiliary;
using App.Model;
using App.Models;

namespace App.Controllers
{
    #region Class
    public class ShippingRequest
    {
        public string SellerCEP { get; set; }
        public string RecipientCEP { get; set; }
        public List<ShippingRequestItem> ShippingItemArray { get; set; }
        public string RecipientCountry { get; set; }
    }

    public class ShippingRequestItem
    {
       public double Height { get; set; }
        public double Length { get; set; }
        public double Quantity { get; set; }
        public double Weight { get; set; }
        public double Width { get; set; }
    }
    
    public class ReturnQuote
    {
        public string Carrier { get; set; }
        public string CarrierCode { get; set; }
        public string DeliveryTime { get; set; }
        public string Msg { get; set; }
        public string ServiceCode { get; set; }
        public string ServiceDescription { get; set; }
        public string ShippingPrice { get; set; }
        public string OriginalDeliveryTime { get; set; }
        public string OriginalShippingPrice { get; set; }
        public bool Error { get; set; }
    }
    #endregion
    
    [ApiController]
    [Route("[controller]")]
    public class QuotationController : Controller
    {
        private readonly _DbContext _context;

        public QuotationController(_DbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Performs freight quotation
        /// </summary>
        /// <returns></returns>
        [HttpPost("Quote_Shipping")]
        public async Task<ActionResult<IEnumerable<ReturnQuote>>> QuoteShopping(ShippingRequest quoteProps)
        {
            try
            {
                // Get the list of available carriers from the frenet api
                List<ReturnQuote> listCarrierFrenet = await Auxiliary.Frenet.GetShippingQuotes(quoteProps);

                foreach(var item in listCarrierFrenet)
                {
                    var quotation = new Quotation
                    {
                        Carrier = item.Carrier,
                        CarrierCode = item.CarrierCode,
                        DeliveryTime = item.DeliveryTime,
                        Msg = item.Msg,
                        OriginalDeliveryTime = item.OriginalDeliveryTime,
                        OriginalShippingPrice = item.OriginalShippingPrice,
                        ServiceCode = item.ServiceCode,
                        ServiceDescription = item.ServiceDescription,
                        ShippingPrice = item.ShippingPrice,
                        RecipientCEP = quoteProps.RecipientCEP,
                        SellerCEP = quoteProps.SellerCEP
                    };
                    
                    _context.Quotation.Add(quotation);
                }

                // Save Changes
                await _context.SaveChangesAsync();

                return listCarrierFrenet
                    .OrderByDescending(w => w.ShippingPrice)
                    .ToList();
            }
            catch
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Performs cep quotation
        /// </summary>
        /// <returns></returns>
        [HttpGet("QuoteByCep/{cep}")]
        public async Task<ActionResult<IEnumerable<Quotation>>> GetQuoteByFreight(string cep)
        {
            var listQuote = await _context.Quotation
                .Where(w => w.SellerCEP == cep)
                .Take(10)
                .ToListAsync();
                                       
            return listQuote;
        }

    }
}
