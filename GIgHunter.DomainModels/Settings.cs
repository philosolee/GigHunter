namespace GigHunter.DomainModels
{
	public class Settings
	{

		public string MongoServer { get; set; }
		public string MongoDatabase { get; set; }

		private static readonly Settings _instance = new Settings();

		private Settings()
		{

		}

		public static Settings GetInstance()
		{
			return _instance;
		}

	}
}
