using System;
using System.Collections.Generic;
using System.Text;

namespace Bacariq.CcmDal.Models
{
    public class JsonData
    {
        public string ACTION { get; set; }
        public string GUID_SRV_OMNIPRO { get; set; }
        public DateTime DTE_LAST_SYNCHRO { get; set; }
        public string FILENAME { get; set; }
        public string MSG { get; set; }
        public string VALUE { get; set; }
        public bool LOCKED { get; set; }
    }

}
