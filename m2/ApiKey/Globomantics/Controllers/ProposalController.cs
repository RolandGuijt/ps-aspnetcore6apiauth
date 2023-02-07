using Globomantics.Client.ApiServices;
using Globomantics.Client.Models;
using Microsoft.AspNetCore.Mvc;

namespace Globomantics.Controllers;
public class ProposalController : Controller
{
    private readonly IConferenceApiService _ConferenceApiService;
    private readonly IProposalApiService _ProposalApiService;

    public ProposalController(IConferenceApiService conferenceApiService, IProposalApiService proposalApiService)
    {
        _ConferenceApiService = conferenceApiService;
        _ProposalApiService = proposalApiService;
    }

    public async Task<IActionResult> Index(int conferenceId)
    {
        var conference = await _ConferenceApiService.GetById(conferenceId);
        ViewBag.Title = $"Speaker - Proposals For Conference {conference.Name} {conference.Location}";
        ViewBag.ConferenceId = conferenceId;

        return View(await _ProposalApiService.GetAll(conferenceId));
    }

    public IActionResult AddProposal(int conferenceId)
    {
        ViewBag.Title = "Speaker - Add Proposal";
        return View(new ProposalModel { ConferenceId = conferenceId });
    }

    [HttpPost]
    public async Task<IActionResult> AddProposal(ProposalModel proposal)
    {
        if (ModelState.IsValid)
            await _ProposalApiService.Add(proposal);
        return RedirectToAction("Index", new { conferenceId = proposal.ConferenceId });
    }

    public async Task<IActionResult> Approve(int proposalId)
    {
        var proposal = await _ProposalApiService.Approve(proposalId);
        return RedirectToAction("Index", new { conferenceId = proposal.ConferenceId });
    }
}
