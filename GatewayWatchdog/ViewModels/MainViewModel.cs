namespace GatewayWatchdog.ViewModels
{
    internal class MainViewModel :ViewModelBase
    {
		private string? _authToken;

		public string? AuthToken
		{
			get => _authToken;
			set
			{
				_authToken = value;
				PropChanged(nameof(AuthToken));
				
			}
		}


		


	}
}