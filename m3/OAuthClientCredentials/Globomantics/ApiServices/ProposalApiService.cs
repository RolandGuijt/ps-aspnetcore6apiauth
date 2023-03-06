using Globomantics.Shared;

namespace Globomantics.ApiServices
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
            await _Client.EnsureAccessTokenInHeader();
            return await _Client.GetFromJsonAsync<IEnumerable<ProposalModel>>($"/proposal/all/{conferenceId}");
        }

        public async Task Add(ProposalModel model)
        {
            await _Client.EnsureAccessTokenInHeader();
            await _Client.PostAsJsonAsync("proposal", model);
        }

        public async Task<ProposalModel?> Approve(int proposalId)
        {
            await _Client.EnsureAccessTokenInHeader();
            var resp =
                await _Client.PutAsync($"/proposal/approve/{proposalId}", null);
            return await resp.Content.ReadFromJsonAsync<ProposalModel>();
        }
    }
}
