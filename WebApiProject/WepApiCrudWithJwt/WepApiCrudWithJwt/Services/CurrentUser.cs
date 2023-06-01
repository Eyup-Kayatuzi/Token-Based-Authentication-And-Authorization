using Microsoft.AspNetCore.Mvc.Filters;

namespace WepApiCrudWithJwt.Services
{
    public class CurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CurrentUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public string CurrentUserName()
        {
            
            return _httpContextAccessor.HttpContext.User.Identity.Name;
        }
    }
}
