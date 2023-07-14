using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace SEG.CustomerWebService.Core.CustomExceptions
{
    public class NotFoundException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }

        public NotFoundException(string message, HttpStatusCode statusCode) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
