using Globomantics.Client.Models;
using Globomantics.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Globomantics.Server.Controllers
{
    [ApiController]
    [Route("proposal")]
    public class ProposalController : Controller
    {
        private readonly IProposalRepository _Repo;

        public ProposalController(IProposalRepository repo)
        {
            _Repo = repo;
        }

        [HttpGet("all/{conferenceId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult GetAll(int conferenceId)
        {
            var proposals = _Repo.GetAllForConference(conferenceId);
            if (proposals == null || !proposals.Any())
            {
                return NoContent();
            }
            return Ok(proposals);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetById(int id)
        {
            var proposal = _Repo.GetOne(id);
            if (proposal == null)
                return NotFound();
            return Ok(proposal);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public IActionResult Add(ProposalModel model)
        {
            var id = _Repo.Add(model);
            return CreatedAtAction(nameof(GetById), new { id }, model);
        }

        [HttpPut("approve/{proposalId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Approve(int proposalId)
        {
            var prop = _Repo.Approve(proposalId);
            if (prop == null)
                return NotFound();
            return Ok(prop);
        }
    }
}
