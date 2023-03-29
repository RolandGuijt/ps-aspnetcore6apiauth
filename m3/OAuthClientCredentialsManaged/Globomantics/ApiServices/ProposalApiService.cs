using Globomantics.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Globomantics.ApiServices
{
    public class ProposalApiService : IProposalApiService
    {
        private readonly HttpClient _Client;

        public ProposalApiService(IHttpClientFactory factory)
        {
            _Client = factory.CreateClient("globoapi");
        }

        public async Task<IEnumerable<ProposalModel>?> GetAll(int conferenceId)
        {
            return await _Client.GetFromJsonAsync<IEnumerable<ProposalModel>>($"/proposal/all/{conferenceId}");
        }

        public async Task Add(ProposalModel model)
        {
            await _Client.PostAsJsonAsync("proposal", model);
        }

        public async Task<ProposalModel?> Approve(int proposalId)
        {
            var resp =
                await _Client.PutAsync($"/proposal/approve/{proposalId}", null);
            return await resp.Content.ReadFromJsonAsync<ProposalModel>();
        }
    }
}
