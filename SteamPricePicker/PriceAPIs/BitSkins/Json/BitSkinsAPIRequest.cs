using System.Collections.Generic;
using Newtonsoft.Json;

namespace SteamPriceAPI.PriceAPIs.BitSkins.Json
{
    public class BitSkinsAPIResponse
    {
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("prices")]
        public List<BitSkinsItemPrice> Prices { get; set; }
    }
}
