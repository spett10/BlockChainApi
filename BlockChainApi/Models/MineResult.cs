using BlockChainSecurity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlockChainApi.Models
{
    public class MineResult
    {
        public string Message { get; set; }
        public int Index { get; set; }
        public List<Transaction> Transactions { get; set; }
        public int Proof { get; set; }
        public string PreviousHash { get; set; }
    }
}