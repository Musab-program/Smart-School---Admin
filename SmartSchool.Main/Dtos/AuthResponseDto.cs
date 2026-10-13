namespace SmartSchool.Main.Dtos
{
    /// <summary>
    /// Returned after a successful login or token refresh.
    /// Contains the JWT access token and a refresh token.
    /// </summary>
    public class AuthResponseDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpiresAt { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}
