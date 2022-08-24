using Microsoft.EntityFrameworkCore;
using SteamPriceAPI.Models;

namespace SteamPriceAPI.Contexts
{
    /// <summary>
    /// Muss von DBContext erben, sodass wir DBSets erstellen können und DBContextOptions benutzen können
    /// DbSet definiert eine Tabelle in der Datenbank
    /// </summary>
    public class ItemContext : DbContext
    {
        public DbSet<CSGOItem> CSGOItems { get; set; }
        public DbSet<SteamItem> SteamItems { get; set; }
        public DbSet<DotaItem> DotaItems { get; set; }
        public DbSet<H1Z1JSItem> H1Z1JSItems { get; set; }
        public DbSet<H1Z1KOTKItem> H1Z1KOTKItems { get; set; }
        public DbSet<RustItem> RustItems { get; set; }
        public DbSet<Payday2Item> Payday2Items { get; set; }
        public DbSet<TF2Item> TF2Items { get; set; }

        public ItemContext(DbContextOptions<ItemContext> _options) : base(_options)
        {
        }
    }
}
