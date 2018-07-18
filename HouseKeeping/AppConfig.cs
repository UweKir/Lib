using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;


namespace DataManager.HouseKeeping
{
    public static class AppConfig
    {
        public static string PathList
        {
            get { return ConfigurationManager.AppSettings["PathList"]; }
        }

        public static string AgeDaysToZip
        {
            get { return ConfigurationManager.AppSettings["AgeDaysToZip"]; }
        }

        public static string AgeDaysToDelete
        {
            get { return ConfigurationManager.AppSettings["AgeDaysToDelete"]; }
        }

    }
}
