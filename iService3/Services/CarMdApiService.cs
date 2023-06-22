using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iService3.Models;
using Newtonsoft.Json;

namespace iService3.Services
{
    public class CarMdApiService
    {
        public static async Task<VinDecodeResponse> DecodeVin(string vin)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"http://api.carmd.com/v3.0/decode?vin={vin}"),
                Headers =
                {
                    {"authorization", "Basic MmI3MzEzOGQtNDZmYy00N2UxLTk5ODMtOWI5Y2RiMmNkNGMw"},
                    {"partner-token", "3e4aa838e3594ad2ad9a453c779dce95"}
                }
            };

            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<VinDecodeResponse>(responseBody);
            }
        }
    }
}
