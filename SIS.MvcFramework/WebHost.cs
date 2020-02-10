using SIS.HTTP;
using SIS.HTTP.Response;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
            AutoRegisterStaticFilesRoutes(routeTable);
            AutoRegisterActionRoutes(routeTable, app);

            Console.WriteLine("Registered routes: ");
            foreach(var route in routeTable)
            {
                Console.WriteLine(route);
            }

            Console.WriteLine();
            Console.WriteLine("Requests: ");
            HttpServer server = new HttpServer(80, routeTable);
            await server.StartAsync();
        }

        private static void AutoRegisterActionRoutes(List<Route> routeTable, IMvcApplication app)
        {
            var types = app.GetType().Assembly.GetTypes().Where(type => type.IsSubclassOf(typeof(Controller)) && !type.IsAbstract);

            foreach(var type in types)
            {
                Console.WriteLine(type.FullName);
                var methods = type.GetMethods().Where(m => !m.IsSpecialName && !m.IsConstructor && m.DeclaringType == type);
                foreach(var meth in methods)
                {
                    string url = "/" + type.Name.Replace("Controller", string.Empty) + "/" + meth.Name;
                    var attribute = meth.GetCustomAttributes()
                        .FirstOrDefault(x => x.GetType()
                        .IsSubclassOf(typeof(HttpMethodAttribute)))
                        as HttpMethodAttribute;

                    var httpActiontype = HttpMethodType.Get;

                    if(attribute != null)
                    {
                        httpActiontype = attribute.Type;
                        if (attribute.Url != null)
                        {
                            url = attribute.Url;
                        }
                    }

                    routeTable.Add(new Route(httpActiontype, url, (request) =>
                     {
                         var controller = Activator.CreateInstance(type) as Controller;
                         controller.Request = request;
                         var response = meth.Invoke(controller, new object[] { }) as HttpResponse;
                         return response;
                     }));

                    Console.WriteLine($"        {url}");
                }
            }
        }

        private static void AutoRegisterStaticFilesRoutes(List<Route> routeTable)
        {
            var staticFiles = Directory.GetFiles("wwwroot", "*", SearchOption.AllDirectories);
            foreach(var staticFile in staticFiles)
            {
                var path = staticFile.Replace("wwwroot", string.Empty).Replace("\\","/");
                routeTable.Add(new Route(HttpMethodType.Get, path, (request) =>
                {
                    var fileInfo = new FileInfo(staticFile);
                    var contentType = fileInfo.Extension switch
                    {
                        ".css" => "text/css",
                        ".js" => "text/javascript",
                        ".ico" => "image/x-icon",
                        ".jpg" => "image/jpeg",
                        ".jpeg" => "image/jpeg",
                        ".png" => "image/png",
                        ".gif" => "image/gif",
                        ".html" => "text/html",
                        _ => "text/plain",
                    };
                    return new FileResponse(File.ReadAllBytes(staticFile), contentType);
                }));
            }
            
        }
    }
}
