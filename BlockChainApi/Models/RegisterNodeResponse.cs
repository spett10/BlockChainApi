using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlockChainApi.Models
{
    public class RegisterNodeResponse
    {
        public string Message { get; set; }
        public List<string> TotalNodes { get; set; }
    }
}