using Microsoft.AspNetCore.Identity;

namespace WepApiCrudWithJwt.Identity
{
    public class AppIdentityRole: IdentityRole
    {
        public string? Description { get; set; }
    }
}
