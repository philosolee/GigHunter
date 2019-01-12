using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Caching;
using System.Web.Http;
using GigHunter.DomainModels.Models;
using GigHunter.DomainModels.Repositories;
using GigHunter.DomainModels.Utilities;
using MongoDB.Bson;

namespace GigHunter.Web.Api.Controllers
{
	public class SourceController : ApiController
	{
		private readonly IRepository<Source> _sourceRepository;

		public SourceController() : this(new SourceRepository())
		{
		}

		public SourceController(IRepository<Source> sourceRepository)
		{
			_sourceRepository = sourceRepository;
		}


		public IEnumerable<Source> GetAllSources()
		{
			return _sourceRepository.GetAll();
		}

		public HttpResponseMessage GetSource(string id)
		{
			if (!ObjectId.TryParse(id, out var objectId))
				return Request.CreateResponse(HttpStatusCode.BadRequest, $"Id {id} is not a valid Id");

			var source = _sourceRepository.GetById(objectId);

			return source != null
				? Request.CreateResponse(HttpStatusCode.Found, source)
				: Request.CreateResponse(HttpStatusCode.NotFound);
		}

		public HttpResponseMessage AddSource(Source source)
		{
			if (_sourceRepository.Exists(source.Id))
				return Request.CreateResponse(HttpStatusCode.BadRequest, $"Id {source.Id} already exists");

			_sourceRepository.Add(source);
			
			return Request.CreateResponse(HttpStatusCode.Created, source);
		}
	}
}
