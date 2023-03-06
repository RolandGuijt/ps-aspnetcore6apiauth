using Globomantics.Models;

namespace Globomantics.Repositories;

public class UserRepository : IUserRepository
{
    private List<UserModel> users = new()
    {
        new UserModel { Id = 3522, Name = "roland", Password = "K7gNU3sdo+OL0wNhqoVWhr3g6s1xYv72ol/pe/Unols=",
            FavoriteColor = "blue", Role = "Admin" }
    };

    public UserModel? GetByUsernameAndPassword(string username, string password)
    {
        var user = users.SingleOrDefault(u => u.Name == username && u.Password == password.Sha256());
        return user;
    }
}
