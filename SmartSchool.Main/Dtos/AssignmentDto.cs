namespace SmartSchool.Main.Dtos
{
    public class AssignmentDto
    {
        public int? Id { get; set; }
        public int SubjectDetailId { get; set; }//Forign Key n to 1 With SubjectDetails Table
        public int StudentId { get; set; }//Forign Key n to 1 With Student Table
        public string Title { get; set; }
        public DateTime LastDate { get; set; }
        public string ChekeState { get; set; }
        public double Mark { get; set; }
    }
}
