using SIS.HTTP;
using SIS.HTTP.Response;
using SIS.MvcFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SulsApp.Controllers
{
    public class UsersController : Controller
    {
        
        public HttpResponse Login()
        {
            return this.View();
        }

        [HttpPost("/Users/Login")]
        public HttpResponse DoLogin()
        {
            return this.View();
        }

        
        public HttpResponse Register()
        {
            return this.View();
           
        }

        [HttpPost("/Users/Register")]
        public HttpResponse DoRegister()
        {
            return this.View();
        }
    }
}
