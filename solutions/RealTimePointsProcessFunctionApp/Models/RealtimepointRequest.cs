using System;
using System.Collections.Generic;
using System.Text;

namespace RealTimePointsProcessFunctionApp.Models
{
    public class RealtimepointRequest
    {
        public string EventName { get; set; }
        public string EventId { get; set; }
        public string AccountID { get; set; }
        public string WalletID { get; set; }

    }
}
