using SIS.HTTP.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SIS.HTTP
{
    public class HttpServer : IHttpServer
    {
        private readonly TcpListener tcpListener;
        private readonly IList<Route> routeTable;
        private readonly IDictionary<string, IDictionary<string, string>> sessions;
        private readonly ILogger logger;
            
        //TODO: actions to pass on the constructor
        public HttpServer(int port, IList<Route> routingTable, ILogger logger)
        {
            this.tcpListener = new TcpListener(IPAddress.Loopback, port);
            this.routeTable = routingTable;
            this.sessions = new Dictionary<string, IDictionary<string, string>>();
            this.logger = logger;
        }



        public async Task ResetAsync()
        {
            this.Stop();
            await this.StartAsync();
        }

        public async Task StartAsync()
        {
            this.tcpListener.Start();

            while (true)
            {
                TcpClient tcpClient = await tcpListener.AcceptTcpClientAsync();
                Task.Run(() => ProcessClientAsync(tcpClient));
            }
        }

        public void Stop()
        {
            this.tcpListener.Stop();
        }

        private async Task ProcessClientAsync(TcpClient client)
        {
            using NetworkStream networkStream = client.GetStream();
            try
            {
                const string newLine = HttpConstants.NewLine;
                {
                    //TODO: use buffer
                    byte[] requestBytes = new byte[10000000];
                    int readedBytes = await networkStream.ReadAsync(requestBytes, 0, requestBytes.Length);
                    string requestasString = Encoding.UTF8.GetString(requestBytes, 0, readedBytes);

                    HttpRequest request = new HttpRequest(requestasString);
                    string newSessionId = null;
                    var sessionCookie = request.Cookies.FirstOrDefault(x => x.Name == HttpConstants.SessionIdCookieName);

                    if (sessionCookie != null && this.sessions.ContainsKey(sessionCookie.Value))
                    {
                        request.SessionData = this.sessions[sessionCookie.Value]; 
                    }
                    else
                    {
                        newSessionId = Guid.NewGuid().ToString();
                        var dictionary = new Dictionary<string, string>();
                        this.sessions.Add(newSessionId, dictionary);
                        request.SessionData = dictionary;
                    }

                    this.logger.Log($"{request.Method} {request.Path}");

                    var route = this.routeTable.FirstOrDefault(x => x.HttpMethod == request.Method && string.Compare(x.Path,request.Path,true) == 0);

                    HttpResponse response;
                    if (route == null)
                    {
                        response = new HttpResponse(ResponseCodeEnumeration.NotFound, new byte[0]);
                    }
                    else
                    {
                        response = route.Action(request);
                    }

                    

                    if (newSessionId != null)
                    {
                        response.Cookies.Add(new ResponseCookie(HttpConstants.SessionIdCookieName,newSessionId )
                        { HttpOnly = true, MaxAge =30*3600 });
                    }
                    

                    byte[] responseBytes = Encoding.UTF8.GetBytes(response.ToString());
                    await networkStream.WriteAsync(responseBytes, 0, responseBytes.Length);
                    await networkStream.WriteAsync(response.Body, 0, response.Body.Length);
                }
            }
            catch (Exception ex)
            {
                var errorResponse = new HttpResponse(ResponseCodeEnumeration.InternalServerError, Encoding.UTF8.GetBytes(ex.Message));

                errorResponse.Headers.Add(new Header("Content-type", "text/html"));

                byte[] responseBytes = Encoding.UTF8.GetBytes(errorResponse.ToString());
                await networkStream.WriteAsync(responseBytes, 0, responseBytes.Length);
                await networkStream.WriteAsync(errorResponse.Body, 0, errorResponse.Body.Length);
            }


        }
    }
}

