using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace AdvancedDevSample.Application.Exceptions
{
    public class ApplicationServiceException : Exception
    {
        public HttpStatusCode StatusCode { get; }

        public ApplicationServiceException(string message, HttpStatusCode statusCode): base(message)
        {
            StatusCode = statusCode;
        }
    }
}
