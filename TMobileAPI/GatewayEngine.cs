using GatewayWatchdog.Models;
using Newtonsoft.Json;

namespace TMobileAPI
{

    public class GatewayEngine
    {
        public ClientsRoot GetDevices(SessionInformation session)
        {
            return GetTelemetryData<ClientsRoot>(session);
        }
        public CellRoot GetCells(SessionInformation session)
        {
            return GetTelemetryData<CellRoot>(session);
        }

        public TelemetryData GetAll(SessionInformation session)
        {
            return GetTelemetryData<TelemetryData>(session);    
        }
        public SimRoot GetSimInformation(SessionInformation session)
        {
            return GetTelemetryData<SimRoot>(session);
           
        }
        public Root GetGatewayInformation(string gatewayUrl)
        {
            var telemetryRequest = new HttpRequestMessage(HttpMethod.Get, $"{gatewayUrl}/TMI/v1/gateway?get=all");


            HttpClient client = new()
            {
                Timeout = TimeSpan.FromSeconds(5)
            };
            var result = client.Send(telemetryRequest);
            return JsonConvert.DeserializeObject<Root>(result.Content.ReadAsStringAsync().Result);
            
        }

        private T GetTelemetryData<T>(SessionInformation session)
        {
                    
            var telemetryRequest = new HttpRequestMessage(HttpMethod.Get, $"{session.GatewayUrl}/TMI/v1/network/telemetry?get=all");
            telemetryRequest.Headers.Add("Authorization", $"Bearer {session.Token}");

            HttpClient client = new()
            {
                Timeout = TimeSpan.FromSeconds(5)
            };

            var telemetryResponse = client.Send(telemetryRequest);
            var telemetryJson = telemetryResponse.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<T>(telemetryJson);                
           
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