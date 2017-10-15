using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlockChainApi.Models
{
    public class RegisterNodeRequest
    {
        public List<string> Nodes { get; set; }
    }
}