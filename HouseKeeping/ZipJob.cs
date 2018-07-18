using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataManager.HouseKeeping
{
    public class ZipJob
    {
        /// <summary>
        ///  The maximum age (in days) of files to delete
        /// </summary>
        private int ageDaysToDelete = 0;
        public int AgeDaysToDelete
        {
            get { return ageDaysToDelete; }
            set { ageDaysToDelete = value; }
        }

        /// <summary>
        ///  The maximum age (in days) of folders to zip
        /// </summary>
        private int ageDaysToZip = 0;
        public int AgeDaysToZip
        {
            get { return ageDaysToZip; }
            set { ageDaysToZip = value; }
        }

        /// <summary>
        /// Path of folder
        /// </summary>
        public string RootPath { get; set; }
    }
}
