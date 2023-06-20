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

    [ApiController]
    [Route("[controller]")]
    public class CarrierAvaliablesController : Controller
    {
        private readonly _DbContext _context;

        public CarrierAvaliablesController(_DbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Lists all available delivery services
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CarrierAvaliable>>> GetListCarrierAvaliable()
        {
            // Get the list of available carriers from the frenet api
            List<CarrierAvaliable> listCarrierFrenet = await Auxiliary.Frenet.ListCarrierAvaliable();

            // Get the list of available carriers from the database
            var listCarrierAvaliable = await _context.CarrierAvaliable.ToListAsync();

            if (listCarrierAvaliable.Count == 0 || listCarrierAvaliable == null)
            {
                return NotFound();
            }
            
            foreach (var item in listCarrierFrenet)
            {
                // Verify that the CarrierCode already exists in the database
                bool exists = listCarrierAvaliable.Any(c => c.CarrierCode == item.CarrierCode);

                if (!exists)
                {
                    // If it does not exist, add the new object to the CarrierAvaliable list
                    listCarrierAvaliable.Add(item);
                    // Save the new record to the database
                    _context.CarrierAvaliable.Add(item);
                }
            }

            // Save Changes
            await _context.SaveChangesAsync();

            return listCarrierAvaliable;
        }

        /// <summary>
        /// Remove Unique Carrier Avaliable
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> RemoveCarrierAvaliable(int id)
        {
            // Get the list of available carriers from the database
            var carrierAvaliable = await _context.CarrierAvaliable.FindAsync(id);

            if (carrierAvaliable == null)
            {
                return NotFound();
            }

            try
            {
                _context.Remove(carrierAvaliable);

                // Save Changes
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
        
        /// <summary>
        /// Update Carrier Avaliable
        /// </summary>
        /// <param name="carrier"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> UpdateCarrierAvaliable(CarrierAvaliable carrier)
        {
            // Get the list of available carriers from the database
            var carrierAvaliable = await _context.CarrierAvaliable.FindAsync(carrier.id);

            if (carrierAvaliable == null)
            {
                return NotFound();
            }

            try
            {
                //Update Carrier
                carrier.CopyData(ref carrierAvaliable);

                // Save Changes
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

    }
}
