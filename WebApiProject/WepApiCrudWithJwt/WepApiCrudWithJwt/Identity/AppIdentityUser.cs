using Microsoft.AspNetCore.Identity;

namespace WepApiCrudWithJwt.Identity
{
    public class AppIdentityUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set;}
    }
}
