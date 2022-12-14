using GatewayWatchdog.Models;
using Newtonsoft.Json;
using System.Net;

namespace TMobileAPI
{

    public class GatewayEngine
    {
        public async Task<ClientsRoot> GetDevices(SessionInformation session)
        {
            return await GetTelemetryData<ClientsRoot>(session);
        }
        public async Task<CellRoot> GetCells(SessionInformation session)
        {
            return await GetTelemetryData<CellRoot>(session);
        }

        public async Task<TelemetryData> GetAll(SessionInformation session)
        {
            return await GetTelemetryData<TelemetryData>(session);    
        }
        public async Task<SimRoot> GetSimInformation(SessionInformation session)
        {
            return await GetTelemetryData<SimRoot>(session);
           
        }
        public async Task<Root> GetGatewayInformation(string gatewayUrl)
        {
            var telemetryRequest = new HttpRequestMessage(HttpMethod.Get, $"{gatewayUrl}/TMI/v1/gateway?get=all");


            HttpClient client = new()
            {
                Timeout = TimeSpan.FromSeconds(5)
            };
            var result = await client.SendAsync(telemetryRequest);
            if (result.StatusCode != HttpStatusCode.OK)
            {
               throw new Exception("Error occurred while retrieving data: " + result.StatusCode + ": " + result.Content.ReadAsStringAsync().Result);
            }

            var json = await result.Content.ReadAsStringAsync();
            if (!json.Contains("\"manufacturer\": \"Arcadyan\""))
            {
                throw new Exception("Invalid source retrieved, check gateway IP address.");
            }
            
            return JsonConvert.DeserializeObject<Root>(await result.Content.ReadAsStringAsync());                      
            
        }

        private async Task<T> GetTelemetryData<T>(SessionInformation session)
        {

            var telemetryRequest = new HttpRequestMessage(HttpMethod.Get, $"{session.GatewayUrl}/TMI/v1/network/telemetry?get=all");
            telemetryRequest.Headers.Add("Authorization", $"Bearer {session.Token}");

            HttpClient client = new()
            {
                Timeout = TimeSpan.FromSeconds(5)
            };

            var telemetryResponse = await client.SendAsync(telemetryRequest);
            var telemetryJson = await telemetryResponse.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(telemetryJson);

        }
        public async Task<string> GetAccessPointData(SessionInformation session)
        {

            var telemetryRequest = new HttpRequestMessage(HttpMethod.Get, $"{session.GatewayUrl}/TMI/v1/network/configuration?get=ap");
            telemetryRequest.Headers.Add("Authorization", $"Bearer {session.Token}");

            HttpClient client = new()
            {
                Timeout = TimeSpan.FromSeconds(5)
            };

            var telemetryResponse = await client.SendAsync(telemetryRequest);
            var telemetryJson = await telemetryResponse.Content.ReadAsStringAsync();
            return telemetryJson;

        }


        public void Reboot(SessionInformation session)
        {
            HttpClient client = new HttpClient();
            var rebootRequest = new HttpRequestMessage(HttpMethod.Post, $"{session.GatewayUrl}/TMI/v1/gateway/reset?set=reboot");
            rebootRequest.Headers.Add("Authorization", $"Bearer {session.Token}");

            Task.Run(() => client.SendAsync(rebootRequest));
        }
    }
}