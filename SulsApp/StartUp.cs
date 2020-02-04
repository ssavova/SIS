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

            routeTable.Add(new Route(HttpMethodType.Get, "/", new HomeController().Index));
            routeTable.Add(new Route(HttpMethodType.Get, "/css/bootstrap.min.css", new StaticFilesController().Bootstrap));
            routeTable.Add(new Route(HttpMethodType.Get, "/css/site.css", new StaticFilesController().SiteCss));
            routeTable.Add(new Route(HttpMethodType.Get, "/css/reset-css.css", new StaticFilesController().ResetCss));
            routeTable.Add(new Route(HttpMethodType.Get, "/Users/Login", new UsersController().Login));
            routeTable.Add(new Route(HttpMethodType.Get, "/Users/Register", new UsersController().Register));
        }
    }
}
