using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
        public DbSet<User> Users { get; set; }  
        public DbSet<Role> Roles { get; set; }
        public DbSet<Adminstration> Adminstrations { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Guardian> Guardians { get; set; }
        public DbSet<RelationType> RelationTypes { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<StudentAttendance> StudentAttendances { get; set;} 
        public DbSet<Teacher> Teachers { get; set;} 
        public DbSet<Specialty> Specialties { get; set;} 
        public DbSet<TeacherHoliday> TeacherHolidays { get; set;} 
        public DbSet<TimeTable> TimeTables { get; set;} 

        
        //Here write settings of special relationships
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Make default sitting of EF before we edit on it
            base.OnModelCreating(modelBuilder);

            //Relation role(1) between user(n) tables 
            
            //make the relationship and the foreignKey

            //Relation Between User(n) and Role(1)
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId);

            //Relation Subject(1) between SubjectDetail(n) tables 
            modelBuilder.Entity<SubjectDetail>()
                .HasOne(u=>u.Subject)
                .WithMany(m=>m.SubjectDetails)
                .HasForeignKey(u => u.SubjectId);

            //Relation Grade(1) between SubjectDetail(n) tables 
            modelBuilder.Entity<SubjectDetail>()
                .HasOne(u => u.Grade)
                .WithMany(m => m.SubjectDetails)
                .HasForeignKey(u => u.GradeId);

            //Relation SubjectDetail(1) between Assignments(n) tables 
            modelBuilder.Entity<SubjectDetail>()
                .HasMany(a => a.Assignments)
                .WithOne(m => m.SubjectDetail)
                .HasForeignKey(m => m.SubjectDetailId);

            //Relation SubjectDetail(1) between Resaults(n) tables 
            modelBuilder.Entity<SubjectDetail>()
                .HasMany(a => a.Resultes)
                .WithOne(m => m.SubjectDetail)
                .HasForeignKey(m => m.SubjectDetailId);

            //Relation SubjectDetail(1) between Exams(n) tables 
            modelBuilder.Entity<SubjectDetail>()
                .HasMany(a => a.Exams)
                .WithOne(m => m.SubjectDetail)
                .HasForeignKey(m => m.SubjectDetailId);

            //Relation SubjectDetail(1) between TeachingSubjects(n) tables 
            modelBuilder.Entity<SubjectDetail>()
                .HasMany(a => a.TeachingSubjects)
                .WithOne(m => m.SubjectDetail)
                .HasForeignKey(m => m.SubjectDetailId);

            //Relation SubjectDetail(1) between Contents(n) tables 
            modelBuilder.Entity<SubjectDetail>()
                .HasMany(a => a.Contents)
                .WithOne(m => m.SubjectDetail)
                .HasForeignKey(m => m.SubjectDetailId);

            //Relation StudSubjectDetailent(1) between TimeTables(n) tables 
            modelBuilder.Entity<SubjectDetail>()
                .HasMany(a => a.TimeTables)
                .WithOne(m => m.SubjectDetail)
                .HasForeignKey(m => m.SubjectDetailId);

            //Relation ExamType(1) between Exam(n) tables 
            modelBuilder.Entity<Exam>()
                .HasOne(u => u.ExamType)
                .WithMany(m => m.Exams)
                .HasForeignKey(u => u.ExamTypeId);

            //Relation Teacher(1) between TeachingSubjects(n) tables 
            modelBuilder.Entity<Teacher>()
                .HasMany(u => u.TeachingSubjects)
                .WithOne(m => m.Teacher)
                .HasForeignKey(u => u.TeacherId);

            //Relation Student(1) between Assignment(n) tables 
            modelBuilder.Entity<Student>()
                .HasMany(u => u.Assignment)
                .WithOne(m => m.Student)
                .HasForeignKey(u => u.StudentId);

            //Relation Student(1) between Resaults(n) tables
            modelBuilder.Entity<Student>()
                .HasMany(u => u.Resultes)
                .WithOne(m => m.Student)
                .HasForeignKey(u => u.StudentId);


            //Relation Between User(1) and Adminstration(1)
            modelBuilder.Entity<Adminstration>()
                .HasOne(u => u.User)
                .WithOne(r => r.Adminstration)
                .HasForeignKey<Adminstration>(u => u.UserId);

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
                .WithOne(r => r.Guardian)
                .HasForeignKey<Guardian>(u => u.RelationTypeId);

            //Relation Between Student(1) and Guardian(n)
            modelBuilder.Entity<Student>()
                .HasOne(u => u.Guardian)
                .WithMany(r => Students)
                .HasForeignKey(u => u.GuardianId);

            //Relation Between Group(1) and Student(n)
            modelBuilder.Entity<Student>()
                .HasOne(u => u.Group)
                .WithMany(r => r.Students)
                .HasForeignKey(u => u.GroupId);

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

            //Relation Between Teacher(1) and User(1)
            modelBuilder.Entity<Teacher>()
                .HasOne(u => u.User)
                .WithOne(r => r.Teacher)
                .HasForeignKey<Teacher>(u => u.UserId);

            //Relation Between Teacher(1) and StudentAttendances(n)
            modelBuilder.Entity<StudentAttendance>()
                .HasOne(u => u.Teacher)
                .WithMany(r => r.StudentAttendances)
                .HasForeignKey(u => u.TeacherId);

            //Relation Between Teacher(1) and Specialty(1)
            modelBuilder.Entity<Teacher>()
                .HasOne(u => u.Specialty)
                .WithOne(r => r.Teacher)
                .HasForeignKey<Teacher>(u => u.SpecialtyId);

            //Relation Between Teacher(1) and TeacherHolidays(n)
            modelBuilder.Entity<TeacherHoliday>()
                .HasOne(u => u.Teacher)
                .WithMany(r => r.TeacherHolidays)
                .HasForeignKey(u => u.TeacherId);

            //Relation Between Teacher(1) and TimeTables(n)
            modelBuilder.Entity<TimeTable>()
                .HasOne(u => u.Teacher)
                .WithMany(r => r.TimeTables)
                .HasForeignKey(u => u.TeacherId);

            //Relation Between Group(1) and TimeTables(n)
            modelBuilder.Entity<TimeTable>()
                .HasOne(u => u.Group)
                .WithMany(r => r.TimeTables)
                .HasForeignKey(u => u.GroupId);

            //Relation Between SubjectDetails(1) and TimeTables(n)
            modelBuilder.Entity<TimeTable>()
                .HasOne(u => u.SubjectDetails)
                .WithMany(r => r.TimeTables)
                .HasForeignKey(u => u.SubjectDetailsId);
        }

    }
}
