using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartSchool.Core;
using SmartSchool.Core.Interfaces;
using SmartSchool.Core.Models;
using SmartSchool.EF;
using SmartSchool.EF.ImplementedClasses;

namespace RepositoryPatternWithUOW.EF
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;



        public IBaseRepository<Assignment> Assignments { get; private set; }

        public IBaseRepository<Content> Contents { get; private set; }

        public IBaseRepository<Exam> Exams { get; private set; }

        public IBaseRepository<ExamType> ExamTypes { get; private set; }

        public IBaseRepository<Grade> Grades { get; private set; }

        public IBaseRepository<Group> Groups { get; private set; }

        public IBaseRepository<Guardian> Guardians { get; private set; }

        public IBaseRepository<Notifiaction> Notifiactions { get; private set; }

        public IBaseRepository<RelationType> RelationTypes { get; private set; }

        public IBaseRepository<Resulte> Resultes { get; private set; }

        public IBaseRepository<Role> Roles { get; private set; }

        public IBaseRepository<Specialty> Specialtys { get; private set; }

        public IBaseRepository<Student> Students { get; private set; }

        public IBaseRepository<StudentAttendance> StudentAttendances { get; private set; }

        public IBaseRepository<Subject> Subjects { get; private set; }

        public IBaseRepository<SubjectDetail> SubjectDetails { get; private set; }

        public IBaseRepository<Teacher> Teachers { get; private set; }

        public IBaseRepository<TeacherHoliday> TeacherHolidays { get; private set; }

        public IBaseRepository<TeachingSubject> TeachingSubjects { get; private set; }

        public IBaseRepository<TimeTable> TimeTables { get; private set; }

        public IBaseRepository<User> Users { get; private set; }

        //to make sure there is property or not
        public IBaseRepository<SubmittedAssignment> SubmittedAssignments {get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;

            Assignments = new BaseRepository<Assignment>(_context);
            Contents = new BaseRepository<Content>(_context);
            Exams = new BaseRepository<Exam>(_context);
            ExamTypes = new BaseRepository<ExamType>(_context);
            Grades = new BaseRepository<Grade>(_context);
            Groups = new BaseRepository<Group>(_context);
            Guardians = new BaseRepository<Guardian>(_context);
            Notifiactions = new BaseRepository<Notifiaction>(_context);
            RelationTypes = new BaseRepository<RelationType>(_context);
            Resultes = new BaseRepository<Resulte>(_context);
            Roles = new BaseRepository<Role>(_context);
            Specialtys = new BaseRepository<Specialty>(_context);
            Students = new BaseRepository<Student>(_context);
            StudentAttendances = new BaseRepository<StudentAttendance>(_context);
            Subjects = new BaseRepository<Subject>(_context);
            SubjectDetails = new BaseRepository<SubjectDetail>(_context);
            Teachers = new BaseRepository<Teacher>(_context);
            TeacherHolidays = new BaseRepository<TeacherHoliday>(_context);
            TeachingSubjects = new BaseRepository<TeachingSubject>(_context);
            TimeTables = new BaseRepository<TimeTable>(_context);
            Users = new BaseRepository<User>(_context);
            // to make sure if there is property or not
            SubmittedAssignments = new BaseRepository<SubmittedAssignment>(_context);

        }


        public int Save()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }


        public async Task<T> ExcuteInTransactionAsync<T>(Func<Task<T>> action)

        {
            var transaction = await _context.Database.BeginTransactionAsync();

            try
            {

                var result= await action();
                await transaction.CommitAsync();
                return result;
            }

            catch
            {
                await transaction.RollbackAsync();
                throw new Exception("حدث خطأ ما!");
            }
        }
    }
}