using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.MvcFramework
{
    public interface IViewEngine
    {
        string GetHtml(string templateHTML, object model, string user);
    }
}
