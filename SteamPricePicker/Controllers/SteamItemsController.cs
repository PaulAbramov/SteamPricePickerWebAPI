using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SteamPriceAPI.Contexts;
using SteamPriceAPI.Models;

namespace SteamPriceAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Items/753")]
    public class SteamItemsController : Controller
    {
        private readonly ItemContext context;

        public SteamItemsController(ItemContext _context)
        {
            context = _context;
        }

        // GET: api/Items
        [HttpGet]
        public IEnumerable<SteamItem> GetItems()
        {
            return context.SteamItems;
        }

        // GET: api/Items/name
        [HttpGet("{name}")]
        public async Task<IActionResult> GetItem([FromRoute] string name)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            SteamItem item = await context.SteamItems.SingleOrDefaultAsync(_m => _m.Name.Equals(name));

            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }
    }
}