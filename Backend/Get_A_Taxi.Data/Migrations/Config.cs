using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Get_A_Taxi.Data.Migrations
{
    public static class Config
    {
        public static string ConnectionString { get; private set; }

        static Config()
        {
            if (String.IsNullOrEmpty(ConnectionString))
            {
                var sqlServerPath = @".\SQLEXPRESS";
                Config.SetConnectionString("GetATaxi", sqlServerPath);
            }
        }

        public static void SetConnectionString(string catalog, string serverPath)
        {
            ConnectionString = @"Data Source=" + serverPath + @";Initial Catalog=" + catalog + ";Integrated Security=True";
        }
    }
}
