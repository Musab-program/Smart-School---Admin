
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
                AttendanceDate = s.AttendanceDate,
                Status = s.Status,
                TeacherId = s.TeacherId,
            });

            return new Response<StudentAttendanceDto>
            {
                Message = "تم جلب سجلات الحضور بنجاح",
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
                    Message = "سجل الحضور غير موجود",
                    Code = 400,
                };
            }

            return new Response<StudentAttendanceDto>
            {
                Message = "تم العثور على سجل الحضور",
                Code = 200,
                Data = new StudentAttendanceDto
                {
                    Id = studentAttendance.Id,
                    TeacherId = studentAttendance.TeacherId,
                    StudentId = studentAttendance.StudentId,
                    AttendanceDate = studentAttendance.AttendanceDate,
                    Status = studentAttendance.Status,
                }
            };
        }

        public async Task<Response<StudentAttendanceDto>> AddStudentAttendance(StudentAttendanceDto dto)
        {
            var student = await _unitOfWork.Students.FindAsync(b => b.Id == dto.StudentId);
            if (student == null)
                return new Response<StudentAttendanceDto>
                {
                    Message = "الطالب غير موجود",
                    Code = 400
                };

            var teacher = await _unitOfWork.Teachers.FindAsync(b => b.Id == dto.TeacherId);
            if (teacher == null)
                return new Response<StudentAttendanceDto>
                {
                    Message = "المعلم غير موجود",
                    Code = 400
                };

            var entity = new Core.Models.StudentAttendance
            {
                StudentId = dto.StudentId,
                TeacherId = dto.TeacherId,
                AttendanceDate = dto.AttendanceDate,
                Status = dto.Status,
            };

            var created = await _unitOfWork.StudentAttendances.AddAsync(entity);
            _unitOfWork.Save();

            return new Response<StudentAttendanceDto>
            {
                Message = "تم تسجيل الحضور بنجاح",
                Code = 200,
                Data = new StudentAttendanceDto
                {
                    Id = created.Id,
                    StudentId = created.StudentId,
                    TeacherId = created.TeacherId,
                    AttendanceDate = created.AttendanceDate,
                    Status = created.Status,
                }
            };
        }

        public async Task<Response<StudentAttendanceDto>> UpdateStudentAttendance(StudentAttendanceDto dto)
        {
            var record = await _unitOfWork.StudentAttendances.FindAsync(b => b.Id == dto.Id);
            if (record == null)
                return new Response<StudentAttendanceDto>
                {
                    Message = "سجل الحضور غير موجود",
                    Code = 400
                };

            var student = await _unitOfWork.Students.FindAsync(b => b.Id == dto.StudentId);
            if (student == null)
                return new Response<StudentAttendanceDto>
                {
                    Message = "الطالب غير موجود",
                    Code = 400
                };

            var teacher = await _unitOfWork.Teachers.FindAsync(b => b.Id == dto.TeacherId);
            if (teacher == null)
                return new Response<StudentAttendanceDto>
                {
                    Message = "المعلم غير موجود",
                    Code = 400
                };

            record.StudentId = dto.StudentId;
            record.TeacherId = dto.TeacherId;
            record.AttendanceDate = dto.AttendanceDate;
            record.Status = dto.Status;

            _unitOfWork.StudentAttendances.Update(record);
            _unitOfWork.Save();

            return new Response<StudentAttendanceDto>
            {
                Message = "تم تعديل سجل الحضور بنجاح",
                Code = 200,
                Data = new StudentAttendanceDto
                {
                    Id = record.Id,
                    StudentId = record.StudentId,
                    TeacherId = record.TeacherId,
                    AttendanceDate = record.AttendanceDate,
                    Status = record.Status,
                }
            };
        }

        public async Task<Response<StudentAttendanceDto>> DeleteStudentAttendance(int id)
        {
            var record = await _unitOfWork.StudentAttendances.FindAsync(b => b.Id == id);
            if (record == null)
                return new Response<StudentAttendanceDto>
                {
                    Message = "سجل الحضور غير موجود",
                    Code = 400
                };

            _unitOfWork.StudentAttendances.Delete(record);
            _unitOfWork.Save();

            return new Response<StudentAttendanceDto>
            {
                Message = "تم حذف سجل الحضور بنجاح",
                Code = 200,
            };
        }

        public async Task<Response<int>> CountStudentAttendances()
        {
            var StudentAttendanceCount = await _unitOfWork.StudentAttendances.CountAsync();

            return new Response<int>
            {
                Message = "Success",
                Code = 200,
                Data = StudentAttendanceCount
            };
        }

    }


    public interface IStudentAttendanceService
    {
        Task<Response<StudentAttendanceDto>> GetAllStudentAttendance();
        Task<Response<StudentAttendanceDto>> GetByIdStudentAttendance(int id);
        Task<Response<StudentAttendanceDto>> AddStudentAttendance(StudentAttendanceDto dto);
        Task<Response<StudentAttendanceDto>> UpdateStudentAttendance(StudentAttendanceDto dto);
        Task<Response<StudentAttendanceDto>> DeleteStudentAttendance(int id);
        Task<Response<int>> CountStudentAttendances();
    }
}
