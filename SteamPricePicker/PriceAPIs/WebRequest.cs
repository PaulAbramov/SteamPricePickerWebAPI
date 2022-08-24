using System.Net.Http;
using System.Threading.Tasks;

namespace SteamPriceAPI.PriceAPIs.WebRequests
{
    public class WebRequest
    {
        public static async Task<string> SendRequest(string _url)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(_url);

                    response.EnsureSuccessStatusCode();

                    var stringResponse = await response.Content.ReadAsStringAsync();

                    return stringResponse;
                }
                catch (HttpRequestException e)
                {
                    string error = e.Message;
                    throw;
                }
            }
        }
    }
}
