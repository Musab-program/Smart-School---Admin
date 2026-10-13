namespace SmartSchool.Main.Dtos
{
    public class UserDto
    {
        public int? Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public int RoleID { get; set; }
        public string Phone { get; set; }

        /// <summary>
        /// Used ONLY when creating or updating a user. Never returned in responses.
        /// </summary>
        public string? PasswordInput { get; set; }

        public DateTime DateOfBirth { get; set; }
        public bool IsActive { get; set; }
        public string Address { get; set; }
        public string gender { get; set; }
    }
}
