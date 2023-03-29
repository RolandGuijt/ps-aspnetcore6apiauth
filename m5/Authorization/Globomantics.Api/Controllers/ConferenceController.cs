using Globomantics.Api.Repositories;
using Globomantics.Shared;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Globomantics.Api.Controllers
{
    [ApiController]
    [Route("conference")]
    public class ConferenceController : Controller
    {
        private readonly IConferenceRepository _Repo;
        private readonly IAuthorizationService _AuthService;

        public ConferenceController(IConferenceRepository repo, 
            IAuthorizationService authService)
        {
            _Repo = repo;
            _AuthService = authService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = "fullaccess")]
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
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _AuthService
                .AuthorizeAsync(User, "isadmin");
            if (result.Succeeded)
            {

            }
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
