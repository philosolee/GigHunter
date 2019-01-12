using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace GigHunter.DomainModels.Utilities
{
	public class Converters
	{
		public static ObjectId ToObjectId(string id)
		{
			return new ObjectId(id);
		}
	}
}
