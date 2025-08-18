using System;
using SmartSchool.Main.Dtos;
using SmartSchool.Core;
using SmartSchool.Core.Models;
using SmartSchool.Core.Shared;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSchool.Main.InterFaces
{
    public class TeacherService : ITeacherService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TeacherService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<TeacherDto>> AddTeacher(TeacherDto dto)
        {

            var user = await _unitOfWork.Users.FindAsync(b => b.Id == dto.UserId);
            if (user == null)
                return new Response<TeacherDto>
                {
                    Message = "المستخدم غير موجود ",
                    Code = 400
                };

            var teacher = await _unitOfWork.Teachers.FindAsync(b => b.Id == dto.Id);
            if (teacher != null)
                return new Response<TeacherDto>
                {
                    Message = "المعلم موجود ",
                    Code = 400
                };


            Teacher addTeacher = new Teacher
            {
               UserId = dto.UserId,
               Salary = dto.Salary,
               SpecialtyId = dto.SpecialtyId,
 

            };
            var teacherNew = await _unitOfWork.Teachers.AddAsync(addTeacher);
            _unitOfWork.Save();
            return new Response<TeacherDto>
            {
                Message = "تمت الاصافة",
                Data = teacherNew,
                Code = 200
            };
        }


        public async Task<Response<TeacherDto>> DeleteTeacher(int id)
        {
            var teacher = await _unitOfWork.Teachers.FindAsync(b => b.Id == id);
            if (teacher == null)
                return new Response<TeacherDto>
                {
                    Message = " المعلم غير موجود ",
                    Code = 400,
                };

            _unitOfWork.Teachers.Delete(teacher);
            _unitOfWork.Save();
            return new Response<TeacherDto>
            {
                Code = 200,
                Message = "تم الحذف",
                Data = teacher
            };

        }


        public async Task<Response<TeacherDto>> GetAllTeachers()
        {
            var teachers = await _unitOfWork.Teachers.GetAllAsync();


            var dataDisplay = teachers.Select(s => new TeacherDto
            {
                Id = s.Id,
                UserId = s.UserId,
                Salary = s.Salary,
                SpecialtyId = s.SpecialtyId,
            });

            return new Response<TeacherDto>
            {
                Message = "تم جلب المعلمين بنجاح ",
                Data = dataDisplay,
                Code = 200
            };

        }


        public async Task<Response<TeacherDto>> GetByIdTeacher(int id)
        {
            var teacher = await _unitOfWork.Teachers.GetByIdAsync(id);

            if (teacher == null)
            {
                return new Response<TeacherDto>
                {
                    Message = "المعلم غير موجود ",
                    Code = 400,
                };
            }

            return new Response<TeacherDto>
            {
                Message = "تم العثور على المعلم",
                Code = 200,
                Data = new TeacherDto
                {
                    Id = teacher.Id,
                    UserId = teacher.UserId,
                    Salary = teacher.Salary,
                    SpecialtyId = teacher.SpecialtyId,
                }
            };

        }


        public async Task<Response<TeacherDto>> UpdateTeacher(TeacherDto dto)
        {
            var teacher = await _unitOfWork.Teachers.FindAsync(b => b.Id == dto.Id);
            if (teacher == null)
                return new Response<TeacherDto>
                {
                    Message = "المعلم غير موجود ",
                    Code = 400,

                };


            teacher.UserId = dto.UserId;
            teacher.Salary = dto.Salary;
            teacher.SpecialtyId = dto.SpecialtyId;
            var TeacherNew = _unitOfWork.Teachers.Update(teacher);
            _unitOfWork.Save();
            return new Response<TeacherDto>
            {
                Code = 200,
                Message = "تم التعديل",
                Data = new TeacherDto
                {
                    Id = teacher.Id,
                    UserId = teacher.UserId,
                    Salary = teacher.Salary,
                    SpecialtyId = teacher.SpecialtyId,
                }
            };
        }
    }


    public interface ITeacherService
    {
        Task<Response<TeacherDto>> AddTeacher(TeacherDto dto);
        Task<Response<TeacherDto>> GetAllTeachers();
        Task<Response<TeacherDto>> GetByIdTeacher(int id);
        Task<Response<TeacherDto>> UpdateTeacher(TeacherDto dto);
        Task<Response<TeacherDto>> DeleteTeacher(int id);
    }
}
