using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bacariq.CcmService.Models
{
    public class JSonExport
    {
        public string GuidClientOP { get; set; }
        public AgendaRdv AgendaRdv { get; set; }
        public Patients Patients { get; set; }
        public Personnes Boss { get; set; }
        public Personnes User { get; set; }
    }
}
