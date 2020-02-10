using SIS.HTTP;
using SIS.MvcFramework;
using SulsApp.Controllers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SulsApp
{
    public class StartUp : IMvcApplication
    {
        public void ConfigureServices()
        {
            var context = new SulsDbContext();
            context.Database.EnsureCreated();
        }

        public void Configure(IList<Route> routeTable)
        {

        }
    }
}
