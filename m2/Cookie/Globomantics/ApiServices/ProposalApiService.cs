using Globomantics.Client.Models;

namespace Globomantics.Client.ApiServices
{
    public class ProposalApiService : IProposalApiService
    {
        private readonly HttpClient _Client;

        public ProposalApiService(HttpClient client)
        {
            _Client = client;
        }

        public async Task<IEnumerable<ProposalModel>?> GetAll(int conferenceId)
        {
            return await _Client.GetFromJsonAsync<IEnumerable<ProposalModel>>($"/api/proposal/all/{conferenceId}");
        }

        public async Task Add(ProposalModel model)
        {
            await _Client.PostAsJsonAsync("/api/proposal", model);
        }

        public async Task<ProposalModel?> Approve(int proposalId)
        {
            var resp =
                await _Client.PutAsync($"/api/proposal/approve/{proposalId}", null);
            return await resp.Content.ReadFromJsonAsync<ProposalModel>();
        }
    }
}
