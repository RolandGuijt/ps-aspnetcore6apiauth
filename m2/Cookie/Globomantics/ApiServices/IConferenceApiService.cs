using Globomantics.Client.Models;

namespace Globomantics.Client.ApiServices
{
    public interface IConferenceApiService
    {
        Task Add(ConferenceModel model);
        Task<IEnumerable<ConferenceModel>?> GetAll();
        Task<ConferenceModel?> GetById(int id);
    }
}