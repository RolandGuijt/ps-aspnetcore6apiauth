namespace Globomantics.Api.ApiServices
{
    public interface IAuthorizationApiService
    {
        Task<Permissions> GetPermissions(int userId, int applicationId);
    }
}