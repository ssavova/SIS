using SIS.HTTP;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.MvcFramework
{
    public abstract class HttpMethodAttribute : Attribute
    {
        public string Url { get; }

        protected HttpMethodAttribute()
        {
                
        }

        protected HttpMethodAttribute(string url)
        {
            this.Url = url;
        }

        public abstract HttpMethodType Type { get; }
    }
}
