using Globomantics.Shared;

namespace Globomantics.Api.Repositories;

public interface IConferenceRepository
{
    int Add(ConferenceModel model);
    IEnumerable<ConferenceModel> GetAll();
    ConferenceModel? GetById(int id);
}