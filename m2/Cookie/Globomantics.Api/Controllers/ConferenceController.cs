using Globomantics.Client.Models;
using Globomantics.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Globomantics.Server.Controllers
{
    [ApiController]
    [Route("/api/conference")]
    [Authorize]
    public class ConferenceApiController : Controller
    {
        private readonly IConferenceRepository _Repo;

        public ConferenceApiController(IConferenceRepository repo)
        {
            _Repo = repo;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult GetAll()
        {
            var conferences = _Repo.GetAll();
            if (conferences == null || !conferences.Any())
            {
                return NoContent();
            }
            return Ok(conferences);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetById(int id)
        {
            var conference = _Repo.GetById(id);
            if (conference == null)
            {
                return NotFound();
            }
            return Ok(conference);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public IActionResult Add(ConferenceModel model)
        {
            var id = _Repo.Add(model);
            return CreatedAtAction(nameof(GetById), new { id }, model);
        }
    }
}
