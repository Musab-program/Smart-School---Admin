using SmartSchool.Main.Dtos;
using SmartSchool.Core;
using SmartSchool.Core.Models;
using SmartSchool.Core.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSchool.Main.InterFaces
{
    public class StudentService : IStudentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public StudentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<StudentDto>> AddStudent(StudentDto dto)
        {
            var user = await _unitOfWork.Users.FindAsync(n => n.Id == dto.UserId);
            if (user.Id == null)
                return new Response<StudentDto>
                {
                    Message = "هذا المستخدم غير موجود",
                    Code = 400
                };

            var group = await _unitOfWork.Groups.FindAsync(n => n.Id == dto.GroupId);
            if (group.Id == null)
            {
                return new Response<StudentDto>
                {
                    Message = "لا يوجد شعبة بهذا الرقم",
                    Code = 400
                };
            }

            var guardian = await _unitOfWork.Guardians.FindAsync(n => n.Id == dto.GuardianId);
            if (guardian.Id == null)
            {
                return new Response<StudentDto>
                {
                    Message = "لا يوجد قريب بهذا الرقم",
                    Code = 400
                };
            }

            var student = await _unitOfWork.Students.FindAsync(b => b.Id == dto.Id);
            if (student != null)
                return new Response<StudentDto>
                {
                    Message = "الطالب موجود مسبقا",
                    Code = 400
                };

            Student addStudent = new Student
            {
                RegisterDate = dto.RegisterDate,
                UserId = dto.UserId,
            };
            var StudentNew = await _unitOfWork.Students.AddAsync(addStudent);
            _unitOfWork.Save();
            return new Response<StudentDto>
            {
                Message = "تمت الاصافة",
                Data = StudentNew,
                Code = 200
            };
        }

        public async Task<Response<StudentDto>> DeleteStudent(int id)
        {
            var Student = await _unitOfWork.Students.FindAsync(b => b.Id == id);
            if (Student == null)
                return new Response<StudentDto>
                {
                    Message = " التخصص غير موجود ",
                    Code = 400,
                };

            _unitOfWork.Students.Delete(Student);
            _unitOfWork.Save();
            return new Response<StudentDto>
            {
                Code = 200,
                Message = "تم الحذف",
                Data = Student
            };
        }

        //public async Task<Response<StudentDto>> GetAllStudent()
        //{
        //    var specialties = await _unitOfWork.Students.GetAllAsync();


        //    var dataDisplay = specialties.Select(s => new StudentDto
        //    {
        //        Id = s.Id,
        //        Name = s.Name,
        //        Qualification = s.Qualification,
        //    });

        //    return new Response<StudentDto>
        //    {
        //        Message = "تم جلب التخصصات بنجاح ",
        //        Data = dataDisplay,
        //        Code = 200
        //    };
        //}

        //public async Task<Response<StudentDto>> GetByIdStudent(int id)
        //{
        //    var Student = await _unitOfWork.Students.GetByIdAsync(id);

        //    if (Student == null)
        //    {
        //        return new Response<StudentDto>
        //        {
        //            Message = "التخصص غير موجود ",
        //            Code = 400,
        //        };
        //    }

        //    return new Response<StudentDto>
        //    {
        //        Message = "تم العثور على التخصص",
        //        Code = 200,
        //        Data = new StudentDto
        //        {
        //            Id = Student.Id,
        //            Name = Student.Name,
        //            Qualification = Student.Qualification
        //        }
        //    };
        //}

        //public async Task<Response<StudentDto>> UpdateStudent(StudentDto dto)
        //{
        //    var Student = await _unitOfWork.Students.FindAsync(b => b.Id == dto.Id);
        //    if (Student == null)
        //        return new Response<StudentDto>
        //        {
        //            Message = "التخصص غير موجود ",
        //            Code = 400,

        //        };


        //    Student.Name = dto.Name;
        //    Student.Qualification = dto.Qualification;
        //    var StudentNew = _unitOfWork.Students.Update(Student);
        //    _unitOfWork.Save();
        //    return new Response<StudentDto>
        //    {
        //        Code = 200,
        //        Message = "تم التعديل",
        //        Data = new StudentDto
        //        {
        //            Id = Student.Id,
        //            Name = Student.Name,
        //            Qualification = Student.Qualification
        //        }
        //    };
        //}
    }


    public interface IStudentService
    {
        Task<Response<StudentDto>> AddStudent(StudentDto dto);
        //Task<Response<StudentDto>> GetAllStudent();
        //Task<Response<StudentDto>> GetByIdStudent(int id);
        //Task<Response<StudentDto>> UpdateStudent(StudentDto dto);
        //Task<Response<StudentDto>> DeleteStudent(int id);
    }
}
