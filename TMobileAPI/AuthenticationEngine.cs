using GatewayWatchdog.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TMobileAPI
{
    public class AuthenticationEngine
    {

        public string GatewayUrl { get; set; }

        public AuthenticationEngine()
        {

        }
        public AuthenticationEngine(string gatewayUrl)
        {
            GatewayUrl = gatewayUrl;
        }


        public SessionInformation Authenticate(string username, string password)
        {
            var content = new StringContent($"{{ \"username\": \"{username}\", \"password\": \"{password}\"}}");

            HttpClient client = new()
            {
                Timeout = TimeSpan.FromSeconds(5)
            };

            var response = client.PostAsync($"{GatewayUrl}/TMI/v1/auth/login", content);

            var responseContent = response.Result.Content.ReadAsStringAsync().Result;
            var authResult = JsonConvert.DeserializeObject<AuthenticationRoot>(responseContent);

            if (authResult != null)
            {
                if (authResult.result != null)
                {
                    throw new Exception(authResult.result.message);
                }
            }
            

            return new SessionInformation
            {
                GatewayUrl = GatewayUrl,
                Token = authResult.auth.Token,
                ExpireDateTime = DateTimeOffset.FromUnixTimeSeconds(authResult.auth.Expiration).LocalDateTime
            };
        }
    }

   
}