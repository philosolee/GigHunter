using RestSharp.Deserializers;

namespace GigHunter.Service.Core.ApiClients.Ents24
{
	public class AuthorisationResponse
	{
		[DeserializeAs(Name = "access_token")]
		public string AccessToken { get; set; }

		[DeserializeAs(Name = "token_type")]
		public string TokenType {get; set;}

		[DeserializeAs(Name = "Bearer")]
		public string Bearer { get; set; }

		[DeserializeAs(Name = "expires")]
		public long Expires { get; set; }

		[DeserializeAs(Name = "expires_in")]
		public long ExpiresIn { get; set; }
	}
}
