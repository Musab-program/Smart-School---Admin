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



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Relation role(1) between user(n) tables 
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


        }

    }
}
