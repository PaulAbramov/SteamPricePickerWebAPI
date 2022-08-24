using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OtpSharp;
using SteamPriceAPI.Contexts;
using SteamPriceAPI.Models;
using SteamPriceAPI.PriceAPIs;
using SteamPricePicker;
using Wiry.Base32;

namespace SteamPriceAPI.Data
{
    /// <summary>
    /// Benutze diese Klasse um die Datenbank zu erstellen und zu füllen
    /// </summary>
    public class DBInitializer
    {
        private static readonly string m_steamIOApiKey = "";
        private static readonly string m_apiKey = "";
        private static readonly string m_secretCode = "";

        public static async void Initialize(ILogger _logger)
        {
            bool firstTime = true;
            while (true)
            {
                if(!firstTime)
                {
                    await Task.Delay(TimeSpan.FromHours(1));
                }

                _logger.LogWarning("start checking prices for csgo");

                await AddItemsToDatabase(Game.Csgo, _logger);
                await AddItemsToDatabase(Game.Steam, _logger);
                await AddItemsToDatabase(Game.Dota, _logger);
                await AddItemsToDatabase(Game.TF2, _logger);
                await AddItemsToDatabase(Game.Rust, _logger);
                await AddItemsToDatabase(Game.H1Z1Kotk, _logger);
                await AddItemsToDatabase(Game.H1Z1JS, _logger);
                await AddItemsToDatabase(Game.Payday2, _logger);

                firstTime = false;
            }
        }

        private static async Task AddItemsToDatabase(Game _game, ILogger _logger)
        {
            List<Item> bitskinItems = null;

            _logger.LogWarning("start getting prices for " + _game);

            if (_game.Equals(Game.Csgo))
            {
                Totp totpGenerator = new Totp(Base32Encoding.Standard.ToBytes(m_secretCode));
            
                bitskinItems = await PricePicker.CatchAndSavePriceFromAPI(string.Format("https://bitskins.com/api/v1/get_all_item_prices/?api_key={0}&code={1}", m_apiKey, totpGenerator.ComputeTotp()));
            }

            List<Item> opSkinsItems = await PricePicker.CatchAndSavePriceFromAPI(string.Format("https://opskins.com/pricelist/{0}.json", Convert.ToUInt32(_game)));
            List<Item> steamAPIItems = await PricePicker.CatchAndSavePriceFromAPI(string.Format("https://api.steamapi.io/market/prices/{0}?key={1}", Convert.ToUInt32(_game), m_steamIOApiKey));

            _logger.LogWarning("compare the prices");

            CompareItemPrices(out List<Item> items, _logger, opSkinsItems, steamAPIItems, bitskinItems);

            _logger.LogWarning("go trough every item");

            foreach (Item item in items)
            {
                //schlecht, einfach nur ein neues Context erstellen, ohne eine Funktion, generell keine Funktionen im using statement
                using (ItemContext itemContext = DBContextFactory.Create("DefaultConnection"))
                {
                    switch (_game)
                    {
                        case Game.Csgo:
                            RefreshDatabase(itemContext, itemContext.CSGOItems, new CSGOItem { Name = item.Name, Value = item.Value }, _logger);
                            break;
                        case Game.Steam:
                            RefreshDatabase(itemContext, itemContext.SteamItems, new SteamItem { Name = item.Name, Value = item.Value }, _logger);
                            break;
                        case Game.Dota:
                            RefreshDatabase(itemContext, itemContext.DotaItems, new DotaItem { Name = item.Name, Value = item.Value }, _logger);
                            break;
                        case Game.H1Z1Kotk:
                            RefreshDatabase(itemContext, itemContext.H1Z1KOTKItems, new H1Z1KOTKItem { Name = item.Name, Value = item.Value }, _logger);
                            break;
                        case Game.H1Z1JS:
                            RefreshDatabase(itemContext, itemContext.H1Z1JSItems, new H1Z1JSItem { Name = item.Name, Value = item.Value }, _logger);
                            break;
                        case Game.Payday2:
                            RefreshDatabase(itemContext, itemContext.Payday2Items, new Payday2Item { Name = item.Name, Value = item.Value }, _logger);
                            break;
                        case Game.Rust:
                            RefreshDatabase(itemContext, itemContext.RustItems, new RustItem { Name = item.Name, Value = item.Value }, _logger);
                            break;
                        case Game.TF2:
                            RefreshDatabase(itemContext, itemContext.TF2Items, new TF2Item { Name = item.Name, Value = item.Value }, _logger);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(_game), _game, null);
                    }
                }
            }
        }

        private static void RefreshDatabase<TItemType>(ItemContext _itemContext, DbSet<TItemType> _dbSetItems, TItemType _itemToAdd, ILogger _logger) where TItemType : Item
        {
            _logger.LogWarning("refreshdatabase: " + _itemToAdd.Name + " " + _itemToAdd.Value);

            _itemContext.Database.EnsureCreated();

            _logger.LogWarning("Databsae exists");

            if (!_dbSetItems.Any(_item => _item.Name.Equals(_itemToAdd.Name)))
            {
                _logger.LogWarning("add item to database");
                _dbSetItems.Add(_itemToAdd);
            }
            else
            {
                var test = _itemContext.Entry(_itemToAdd).Property(_t => _t.Value).OriginalValue;

                if (_itemContext.Entry(_itemToAdd).Property(_t => _t.Value).OriginalValue != _itemToAdd.Value)
                {
                    _itemContext.Entry(_itemToAdd).Property(_t => _t.Value).CurrentValue = _itemToAdd.Value;
                    _dbSetItems.Update(_itemToAdd);
                }
                var test2 = _itemContext.Entry(_itemToAdd).Property(_t => _t.Value).OriginalValue;

            }
            _logger.LogWarning("Save Changes");
            _itemContext.SaveChanges();
        }

        /// <summary>
        /// Check all items in the first list and add the item with the higher value to the list
        /// If the second list doesn't have the same item as list one, then just add it to the list with the current price
        /// If the second list has some Items left and the first list is empty, just add them to the outgoing list
        /// 
        /// Check any further list, which exists in the objectarray and put its items into the itemslist which will be given back
        /// </summary>
        //private static void CompareItemPrices(out List<Item> _items, ILogger _logger, params object[] _lists)
        //{
        //    _items = new List<Item>();
        //
        //    List<Item> listOne = _lists[0] as List<Item>;
        //    List<Item> listTwo = _lists[1] as List<Item>;
        //
        //    if(listOne != null && listTwo != null)
        //    {
        //        foreach(Item item in listOne)
        //        {
        //            Item foundSameItem = listTwo.Find(_item => _item.Name.ToLower().Equals(item.Name.ToLower()));
        //
        //            if (foundSameItem != null)
        //            {
        //                _items.Add(ComparePrice(item.Value, foundSameItem.Value) ? new Item { Name = item.Name, Value = item.Value } : new Item { Name = foundSameItem.Name, Value = foundSameItem.Value });
        //            }
        //            else
        //            {
        //                _items.Add(new Item { Name = item.Name, Value = item.Value });
        //            }
        //
        //            listTwo.Remove(foundSameItem);
        //        }
        //
        //        listOne.Clear();
        //
        //        if (listTwo.Count > 0)
        //        {
        //            _items.AddRange(listTwo.Select(_item => new Item { Name = _item.Name, Value = _item.Value }));
        //            listTwo.Clear();
        //        }
        //
        //        if(_lists.Length > 2)
        //        {
        //            for(int i = 2; i < _lists.Length; i++)
        //            {
        //                List<Item> listToCheck = _lists[i] as List<Item>;
        //
        //                if(listToCheck != null)
        //                {
        //                    foreach (Item item in _items)
        //                    {
        //                        Item foundSameItem = listToCheck.Find(_item => _item.Name.ToLower().Equals(item.Name.ToLower()));
        //
        //                        if (foundSameItem != null)
        //                        {
        //                            if(ComparePrice(foundSameItem.Value, item.Value))
        //                            {
        //                                item.Value = foundSameItem.Value;
        //                            }
        //
        //                            listToCheck.Remove(foundSameItem);
        //                        }
        //                    }
        //
        //                    listToCheck.RemoveAll(_item => _item.Name.ToLower().Contains("holo/foil"));
        //                    listToCheck.RemoveAll(_item => _item.Name.ToLower().Contains("music kit"));
        //                    listToCheck.RemoveAll(_item => _item.Name.ToLower().Contains("sticker"));
        //
        //                    if(listToCheck.Count > 0)
        //                    {
        //                        _items.AddRange(listToCheck.Select(_item => new Item { Name = _item.Name, Value = _item.Value }));
        //                    }
        //
        //                    listToCheck.Clear();
        //                }
        //            }
        //        }
        //    }
        //}

        private static void CompareItemPrices(out List<Item> _items, ILogger _logger, params object[] _lists)
        {
            _items = new List<Item>();

            if (_lists.Length > 0)
            {
                _logger.LogWarning("We have more than 0 lists to check");

                for (int i = 0; i < _lists.Length; i++)
                {
                    List<Item> listToCheck = _lists[i] as List<Item>;

                    if (listToCheck != null)
                    {
                        _logger.LogWarning("list " + i + " isn't null lets check it");

                        foreach (Item item in _items)
                        {
                            Item foundSameItem = listToCheck.Find(_item => _item.Name.ToLower().Equals(item.Name.ToLower()));

                            if (foundSameItem != null)
                            {
                                _logger.LogWarning("We've found the same item, take the higher price");

                                if (ComparePrice(foundSameItem.Value, item.Value))
                                {
                                    item.Value = foundSameItem.Value;
                                }

                                listToCheck.Remove(foundSameItem);
                            }
                        }

                        if (listToCheck.Count > 0)
                        {
                            _items.AddRange(listToCheck.Select(_item => new Item { Name = _item.Name, Value = _item.Value }));
                        }

                        _logger.LogWarning("Add the remaining items, we have not found and clear the list we have checked");

                        listToCheck.Clear();
                    }
                }
            }
        }

        /// <summary>
        /// compare the itemsvalue and return if the first item is more expensive
        /// </summary>
        /// <param name="_valueOne"></param>
        /// <param name="_valueTwo"></param>
        /// <returns></returns>
        private static bool ComparePrice(float _valueOne, float _valueTwo)
        {
            return _valueOne >= _valueTwo;
        }
    }
}