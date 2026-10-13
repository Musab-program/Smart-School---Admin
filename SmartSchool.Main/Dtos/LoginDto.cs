namespace SmartSchool.Main.Dtos
{
    /// <summary>
    /// Data transfer object used for login requests.
    /// </summary>
    public class LoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
