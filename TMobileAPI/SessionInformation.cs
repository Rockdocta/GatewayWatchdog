namespace TMobileAPI
{
    public class SessionInformation
    {
        public string GatewayUrl { get; set; }
        public string Token { get; set; }

        public DateTime ExpireDateTime { get; set; }
    }
}