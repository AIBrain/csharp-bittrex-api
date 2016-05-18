using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bittrex
{
    class ApiCallResponse<T>
    {
        public Boolean success { get; set; }
        public String message { get; set; }
        public T result { get; set; }
    }
}
