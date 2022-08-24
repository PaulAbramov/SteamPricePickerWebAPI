using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SteamPriceAPI.Contexts;
using SteamPriceAPI.Models;

namespace SteamPriceAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Items/218620")]
    public class Payday2ItemsController : Controller
    {
        private readonly ItemContext context;

        public Payday2ItemsController(ItemContext _context)
        {
            context = _context;
        }

        // GET: api/Items
        [HttpGet]
        public IEnumerable<Payday2Item> GetItems()
        {
            return context.Payday2Items;
        }

        // GET: api/Items/name
        [HttpGet("{name}")]
        public async Task<IActionResult> GetItem([FromRoute] string name)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Payday2Item item = await context.Payday2Items.SingleOrDefaultAsync(_m => _m.Name.Equals(name));

            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }
    }
}