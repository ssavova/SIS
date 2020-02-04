using SIS.HTTP;
using SIS.HTTP.Response;
using SIS.MvcFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DemoApp
{
    public class StartUp : IMvcApplication
    {
        public void Configure(IList<Route> routeTable)
        {
            routeTable.Add(new Route(HttpMethodType.Get, "/", Index));
            routeTable.Add(new Route(HttpMethodType.Post, "/Tweets/Create", CreateTweet));
            routeTable.Add(new Route(HttpMethodType.Get, "/favicon.ico", FavIcon));
        }

        public void ConfigureServices()
        {
            var db = new ApplicationDbContext();
            db.Database.EnsureCreated();
        }

        private static HttpResponse CreateTweet(HttpRequest request)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            context.Tweets.Add(new Tweet()
            {
                CreatedOn = DateTime.UtcNow,
                Creator = request.FormData["creator"],
                Content = request.FormData["tweetName"]
            });

            context.SaveChanges();

            return new RedirectResponse("/");
        }

        private static HttpResponse FavIcon(HttpRequest request)
        {
            var byteContent = File.ReadAllBytes(@"C:\Users\HP\source\repos\SIS\DemoApp\wwwroot\favicon.ico");
            return new FileResponse(byteContent, "image/x-icon");
        }



        public static HttpResponse Index(HttpRequest request)
        {
            var username = request.SessionData.ContainsKey("Username") ? request.SessionData["Username"] : "Anonymous";

            ApplicationDbContext context = new ApplicationDbContext();
            var tweets = context.Tweets.Select(t => new
            {
                t.CreatedOn,
                t.Creator,
                t.Content
            }).ToList();

            StringBuilder html = new StringBuilder();

            html.Append("<table><tr><th>Date</th><th>Creator</th><th>Content</th></tr>");
            foreach (var tweet in tweets)
            {
                html.Append($"<tr><td>{tweet.CreatedOn}</td><td>{tweet.Creator}</td><td>{tweet.Content}</td></tr>");
            }
            html.Append("</table>");
            html.Append($"<form action='/Tweets/Create' method='post'><input name='creator'/><br /><textarea name='tweetName'></textarea><br /><input type='submit'/></form>");

            return new HtmlResponse(html.ToString());
        }


    }
}
