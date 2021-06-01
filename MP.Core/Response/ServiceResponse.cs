using System;
using System.Collections.Generic;
using System.Text;

namespace MP.Core.Response
{
    public class ServiceResponse<T>
    {
        public bool Success { get; }
        public Error Error { get; }
        public T Content { get; }

        public ServiceResponse(T content)
        {
            Success = true;
            Content = content;
        }

        public ServiceResponse(Error error)
        {
            Success = false;
            Error = error;
        }
    }
}
