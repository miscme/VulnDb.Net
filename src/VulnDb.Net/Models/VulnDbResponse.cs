#nullable enable
using System;
using System.Net.Http;

namespace VulnDb.Net.Models
{
    public class VulnDbResponse<T> where T:class?
    {
        private T Response { get;}
        private Exception? Exception { get; }

        public VulnDbResponse(T response, ErrorResponse? error)
        {
            Response = response;
            Exception = error != null ? buildException(error) : null;
        }
        
        public VulnDbResponse(T response, Exception exception)
        {
            Response = response;
            Exception = exception;
        }

        private Exception buildException(ErrorResponse error)
        {
            return new HttpRequestException($"{error.ErrorMessage}");
        }
    }
}