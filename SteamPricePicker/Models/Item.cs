namespace SteamPriceAPI.Models
{
    /// <summary>
    /// ID wird automatisch als Key gesetzt
    /// anders auch möglich: "classnameID"
    /// </summary>
    public class Item
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public float Value { get; set; }
    }
}
