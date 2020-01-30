using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SIS.HTTP
{
    public class HttpRequest
    {
        public HttpRequest(string httpRquestAsString)
        {
            this.Cookies = new List<Cookie>();

            this.Headers = new List<Header>();

            var lines = httpRquestAsString
                .Split(new string[] { HttpConstants.NewLine }, StringSplitOptions.None);

            var httpInfoHeader = lines[0];

            var infoHeaderParts = httpInfoHeader.Split(' ');

            if(infoHeaderParts.Length != 3)
            {
                throw new HttpServerException("Invalid Http Header line.");
            }

            var httpMethod = infoHeaderParts[0];
            switch (httpMethod)
            {
                case "POST": this.Method = HttpMethodType.Post;break;
                case "GET": this.Method = HttpMethodType.Get; break;
                case "PUT": this.Method = HttpMethodType.Put; break;
                case "DELETE": this.Method = HttpMethodType.Delete; break;
            };

            this.Path = infoHeaderParts[1];

            var versionType = infoHeaderParts[2];

            switch (versionType)
            {
                case "HTTP/1.0": this.Version = HttpVersionType.Http10; break;
                case "HTTP/1.1": this.Version = HttpVersionType.Http11; break;
                case "HTTP/2.0": this.Version = HttpVersionType.Http20; break;
            };

            bool isInHeader = true;

            StringBuilder bodyBuilder = new StringBuilder();

            for(int i= 1; i<lines.Length; i++)
            {
                var line = lines[i];

                if (string.IsNullOrWhiteSpace(line))
                {
                    isInHeader = false;
                    continue;
                }

                if (isInHeader)
                {
                    var headerParts = line.Split(new string[] { ": " },2,StringSplitOptions.None);
                    if(headerParts.Length != 2)
                    {
                        throw new HttpServerException($"Invalid header: {line}");
                    }

                    Header currentHeader = new Header(headerParts[0],headerParts[1]);
                    this.Headers.Add(currentHeader);

                    if(headerParts[0] == "Cookie")
                    {
                        var cookiesAsString = headerParts[1];
                        var cookies = cookiesAsString.Split(new string[] { "; " }, StringSplitOptions.RemoveEmptyEntries);

                        foreach(var cookieAsString in cookies)
                        {
                            var coookieParts = cookieAsString.Split(new char[] { '=' }, 2);

                            if(coookieParts.Length == 2)
                            {
                                this.Cookies.Add(new Cookie(coookieParts[0], coookieParts[1]));
                            }
                        }
                    }
                }
                else
                {
                    bodyBuilder.AppendLine(line);
                }
            }

            this.Body = bodyBuilder.ToString();

        }
        public HttpMethodType Method { get; set; }
        public string Path { get; set; }

        public HttpVersionType Version { get; set; }

        public IList<Header> Headers { get; set; }

        public IList<Cookie> Cookies { get; set; }

        public string Body { get; set; }

        public IDictionary<string,string> SessionData { get; set; }
    }
}
