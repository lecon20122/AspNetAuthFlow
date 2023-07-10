using Microsoft.AspNetCore.DataProtection;

namespace Auth
{
    public class AuthService
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly IDataProtectionProvider _idp;

        public AuthService(IHttpContextAccessor accessor, IDataProtectionProvider idp)
        {
            _accessor = accessor;
            _idp = idp;
        }

        public void SignIn()
        {
            var protectedCookie = _idp.CreateProtector("auth").Protect("mustafa");
            _accessor.HttpContext.Response.Headers["Set-Cookie"] = $"username={protectedCookie}";
        }
    }
}
