using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iService3.Tools
{
    internal class HttpConnectionServer
    {
        private static HttpClientHandler GetInsecureHandler()
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
            {
                if (cert.Issuer.Equals("CN=localhost"))
                    return true;
                return errors == System.Net.Security.SslPolicyErrors.None;
            };
            return handler;
        }

        public HttpClient GetHttpClient()
        {
            var httpClient = new HttpClient(GetInsecureHandler())
            {
                //BaseAddress = new Uri("https://iservice-api.azurewebsites.net")
                BaseAddress = new Uri("https://188.26.168.152:81/")
            };
            return httpClient;
        }
    }
}
