using System.Collections.Generic;
using GigHunter.DomainModels.Models;
using GigHunter.DomainModels.Repositories;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace GigHunter.Web.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class SourceController : ControllerBase
	{
		private readonly IRepository<Source> _sourceRepository;

		public SourceController()
		{
			_sourceRepository = new SourceRepository();
		}

		[HttpGet]
		public IEnumerable<Source> Get()
		{
			return _sourceRepository.GetAll();
		}

		[HttpGet("{id}")]
		public ActionResult<Source> Get(string id)
		{
			if (!ObjectId.TryParse(id, out var objectId))
				return BadRequest("Id must be a valid Mongo Id");

			var source = _sourceRepository.GetById(objectId.ToString());

			return source != null
				? (ActionResult<Source>) source
				: NotFound();
		}

		[HttpPost]
		public ActionResult Post([FromBody] Source source)
		{
			if (source.Id != null && _sourceRepository.Exists(source.Id))
				return BadRequest($"Source with Id of {source.Id} already exists");

			_sourceRepository.Add(source);
			return Created($"/api/source/{source.Id}", source);
		}
	}
}