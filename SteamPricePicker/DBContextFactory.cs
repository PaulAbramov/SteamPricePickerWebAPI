using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SteamPriceAPI.Contexts;

namespace SteamPricePicker
{
    public class DBContextFactory
    {
        public static Dictionary<string, string> m_ConnectionStrings { get; set; }

        public static void SetConnectionString(Dictionary<string, string> _connectionStrings)
        {
            m_ConnectionStrings = _connectionStrings;
        }

        public static ItemContext Create(string _connectionString)
        {
            if (!string.IsNullOrEmpty(_connectionString))
            {
                var connectionString = m_ConnectionStrings[_connectionString];
                var optionsBuilder = new DbContextOptionsBuilder<ItemContext>();
                optionsBuilder.UseSqlServer(connectionString);
                return new ItemContext(optionsBuilder.Options);
            }
            else
            {
                throw new ArgumentNullException();
            }
        }
    }
}
