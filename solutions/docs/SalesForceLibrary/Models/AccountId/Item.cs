using System;
using System.Collections.Generic;
using System.Text;

namespace SalesForceLibrary.Models.AccountId
{
    public class Item
    {
        public string Account_ID { get; set; }
        public string Wallet_ID { get; set; }
        public string State { get; set; }
        public string Campaign_ID { get; set; }
        public string Type { get; set; }
        public string ClientType { get; set; }
        public DateTime Valid_From { get; set; }
        public DateTime Valid_To { get; set; }
        public string Status { get; set; }

    }
}
