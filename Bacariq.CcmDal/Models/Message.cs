using System;
using System.Collections.Generic;
using System.Text;

namespace Bacariq.CcmDal.Models
{
    public class Messages
    {
        public string PkGuid { get; set; }
        public string Message{ get; set; }
    }
    public class MessagesApi
    {
        public string Action { get; set; }
        public string Data { get; set; }
    }
}
