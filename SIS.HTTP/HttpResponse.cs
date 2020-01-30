using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.HTTP
{
    public class HttpResponse
    {
        public HttpResponse(ResponseCodeEnumeration statusCode, byte[] body)
        {
            this.Cookies = new List<ResponseCookie>();
            this.Version = HttpVersionType.Http10;
            this.Headers = new List<Header>();
            this.StatusCode = statusCode;
            this.Body = body;

            if (body?.Length > 0)
            {
                this.Headers.Add(new Header("Content-Lenght", body.Length.ToString()));
            }
                
        }

        internal HttpResponse()
        {
            this.Version = HttpVersionType.Http10;
            this.Cookies = new List<ResponseCookie>();
            this.Headers = new List<Header>();
        }
        public HttpVersionType Version { get; set; }

        public ResponseCodeEnumeration StatusCode { get; set; }

        public IList<Header> Headers { get; set; }

        public IList<ResponseCookie> Cookies { get; set; }
        public byte[] Body { get; set; }
        public override string ToString()
        {
            StringBuilder responseAsString = new StringBuilder();

            var httpVersionAsString = this.Version switch
            {
                HttpVersionType.Http10 => "HTTP/1.0",
                HttpVersionType.Http11 => "HTTP/1.1",
                HttpVersionType.Http20 => "HTTP/2.0",
                _=> "HTTP/1.1"
            };

            responseAsString.Append($"{httpVersionAsString} {(int)this.StatusCode} {this.StatusCode}" + HttpConstants.NewLine);

            foreach(Header header in this.Headers)
            {
                responseAsString.Append($"{header.ToString()}" + HttpConstants.NewLine);
            }

            foreach(Cookie cookie in this.Cookies)
            {
                responseAsString.Append($"Set-Cookie: " + cookie.ToString() + HttpConstants.NewLine);
            }

            responseAsString.Append(HttpConstants.NewLine);
            return responseAsString.ToString();
        }
    }
}
