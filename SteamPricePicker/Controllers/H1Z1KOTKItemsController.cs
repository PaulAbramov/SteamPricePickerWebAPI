using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SteamPriceAPI.Contexts;
using SteamPriceAPI.Models;

namespace SteamPriceAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Items/433850")]
    public class H1Z1KOTKItemsController : Controller
    {
        private readonly ItemContext context;

        public H1Z1KOTKItemsController(ItemContext _context)
        {
            context = _context;
        }

        // GET: api/Items
        [HttpGet]
        public IEnumerable<H1Z1KOTKItem> GetItems()
        {
            return context.H1Z1KOTKItems;
        }

        // GET: api/Items/name
        [HttpGet("{name}")]
        public async Task<IActionResult> GetItem([FromRoute] string name)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            H1Z1KOTKItem item = await context.H1Z1KOTKItems.SingleOrDefaultAsync(_m => _m.Name.Equals(name));

            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }
    }
}