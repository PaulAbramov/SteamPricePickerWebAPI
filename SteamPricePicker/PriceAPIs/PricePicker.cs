using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SteamPriceAPI.Models;
using SteamPriceAPI.PriceAPIs.BitSkins.Json;
using SteamPriceAPI.PriceAPIs.OPSkins.Json;
using SteamPriceAPI.PriceAPIs.WebRequests;

namespace SteamPriceAPI.PriceAPIs
{
    public class PricePicker
    {
        /// <summary>
        /// Get the price of all recent sold items for the given game
        /// </summary>
        /// <param name="_game"></param>
        public static async Task<List<Item>> CatchAndSavePriceFromAPI(string _url)
        {
            List<Item> itemList = new List<Item>();

            var response = await WebRequest.SendRequest(_url);

            if(_url.ToLower().Contains("opskins"))
            {
                Dictionary<string, Dictionary<string, OPSkinsItemPrice>> items = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, OPSkinsItemPrice>>>(response);

                foreach (KeyValuePair<string, Dictionary<string, OPSkinsItemPrice>> item in items)
                {
                    Item generatedItem = new Item
                    {
                        Name = item.Key,
                        Value = item.Value.Sum(_date => _date.Value.Price)
                    };

                    generatedItem.Value /= item.Value.Count;

                    generatedItem.Value /= 100;

                    itemList.Add(generatedItem);
                }
            }
            else if(_url.ToLower().Contains("steamapi"))
            {
                Dictionary<string, string> items = JsonConvert.DeserializeObject<Dictionary<string, string>>(response);

                itemList.AddRange(items.Select(_item => new Item
                {
                    Name = _item.Key, Value = Convert.ToSingle(_item.Value) / 100
                }));
            }
            else if (_url.ToLower().Contains("bitskins"))
            {
                BitSkinsAPIResponse items = JsonConvert.DeserializeObject<BitSkinsAPIResponse>(response);

                itemList.AddRange(items.Prices.Select(_item => new Item
                {
                    Name = _item.MarketHashName, Value = Convert.ToSingle(_item.Price) / 100
                }));
            }

            return itemList;
        }
    }

    public enum Game
    {
        TF2 = 440,
        Dota = 570,
        Csgo = 730,
        Steam = 753,
        Payday2 = 218620,
        Rust = 252490,
        H1Z1JS = 295110,
        H1Z1Kotk = 433850,
    }
}