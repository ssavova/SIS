using SIS.HTTP;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SIS.MvcFramework
{
    public static class WebHost
    {
        public static async Task StartAsync(IMvcApplication app)
        {
            var routeTable = new List<Route>();
            app.ConfigureServices();
            app.Configure(routeTable);

            HttpServer server = new HttpServer(80, routeTable);
            await server.StartAsync();
        }
    }
}
