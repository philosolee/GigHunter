using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace GigHunter.DomainModels.Models
{
	public interface IModel
	{
		ObjectId Id { get; set; }
	}
}
