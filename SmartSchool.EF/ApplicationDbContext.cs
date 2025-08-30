using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SmartSchool.Core.Models;

namespace SmartSchool.EF
{
    public class ApplicationDbContext : DbContext
    {
        //This Constarcter is important for connecting to the Database
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<SubjectDetail> SubjectDetails { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<TeachingSubject> TeachingSubjects { get; set; }
        public DbSet<Content> Contents { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<Resulte> Resultes { get; set; }
        public DbSet<ExamType> ExamTypes { get; set; }


        // Here Know what classes are tables in DB
        public DbSet<Student> Students { get; set; }
        public DbSet<Guardian> Guardians { get; set; }
        public DbSet<RelationType> RelationTypes { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<StudentAttendance> StudentAttendances { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Specialty> Specialties { get; set; }
        public DbSet<TeacherHoliday> TeacherHolidays { get; set; }
        public DbSet<TimeTable> TimeTables { get; set; }


        //Here write settings of special relationships
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            //Make default sitting of EF before we edit on it
            base.OnModelCreating(modelBuilder);


            //Relation Between User(n) and Role(1)
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId);

            //Relation Subject(1) between SubjectDetail(n) tables 
            modelBuilder.Entity<SubjectDetail>()
                .HasOne(u => u.Subject)
                .WithMany(m => m.SubjectDetails)
                .HasForeignKey(u => u.SubjectId);

            //Relation Grade(1) between SubjectDetail(n) tables 
            modelBuilder.Entity<SubjectDetail>()
                .HasOne(u => u.Grade)
                .WithMany(m => m.SubjectDetails)
                .HasForeignKey(u => u.GradeId);

            // This is the correct way to define the relationship for Assignments.
            // No need for the other fluent API call from the Assignment side.
            modelBuilder.Entity<SubjectDetail>()
                .HasMany(a => a.Assignments)
                .WithOne(m => m.SubjectDetail)
                .HasForeignKey(m => m.SubjectDetailId)
                .OnDelete(DeleteBehavior.NoAction);

            // This is the correct way to define the relationship for Results.
            // No need for the other fluent API call from the Resulte side.
            modelBuilder.Entity<SubjectDetail>()
                .HasMany(a => a.Resultes)
                .WithOne(m => m.SubjectDetail)
                .HasForeignKey(m => m.SubjectDetailId)
                .OnDelete(DeleteBehavior.NoAction);


            // This is the correct way to define the relationship for Exams.
            // No need for the other fluent API call from the Exam side.
            modelBuilder.Entity<SubjectDetail>()
                .HasMany(a => a.Exams)
                .WithOne(m => m.SubjectDetail)
                .HasForeignKey(m => m.SubjectDetailId)
                .OnDelete(DeleteBehavior.NoAction);

            // NEW: Configuring the foreign key to Groups for Exams
            modelBuilder.Entity<Exam>()
                .HasOne(e => e.Group)
                .WithMany(g => g.Exams)
                .HasForeignKey(e => e.GroupId)
                .OnDelete(DeleteBehavior.NoAction);

            //Relation SubjectDetail(1) between TeachingSubjects(n) tables 
            modelBuilder.Entity<SubjectDetail>()
                .HasMany(a => a.TeachingSubjects)
                .WithOne(m => m.SubjectDetail)
                .HasForeignKey(m => m.SubjectDetailId)
                .OnDelete(DeleteBehavior.NoAction);

            //Relation SubjectDetail(1) between Contents(n) tables 
            modelBuilder.Entity<SubjectDetail>()
                .HasMany(a => a.Contents)
                .WithOne(m => m.SubjectDetail)
                .HasForeignKey(m => m.SubjectDetailId)
                .OnDelete(DeleteBehavior.NoAction);

            //Relation SubjectDetail(1) between TimeTables(n) tables 
            modelBuilder.Entity<SubjectDetail>()
                .HasMany(a => a.TimeTables)
                .WithOne(m => m.SubjectDetail)
                .HasForeignKey(m => m.SubjectDetailId)
                .OnDelete(DeleteBehavior.NoAction);

            //Relation ExamType(1) between Exam(n) tables 
            modelBuilder.Entity<Exam>()
                .HasOne(u => u.ExamType)
                .WithMany(m => m.Exams)
                .HasForeignKey(u => u.ExamTypeId)
                .OnDelete(DeleteBehavior.NoAction);

            //Relation Teacher(1) between TeachingSubjects(n) tables 
            modelBuilder.Entity<Teacher>()
                .HasMany(u => u.TeachingSubjects)
                .WithOne(m => m.Teacher)
                .HasForeignKey(u => u.TeacherId)
                .OnDelete(DeleteBehavior.NoAction);

            //Relation Student(1) between Resaults(n) tables
            modelBuilder.Entity<Student>()
                .HasMany(u => u.Resultes)
                .WithOne(m => m.Student)
                .HasForeignKey(u => u.StudentId)
                .OnDelete(DeleteBehavior.NoAction);

            //Relation Between User(1) and Guardian(1)
            modelBuilder.Entity<Guardian>()
                .HasOne(u => u.User)
                .WithOne(r => r.Guardian)
                .HasForeignKey<Guardian>(u => u.UserId);

            //Relation Between User(1) and Student(1)
            modelBuilder.Entity<Student>()
                .HasOne(u => u.User)
                .WithOne(r => r.Student)
                .HasForeignKey<Student>(u => u.UserId);

            //Relation Between Guardian(1) and RelationType(1)
            modelBuilder.Entity<Guardian>()
                .HasOne(u => u.RelationType)
                .WithMany(r => r.Guardians)
                .HasForeignKey(u => u.RelationTypeId)
                .OnDelete(DeleteBehavior.NoAction);

            //Relation Between Student(1) and Guardian(n)
            modelBuilder.Entity<Student>()
                .HasOne(u => u.Guardian)
                .WithMany(r => r.Students)
                .HasForeignKey(u => u.GuardianId)
                .OnDelete(DeleteBehavior.NoAction);

            //Relation Between Group(1) and Student(n)
            modelBuilder.Entity<Student>()
                .HasOne(u => u.Group)
                .WithMany(r => r.Students)
                .HasForeignKey(u => u.GroupId)
                .OnDelete(DeleteBehavior.Restrict);

            //Relation Between Grade(1) and Group(n)
            modelBuilder.Entity<Group>()
                .HasOne(u => u.Grade)
                .WithMany(r => r.Groups)
                .HasForeignKey(u => u.GradeId);

            //Relation Between Student(1) and StudentAttendance(n)
            modelBuilder.Entity<StudentAttendance>()
                .HasOne(u => u.Student)
                .WithMany(r => r.StudentAttendances)
                .HasForeignKey(u => u.StudentId);

            //Relation Between Teacher(1) and StudentAttendances(n)
            modelBuilder.Entity<StudentAttendance>()
                .HasOne(u => u.Teacher)
                .WithMany(r => r.StudentAttendances)
                .HasForeignKey(u => u.TeacherId)
                .OnDelete(DeleteBehavior.NoAction);

            //Relation Between Teacher(1) and User(1)
            modelBuilder.Entity<Teacher>()
                .HasOne(u => u.User)
                .WithOne(r => r.Teacher)
                .HasForeignKey<Teacher>(u => u.UserId);

            //Relation Between Teacher(1) and Specialty(1)
            modelBuilder.Entity<Teacher>()
                .HasOne(u => u.Specialty)
                .WithMany(r => r.Teachers)
                .HasForeignKey(u => u.SpecialtyId)
                .OnDelete(DeleteBehavior.NoAction);

            //Relation Between Teacher(1) and TeacherHolidays(n)
            modelBuilder.Entity<TeacherHoliday>()
                .HasOne(u => u.Teacher)
                .WithMany(r => r.TeacherHolidays)
                .HasForeignKey(u => u.TeacherId);

            // Relation Between Teacher(1) and TimeTables(n) - CORRECTED
            modelBuilder.Entity<TimeTable>()
                .HasOne(u => u.Teacher)
                .WithMany(r => r.TimeTables)
                .HasForeignKey(u => u.TeacherId)
                .OnDelete(DeleteBehavior.NoAction);

            // Relation Between Group(1) and TimeTables(n) - CORRECTED
            modelBuilder.Entity<TimeTable>()
                .HasOne(u => u.Group)
                .WithMany(r => r.TimeTables)
                .HasForeignKey(u => u.GroupId)
                .OnDelete(DeleteBehavior.NoAction);

            // Relation Between SubjectDetails(1) and TimeTables(n) - CORRECTED
            modelBuilder.Entity<TimeTable>()
                .HasOne(u => u.SubjectDetail)
                .WithMany(r => r.TimeTables)
                .HasForeignKey(u => u.SubjectDetailId)
                .OnDelete(DeleteBehavior.NoAction);

            //Modify all created_at property to make dynamic
            //For users when thier record add to our database
            modelBuilder.Entity<User>()
            .Property(s => s.CreatedAt)
            .HasDefaultValueSql("GETDATE()");

            //For users when thier record add to our database
            modelBuilder.Entity<StudentAttendance>()
            .Property(s => s.AttendanceDate)
            .HasDefaultValueSql("GETDATE()");

            //For users when thier record add to our database
            modelBuilder.Entity<Assignment>()
            .Property(s => s.UploadDate)
            .HasDefaultValueSql("GETDATE()");

            //For users when thier record add to our database
            modelBuilder.Entity<SubmittedAssignment>()
            .Property(s => s.SubmittedDate)
            .HasDefaultValueSql("GETDATE()");

            

            // The Unique Propeties

            // Composite unique index on Title and SubjectDetailId
            modelBuilder.Entity<Assignment>()
                .HasIndex(a => new { a.Title, a.SubjectDetailId })
                .IsUnique();

            // Composite unique index on Name and SubjectDetailId
            modelBuilder.Entity<Content>()
                .HasIndex(c => new { c.Name, c.SubjectDetailId })
                .IsUnique();

            // Composite unique index on ExamDate, GroupId, and SubjectDetailId
            modelBuilder.Entity<Exam>()
                .HasIndex(e => new { e.ExamDate, e.GroupId, e.SubjectDetailId })
                .IsUnique();

            // Composite unique index on Name and Year
            modelBuilder.Entity<ExamType>()
                .HasIndex(et => new { et.Name, et.Year })
                .IsUnique();

            // Composite unique index on Name and Stage
            modelBuilder.Entity<Grade>()
                .HasIndex(g => new { g.Name, g.Stage })
                .IsUnique();

            // Composite unique index on Name, AcademicYear, and GradeId
            modelBuilder.Entity<Group>()
                .HasIndex(g => new { g.Name, g.AcademicYear, g.GradeId })
                .IsUnique();

            // Ensure a one-to-one relationship by making UserId unique
            modelBuilder.Entity<Guardian>()
                .HasIndex(g => g.UserId)
                .IsUnique();

            // Ensure that the relationship name is unique
            modelBuilder.Entity<RelationType>()
                .HasIndex(rt => rt.Name)
                .IsUnique();

            // Composite unique index on StudentId and SubjectDetailId
            // Ensures a student has only one result per subject.
            modelBuilder.Entity<Resulte>()
                .HasIndex(r => new { r.StudentId, r.SubjectDetailId })
                .IsUnique();

            // Ensure that the role name is unique
            modelBuilder.Entity<Role>()
                .HasIndex(r => r.Name)
                .IsUnique();

            // Composite unique index on Name and Qualification
            modelBuilder.Entity<Specialty>()
                .HasIndex(s => new { s.Name, s.Qualification })
                .IsUnique();

            // Ensure a one-to-one relationship by making UserId unique
            modelBuilder.Entity<Student>()
                .HasIndex(s => s.UserId)
                .IsUnique();

            // Composite unique index on StudentId and AttendanceDate
            // Ensures a student has only one attendance record per day.
            modelBuilder.Entity<StudentAttendance>()
                .HasIndex(sa => new { sa.StudentId, sa.AttendanceDate })
                .IsUnique();

            // Ensure that the subject name is unique
            modelBuilder.Entity<Subject>()
                .HasIndex(s => s.Name)
                .IsUnique();

            // Composite unique index on GradeId and SubjectId
            // Ensures a subject is associated with a specific grade only once.
            modelBuilder.Entity<SubjectDetail>()
                .HasIndex(sd => new { sd.GradeId, sd.SubjectId })
                .IsUnique();

            // Composite unique index on StudentId and AssignmentId
            // Ensures a student submits a single copy of a given assignment.
            modelBuilder.Entity<SubmittedAssignment>()
                .HasIndex(sa => new { sa.StudentId, sa.AssignmentId })
                .IsUnique();

            // Ensures a one-to-one relationship by making UserId unique
            modelBuilder.Entity<Teacher>()
                .HasIndex(t => t.UserId)
                .IsUnique();

            // Composite unique index on TeacherId and StartDate
            // Ensures a teacher has only one holiday request starting on a specific day.
            modelBuilder.Entity<TeacherHoliday>()
                .HasIndex(th => new { th.TeacherId, th.StartDate })
                .IsUnique();

            // Composite unique index on TeacherId, SubjectDetailId, AcademicYear, and Semester
            // Ensures a teacher is assigned to a specific subject only once per semester.
            modelBuilder.Entity<TeachingSubject>()
                .HasIndex(ts => new { ts.TeacherId, ts.SubjectDetailId, ts.AcademicYear, ts.Semster })
                .IsUnique();

            // Composite unique index on GroupId, DayOfWeek, and StartTime
            // Ensures a group has only one class at a specific time on a specific day.
            modelBuilder.Entity<TimeTable>()
                .HasIndex(tt => new { tt.GroupId, tt.DayOfWeek, tt.StartTime })
                .IsUnique();

            // In User Table Name Can't Be Unique ,The Only Things That Will Be is:
            // Ensure the email address is unique
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Ensure the phone number is unique
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Phone)
                .IsUnique();
        }
    }
}

