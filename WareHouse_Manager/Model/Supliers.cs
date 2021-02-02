using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WareHouse_Manager.Model
{
    public class Supliers
    {
        public int ID { get; set; }
        public int STT { get; set; }
        public string NAME { get; set; }
        public string ADDRESS { get; set; }
        public string PHONE { get; set; }
        public string EMAIL { get; set; }
        public string MORE_INFO { get; set; }
        public DateTime CONSTRACT_DATE { get; set; }
    }
}
