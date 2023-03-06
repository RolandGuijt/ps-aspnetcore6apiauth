using Globomantics.Models;

namespace Globomantics.Repositories;

public interface IUserRepository
{
    UserModel? GetByUsernameAndPassword(string username, string password);
}