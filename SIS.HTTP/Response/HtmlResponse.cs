using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.HTTP.Response
{
    public class HtmlResponse : HttpResponse
    {
        public HtmlResponse(string html) 
            : base()
        {
            this.Headers.Add(new Header("Server", "StefiServer/1.0"));
            this.Headers.Add(new Header("Content-Type", "text/html"));
            this.StatusCode = ResponseCodeEnumeration.Ok;
            byte[] contentBytes = Encoding.UTF8.GetBytes(html);
            this.Body = contentBytes;

            this.Headers.Add(new Header("Content-Length", this.Body.Length.ToString()));
        }


    }
}
