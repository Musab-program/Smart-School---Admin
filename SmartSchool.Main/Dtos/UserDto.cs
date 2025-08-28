namespace SmartSchool.Main.Dtos
{
    public class UserDto
    {
        public int? Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public int RoleID { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool IsActive { get; set; }
        public string Address { get; set; }
        public string gender { get; set; }

    }
}
