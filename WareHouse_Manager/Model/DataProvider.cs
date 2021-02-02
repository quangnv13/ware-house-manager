using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WareHouse_Manager.Model
{
    public class DataProvider
    {
        private static DataProvider _instance;
        public WAREHOUSE_MANAGEREntities DB;
        public static DataProvider Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new DataProvider();
                return _instance;
            }
            set
            {
                _instance = value;
            }
        }
        private DataProvider()
        {
            DB = new WAREHOUSE_MANAGEREntities();
        }
    }
}
