
using SmartSchool.Core;
using SmartSchool.Core.Shared;
using SmartSchool.Main.Dtos;

namespace SmartSchool.Main.InterFaces
{
    public class StudentAttendanceService : IStudentAttendanceService
    {
        private readonly IUnitOfWork _unitOfWork;

        public StudentAttendanceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        
        public async Task<Response<StudentAttendanceDto>> GetAllStudentAttendance()
        {
            var studentAttendances = await _unitOfWork.StudentAttendances.GetAllAsync();

            var dataDisplay = studentAttendances.Select(s => new StudentAttendanceDto
            {
                Id = s.Id,
                StudentId = s.StudentId,
                Status = s.Status,
                TeacherId = s.TeacherId,
            });

            return new Response<StudentAttendanceDto>
            {
                Message = "تم جلب جدول الحضور بنجاح",
                Data = dataDisplay,
                Code = 200
            };
        }


        public async Task<Response<StudentAttendanceDto>> GetByIdStudentAttendance(int id)
        {
            var studentAttendance = await _unitOfWork.StudentAttendances.GetByIdAsync(id);

            if (studentAttendance == null)
            {
                return new Response<StudentAttendanceDto>
                {
                    Message = "جدول الحضور غير موجود ",
                    Code = 400,
                };
            }

            return new Response<StudentAttendanceDto>
            {
                Message = "تم العثور على جدول الحضور",
                Code = 200,
                Data = new StudentAttendanceDto
                {
                    Id = studentAttendance.Id,
                    TeacherId=studentAttendance.TeacherId,
                    StudentId = studentAttendance.StudentId,
                    Status = studentAttendance.Status,
                }
            };
        }

        }


    public interface IStudentAttendanceService
    {
        Task<Response<StudentAttendanceDto>> GetAllStudentAttendance();
        Task<Response<StudentAttendanceDto>> GetByIdStudentAttendance(int id);
    }
}
