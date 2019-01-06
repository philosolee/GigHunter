using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace GigHunter.Service.Core.Wrappers
{
	public interface IRestClientWrapper : IRestClient
	{
		void AddDefaultHeader(string name, string value);
	}
}
