using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChainSecurity
{
    public class Transaction
    {
        public string Sender { get; private set; }
        public string Recipient { get; private set; }
        public int Amount { get; private set; }

        public Transaction(string sender, string recipient, int amount)
        {
            this.Sender = sender;
            this.Recipient = recipient;
            this.Amount = amount;
        }
    }
}
