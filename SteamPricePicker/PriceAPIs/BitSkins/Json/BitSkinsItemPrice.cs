using Newtonsoft.Json;

namespace SteamPriceAPI.PriceAPIs.BitSkins.Json
{
    public class BitSkinsItemPrice
    {
        [JsonProperty("appid")]
        public string AppID { get; set; }
        [JsonProperty("market_hash_name")]
        public string MarketHashName { get; set; }
        [JsonProperty("price")]
        public string Price { get; set; }
        [JsonProperty("pricing_mode")]
        public string PricingMode { get; set; }
        [JsonProperty("skewness")]
        public string Skewness { get; set; }
        [JsonProperty("created_at")]
        public long CreatedAt { get; set; }
    }
}