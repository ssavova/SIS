using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.HTTP.Response
{
    public class FileResponse : HttpResponse
    {
        public FileResponse(byte[] fileContent, string contentType)
        {
            this.Headers.Add(new Header("Server", "StefiServer/1.0"));
            this.Headers.Add(new Header("Content-Type", contentType));
            this.StatusCode = ResponseCodeEnumeration.Ok;
            this.Body = fileContent;

            this.Headers.Add(new Header("Content-Length", this.Body.Length.ToString()));
        }
    }
}
