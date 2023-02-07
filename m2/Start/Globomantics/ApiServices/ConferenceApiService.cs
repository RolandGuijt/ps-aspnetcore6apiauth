using Globomantics.Client.Models;

namespace Globomantics.Client.ApiServices
{
    public class ConferenceApiService : IConferenceApiService
    {
        private readonly HttpClient _Client;

        public ConferenceApiService(HttpClient client)
        {
            _Client = client;
        }

        public async Task<IEnumerable<ConferenceModel>?> GetAll()
        {
            return await _Client
                .GetFromJsonAsync<IEnumerable<ConferenceModel>>("/conference");
        }

        public async Task<ConferenceModel?> GetById(int id)
        {
            return await _Client
                .GetFromJsonAsync<ConferenceModel>($"/conference/{id}");
        }

        public async Task Add(ConferenceModel model)
        {
            await _Client.PostAsJsonAsync("/conference", model);
        }
    }
}
