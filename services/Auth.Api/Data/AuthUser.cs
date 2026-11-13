namespace Auth.Api.Data
{
    public class AuthUser
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = String.Empty;
        public string PasswordHash { get; set; } = String.Empty;
        public string Role { get; set; } = "User";
    }
}
