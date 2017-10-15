using BlockChainSecurity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlockChainApi.Models
{
    public class ConsensusResponse
    {
        public string Message { get; set; }
        public List<Block> Chain { get; set; }
    }
}