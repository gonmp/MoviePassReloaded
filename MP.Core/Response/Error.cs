using System;
using System.Collections.Generic;
using System.Text;

namespace MP.Core.Response
{
    public class Error
    {
        public int Code { get; }
        public string Message { get; }

        public Error(int code, string message)
        {
            Code = code;
            Message = message;
        }
    }
}
