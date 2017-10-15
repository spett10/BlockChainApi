using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlockChainApi.Models
{
    public class TransactionRequest
    {
        public string Sender { get; set; }
        public string Recipient { get; set; }
        public int Amount { get; set; }
    }
}