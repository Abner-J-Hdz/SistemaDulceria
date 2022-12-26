using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSistemaDulceria.Utils
{
    public class Response
    {
        public bool Ok { get; set; }

        public string Message { get; set; }

        public string Error { get; set; }

        public string ResponseParameter1 { get; set; }

        public string ResponseParameter2 { get; set; }
    }
}
