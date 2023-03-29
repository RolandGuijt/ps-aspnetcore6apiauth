using Microsoft.AspNetCore.Authorization;

namespace Globomantics.Api.Authorization
{
    public class IsInRoleRequirement: IAuthorizationRequirement
    {
        public string Role { get; set; }
        public int ApplicationId { get; set; }
    }
}
