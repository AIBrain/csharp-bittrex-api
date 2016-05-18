namespace Bittrex
{
    using System;

    class ApiCallResponse<T>
    {
        public Boolean success { get; set; }
        public String message { get; set; }
        public T result { get; set; }
    }
}
