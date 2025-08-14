using SmartSchool.Core.Interfaces;
using SmartSchool.Core.Models;

namespace SmartSchool.Core
{
    /// <summary>
    ///// The Goal of unit of work is to collect all the reposetries in in one place ,so
    ///you can manage all the operation  related to DB
    /// </summary>
    /// 
    public interface IUnitOfWork : IDisposable
    {
        //We write just {get} not {set} to make the data more secutity and nobody can change it 
        //and means the programer can reach to the properity not modify it

        /// <summary>
        /// References for all the repositories 
        /// </summary>
        IBaseRepository<Assignment> Assignments { get; }
        IBaseRepository<Content> Contents { get; }
        IBaseRepository<Exam> Exams { get; }
        IBaseRepository<ExamType> ExamTypes { get; }
        IBaseRepository<Grade> Grades { get; }
        IBaseRepository<Group> Groups { get; }
        IBaseRepository<Guardian> Guardians { get; }
        IBaseRepository<Notifiaction> Notifiactions { get; }
        IBaseRepository<RelationType> RelationTypes { get; }
        IBaseRepository<Resulte> Resultes { get; }
        IBaseRepository<Role> Roles { get; }
        IBaseRepository<Specialty> Specialtys { get; }
        IBaseRepository<Student> Students { get; }
        IBaseRepository<StudentAttendance> StudentAttendances { get; }
        IBaseRepository<Subject> Subjects { get; }
        IBaseRepository<SubjectDetail> SubjectDetails { get; }
        IBaseRepository<Teacher> Teachers { get; }
        IBaseRepository<TeacherHoliday> TeacherHolidays { get; }
        IBaseRepository<TeachingSubject> TeachingSubjects { get; }
        IBaseRepository<TimeTable> TimeTables { get; }
        IBaseRepository<User> Users { get; }



        int Save();
    }
}