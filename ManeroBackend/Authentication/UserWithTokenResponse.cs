using ManeroBackend.Contexts;

namespace ManeroBackend.Authentication
{
    public class UserWithTokenResponse
    {
        public ApplicationUser User { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
