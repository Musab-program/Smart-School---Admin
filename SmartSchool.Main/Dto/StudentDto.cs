namespace SmartSchool.Main.Dtos
{
    public class StudentDto
    {
            // Student Properties
            public int? Id { get; set; } // قد يكون null عند الإضافة الجديدة
            public DateTime RegisterDate { get; set; }

        // Users Properties
            public int UserId { get; set; } 
            public string UserName { get; set; } 
            public string UserEmail { get; set; } 
            public byte[] UserPassword { get; set; }
            public string UserPhone { get; set; }
            public DateTime UserDateOfBirth { get; set; }
            public DateTime UserCreatedAt { get; set; }
            public bool IsActiveUser { get; set; }
            public string UserAddress { get; set; }
            public string UserGender { get; set; }
            public int RoleId { get; set; }

            // Guardian Properties
            public int GuardianId { get; set; } 
            public string GuardianName { get; set; }
            public int GuardianUserId { get; set; } //*******************
            public int RelationTypeId { get; set; } 
            public string SecondryPhone { get; set; }

        //Group Properties
        public int GroupId { get; set; } 
        }

}
