using MongoDB.Driver;


namespace GigHunter.DomainModels
{
	public class RepositoryBase
	{
		public static string ConnectionString => Properties.Settings.Default.ConnectionString;
		public static string DatabaseName => Properties.Settings.Default.Database;
		public IMongoDatabase mongoDatabase;

		public RepositoryBase()
		{
			var client = new MongoClient(ConnectionString);
			mongoDatabase = client.GetDatabase(DatabaseName);
		}
	}
}
