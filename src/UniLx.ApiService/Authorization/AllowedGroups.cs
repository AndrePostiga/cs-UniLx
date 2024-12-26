using Microsoft.AspNetCore.Authorization;

namespace UniLx.ApiService.Authorization
{
    public class AllowedGroups(params string[] roles)
    {
        private readonly string[] _roles = roles;

        public static implicit operator IAuthorizeData[](AllowedGroups authorize)
        {
            return new[] { new AuthorizeAttribute { Roles = string.Join(",", authorize._roles) } };
        }
    }
}
