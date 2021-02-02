using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WareHouse_Manager.Model
{
    public class OutputDetails
    {
        public int ID { get; set; }
        public int OBJECT_ID { get; set; }
        public string NAME { get; set; }
        public string SUPLIER { get; set; }
        public string OBJECT_TYPE { get; set; }
        public string UNIT { get; set; }
        public int AMOUNT { get; set; }
        public int OUT_PRICE { get; set; }
    }
}
