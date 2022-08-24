using Newtonsoft.Json;

namespace SteamPriceAPI.PriceAPIs.OPSkins.Json
{
    public class OPSkinsItemPrice
    {
        [JsonProperty("price")]
        public int Price { get; set; }
        [JsonProperty("count")]
        public int Count { get; set; }
    }
}
