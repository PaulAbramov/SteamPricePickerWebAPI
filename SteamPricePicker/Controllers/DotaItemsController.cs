using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SteamPriceAPI.Contexts;
using SteamPriceAPI.Models;

namespace SteamPriceAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Items/570")]
    public class DotaItemsController : Controller
    {
        private readonly ItemContext context;

        public DotaItemsController(ItemContext _context)
        {
            context = _context;
        }

        // GET: api/Items
        [HttpGet]
        public IEnumerable<DotaItem> GetItems()
        {
            return context.DotaItems;
        }

        // GET: api/Items/name
        [HttpGet("{name}")]
        public async Task<IActionResult> GetItem([FromRoute] string name)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            DotaItem item = await context.DotaItems.SingleOrDefaultAsync(_m => _m.Name.Equals(name));

            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }
    }
}