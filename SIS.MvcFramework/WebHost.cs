using SIS.HTTP;
using SIS.HTTP.Logging;
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
            IList<Route> routeTable = new List<Route>();

            IServiceCollection serviceCollection = new ServiceCollection();

            serviceCollection.Add<ILogger, ConsoleLogger>();

            app.ConfigureServices(serviceCollection);
            app.Configure(routeTable);
            AutoRegisterStaticFilesRoutes(routeTable);
            AutoRegisterActionRoutes(routeTable, app, serviceCollection);
            ILogger logger = serviceCollection.CreateInstance<ILogger>();

            logger.Log("Registered routes: ");
            foreach(var route in routeTable)
            {
                logger.Log(route.ToString());
            }

            logger.Log(string.Empty);
            logger.Log("Requests: ");
            HttpServer server = new HttpServer(80, routeTable,logger);
            await server.StartAsync();
        }

        private static void AutoRegisterActionRoutes(IList<Route> routeTable, IMvcApplication app, IServiceCollection serviceCollection)
        {
            var controllers = app.GetType().Assembly.GetTypes().Where(type => type.IsSubclassOf(typeof(Controller)) && !type.IsAbstract);

            foreach(var controller in controllers)
            {
                
                var actions = controller.GetMethods().Where(m => !m.IsSpecialName && !m.IsConstructor && m.IsPublic && m.DeclaringType == controller);
                foreach(var action in actions)
                {
                    string url = "/" + controller.Name.Replace("Controller", string.Empty) + "/" + action.Name;
                    var attribute = action.GetCustomAttributes()
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

                    routeTable.Add(new Route(httpActiontype, url, (request) => InvokeAction(request, serviceCollection,controller,action)));

                }
            }
        }

        private static HttpResponse InvokeAction(HttpRequest request,IServiceCollection serviceCollection, Type controllerType, MethodInfo actionMethod)
        {
                var controller = serviceCollection.CreateInstance(controllerType) as Controller;
                controller.Request = request;
                var response = actionMethod.Invoke(controller, new object[] { }) as HttpResponse;
                return response;
        }
        private static void AutoRegisterStaticFilesRoutes(IList<Route> routeTable)
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
