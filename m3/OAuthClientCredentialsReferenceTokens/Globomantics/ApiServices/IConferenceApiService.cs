using Globomantics.Shared;

namespace Globomantics.ApiServices
{
    public interface IConferenceApiService
    {
        Task Add(ConferenceModel model);
        Task<IEnumerable<ConferenceModel>?> GetAll();
        Task<ConferenceModel?> GetById(int id);
    }
}