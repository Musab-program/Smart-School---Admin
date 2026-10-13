using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Core.Models;

namespace SmartSchool.EF
{
    // IdentityDbContext<User, Role, int>:
    //   - User    → our custom user extending IdentityUser<int>
    //   - Role    → our custom role extending IdentityRole<int>
    //   - int     → the key type (matches our existing int PKs)
    // Identity automatically creates: AspNetUsers, AspNetRoles, AspNetUserRoles,
    //   AspNetUserClaims, AspNetRoleClaims, AspNetUserLogins, AspNetUserTokens
    public class ApplicationDbContext : IdentityDbContext<User, Role, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // ──────────────────────────────────────────────────────────────
        // DbSets (Role is now managed by Identity, no need for DbSet<Role>)
        // ──────────────────────────────────────────────────────────────
        public DbSet<SubjectDetail> SubjectDetails { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<TeachingSubject> TeachingSubjects { get; set; }
        public DbSet<Content> Contents { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<Resulte> Resultes { get; set; }
        public DbSet<ExamType> ExamTypes { get; set; }
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
        public DbSet<SubmittedAssignment> SubmittedAssignments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // MUST call base first so Identity tables are configured
            base.OnModelCreating(modelBuilder);

            // ──────────────────────────────────────────────────────────
            // Custom User → Role relationship (direct FK for easy access)
            // This is ADDITIONAL to Identity's AspNetUserRoles join table
            // ──────────────────────────────────────────────────────────
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            // ──────────────────────────────────────────────────────────
            // Subject relationships
            // ──────────────────────────────────────────────────────────
            modelBuilder.Entity<SubjectDetail>()
                .HasOne(u => u.Subject)
                .WithMany(m => m.SubjectDetails)
                .HasForeignKey(u => u.SubjectId);

            modelBuilder.Entity<SubjectDetail>()
                .HasOne(u => u.Grade)
                .WithMany(m => m.SubjectDetails)
                .HasForeignKey(u => u.GradeId);

            modelBuilder.Entity<SubjectDetail>()
                .HasMany(a => a.Assignments)
                .WithOne(m => m.SubjectDetail)
                .HasForeignKey(m => m.SubjectDetailId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<SubjectDetail>()
                .HasMany(a => a.Resultes)
                .WithOne(m => m.SubjectDetail)
                .HasForeignKey(m => m.SubjectDetailId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<SubjectDetail>()
                .HasMany(a => a.Exams)
                .WithOne(m => m.SubjectDetail)
                .HasForeignKey(m => m.SubjectDetailId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<SubjectDetail>()
                .HasMany(a => a.TeachingSubjects)
                .WithOne(m => m.SubjectDetail)
                .HasForeignKey(m => m.SubjectDetailId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<SubjectDetail>()
                .HasMany(a => a.Contents)
                .WithOne(m => m.SubjectDetail)
                .HasForeignKey(m => m.SubjectDetailId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<SubjectDetail>()
                .HasMany(a => a.TimeTables)
                .WithOne(m => m.SubjectDetail)
                .HasForeignKey(m => m.SubjectDetailId)
                .OnDelete(DeleteBehavior.NoAction);

            // ──────────────────────────────────────────────────────────
            // Exam relationships
            // ──────────────────────────────────────────────────────────
            modelBuilder.Entity<Exam>()
                .HasOne(e => e.Group)
                .WithMany(g => g.Exams)
                .HasForeignKey(e => e.GroupId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Exam>()
                .HasOne(u => u.ExamType)
                .WithMany(m => m.Exams)
                .HasForeignKey(u => u.ExamTypeId)
                .OnDelete(DeleteBehavior.NoAction);

            // ──────────────────────────────────────────────────────────
            // Teacher relationships
            // ──────────────────────────────────────────────────────────
            modelBuilder.Entity<Teacher>()
                .HasMany(u => u.TeachingSubjects)
                .WithOne(m => m.Teacher)
                .HasForeignKey(u => u.TeacherId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Teacher>()
                .HasOne(u => u.User)
                .WithOne(r => r.Teacher)
                .HasForeignKey<Teacher>(u => u.UserId);

            modelBuilder.Entity<Teacher>()
                .HasOne(u => u.Specialty)
                .WithMany(r => r.Teachers)
                .HasForeignKey(u => u.SpecialtyId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<TeacherHoliday>()
                .HasOne(u => u.Teacher)
                .WithMany(r => r.TeacherHolidays)
                .HasForeignKey(u => u.TeacherId);

            // ──────────────────────────────────────────────────────────
            // Student relationships
            // ──────────────────────────────────────────────────────────
            modelBuilder.Entity<Student>()
                .HasMany(u => u.Resultes)
                .WithOne(m => m.Student)
                .HasForeignKey(u => u.StudentId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Student>()
                .HasOne(u => u.User)
                .WithOne(r => r.Student)
                .HasForeignKey<Student>(u => u.UserId);

            modelBuilder.Entity<Student>()
                .HasOne(u => u.Group)
                .WithMany(r => r.Students)
                .HasForeignKey(u => u.GroupId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Student>()
                .HasOne(u => u.Guardian)
                .WithMany(r => r.Students)
                .HasForeignKey(u => u.GuardianId)
                .OnDelete(DeleteBehavior.NoAction);

            // ──────────────────────────────────────────────────────────
            // Guardian relationships
            // ──────────────────────────────────────────────────────────
            modelBuilder.Entity<Guardian>()
                .HasOne(u => u.User)
                .WithOne(r => r.Guardian)
                .HasForeignKey<Guardian>(u => u.UserId);

            modelBuilder.Entity<Guardian>()
                .HasOne(u => u.RelationType)
                .WithMany(r => r.Guardians)
                .HasForeignKey(u => u.RelationTypeId)
                .OnDelete(DeleteBehavior.NoAction);

            // ──────────────────────────────────────────────────────────
            // Group & Grade relationships
            // ──────────────────────────────────────────────────────────
            modelBuilder.Entity<Group>()
                .HasOne(u => u.Grade)
                .WithMany(r => r.Groups)
                .HasForeignKey(u => u.GradeId);

            // ──────────────────────────────────────────────────────────
            // Attendance relationships
            // ──────────────────────────────────────────────────────────
            modelBuilder.Entity<StudentAttendance>()
                .HasOne(u => u.Student)
                .WithMany(r => r.StudentAttendances)
                .HasForeignKey(u => u.StudentId);

            modelBuilder.Entity<StudentAttendance>()
                .HasOne(u => u.Teacher)
                .WithMany(r => r.StudentAttendances)
                .HasForeignKey(u => u.TeacherId)
                .OnDelete(DeleteBehavior.NoAction);

            // ──────────────────────────────────────────────────────────
            // TimeTable relationships
            // ──────────────────────────────────────────────────────────
            modelBuilder.Entity<TimeTable>()
                .HasOne(u => u.Teacher)
                .WithMany(r => r.TimeTables)
                .HasForeignKey(u => u.TeacherId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<TimeTable>()
                .HasOne(u => u.Group)
                .WithMany(r => r.TimeTables)
                .HasForeignKey(u => u.GroupId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<TimeTable>()
                .HasOne(u => u.SubjectDetail)
                .WithMany(r => r.TimeTables)
                .HasForeignKey(u => u.SubjectDetailId)
                .OnDelete(DeleteBehavior.NoAction);

            // ──────────────────────────────────────────────────────────
            // Default values
            // ──────────────────────────────────────────────────────────
            modelBuilder.Entity<User>()
                .Property(s => s.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<StudentAttendance>()
                .Property(s => s.AttendanceDate)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Assignment>()
                .Property(s => s.UploadDate)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<SubmittedAssignment>()
                .Property(s => s.SubmittedDate)
                .HasDefaultValueSql("GETDATE()");

            // ──────────────────────────────────────────────────────────
            // Unique Indexes
            // ──────────────────────────────────────────────────────────
            modelBuilder.Entity<Assignment>()
                .HasIndex(a => new { a.Title, a.SubjectDetailId })
                .IsUnique();

            modelBuilder.Entity<Content>()
                .HasIndex(c => new { c.Name, c.SubjectDetailId })
                .IsUnique();

            modelBuilder.Entity<Exam>()
                .HasIndex(e => new { e.ExamDate, e.GroupId, e.SubjectDetailId })
                .IsUnique();

            modelBuilder.Entity<ExamType>()
                .HasIndex(et => new { et.Name, et.Year })
                .IsUnique();

            modelBuilder.Entity<Grade>()
                .HasIndex(g => new { g.Name, g.Stage })
                .IsUnique();

            modelBuilder.Entity<Group>()
                .HasIndex(g => new { g.Name, g.AcademicYear, g.GradeId })
                .IsUnique();

            modelBuilder.Entity<Guardian>()
                .HasIndex(g => g.UserId)
                .IsUnique();

            modelBuilder.Entity<RelationType>()
                .HasIndex(rt => rt.Name)
                .IsUnique();

            modelBuilder.Entity<Resulte>()
                .HasIndex(r => new { r.StudentId, r.SubjectDetailId })
                .IsUnique();

            modelBuilder.Entity<Specialty>()
                .HasIndex(s => new { s.Name, s.Qualification })
                .IsUnique();

            modelBuilder.Entity<Student>()
                .HasIndex(s => s.UserId)
                .IsUnique();

            modelBuilder.Entity<StudentAttendance>()
                .HasIndex(sa => new { sa.StudentId, sa.AttendanceDate })
                .IsUnique();

            modelBuilder.Entity<Subject>()
                .HasIndex(s => s.Name)
                .IsUnique();

            modelBuilder.Entity<SubjectDetail>()
                .HasIndex(sd => new { sd.GradeId, sd.SubjectId })
                .IsUnique();

            modelBuilder.Entity<SubmittedAssignment>()
                .HasIndex(sa => new { sa.StudentId, sa.AssignmentId })
                .IsUnique();

            modelBuilder.Entity<Teacher>()
                .HasIndex(t => t.UserId)
                .IsUnique();

            modelBuilder.Entity<TeacherHoliday>()
                .HasIndex(th => new { th.TeacherId, th.StartDate })
                .IsUnique();

            modelBuilder.Entity<TeachingSubject>()
                .HasIndex(ts => new { ts.TeacherId, ts.SubjectDetailId, ts.AcademicYear, ts.Semster })
                .IsUnique();

            modelBuilder.Entity<TimeTable>()
                .HasIndex(tt => new { tt.GroupId, tt.DayOfWeek, tt.StartTime })
                .IsUnique();
        }
    }
}
