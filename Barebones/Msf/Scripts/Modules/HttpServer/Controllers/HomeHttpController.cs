using System.Text;
using WebSocketSharp.Net;

namespace Barebones.MasterServer
{
    public class HomeHttpController : HttpController
    {
        public override void Initialize(HttpServerModule server)
        {
            base.Initialize(server);

            server.RegisterHttpRequestHandler("home", OnHomeHttpRequestHandler);
        }

        private void OnHomeHttpRequestHandler(HttpListenerRequest request, HttpListenerResponse response)
        {
            byte[] contents = GetHtmlBytes();

            response.ContentType = "text/html";
            response.ContentEncoding = Encoding.UTF8;
            response.ContentLength64 = contents.LongLength;
            response.Close(contents, true);
        }
    }
}