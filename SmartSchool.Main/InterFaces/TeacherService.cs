
using Microsoft.EntityFrameworkCore;
using SmartSchool.Core;
using SmartSchool.Core.Models;
using SmartSchool.Core.Shared;
using SmartSchool.Main.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

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
            //Response<TeacherDto> response;
            return await _unitOfWork.ExecuteInTransactionAsync<Response<TeacherDto>>(async () =>
            {
                // 1. ابحث عن المستخدم الحالي باستخدام اسمه والدور المطلوب.
                // هذا يضمن أنك تتعامل مع المستخدم الصحيح لكل دور.
                var existingUser = await _unitOfWork.Users.FindAsync(b => b.Name == dto.UserName && b.RoleId == dto.RoleID);

                // 2. إذا كان المستخدم موجوداً، تأكد من أنه لم يُضاف كمعلم من قبل.
                if (existingUser != null)
                {
                    var existingTeacher = await _unitOfWork.Teachers.FindAsync(b => b.UserId == existingUser.Id);
                    if (existingTeacher != null)
                    {
                        return new Response<TeacherDto>
                        {
                            Message = "المعلم موجود بالفعل.",
                            Code = 400
                        };
                    }
                }

                // 3. تأكد أن الدور المرسل موجود ونوعه "معلم".
                var role = await _unitOfWork.Roles.FindAsync(b => b.Id == dto.RoleID);
                if (role == null || role.Name != "معلم")
                {
                    return new Response<TeacherDto>
                    {
                        Message = "النوع غير موجود أو يجب أن يكون معلم.",
                        Code = 400
                    };
                }

                // 4. إذا لم يكن المستخدم موجودًا، قم بإنشاء واحد جديد.
                User userToAddOrUpdate;
                if (existingUser == null)
                {
                    userToAddOrUpdate = new User
                    {
                        Name = dto.UserName,
                        Email = dto.Email,
                        DateOfBirth = dto.DateOfBirth,
                        gender = dto.gender,
                        Address = dto.Address,
                        Password = Encoding.UTF8.GetBytes(dto.Password),
                        Phone = dto.Phone,
                        RoleId = dto.RoleID,
                        IsActive = dto.IsActive
                    };
                    // لا نحفظ هنا، بل نترك Entity Framework يقوم بتتبع الكائن.
                    await _unitOfWork.Users.AddAsync(userToAddOrUpdate);
                }
                else
                {
                    // إذا كان موجوداً، استخدمه لإضافة المعلم.
                    userToAddOrUpdate = existingUser;
                }

                // 5. تحقق من وجود التخصص.
                var specialty = await _unitOfWork.Specialtys.FindAsync(b => b.Id == dto.SpecialtyId);
                if (specialty == null)
                {
                    return new Response<TeacherDto>
                    {
                        Message = "التخصص غير موجود.",
                        Code = 400
                    };
                }

                // 6. أضف المعلم واربطه بالمستخدم.
                Teacher addTeacher = new Teacher
                {
                    User = userToAddOrUpdate, // 👈 اربط الكائن مباشرة
                    SpecialtyId = dto.SpecialtyId,
                    Salary = dto.Salary,
                };
                var teacherNew = await _unitOfWork.Teachers.AddAsync(addTeacher);

                // 7. احفظ جميع التغييرات مرة واحدة في نهاية العملية.
                _unitOfWork.Save();

                return new Response<TeacherDto>
                {
                    Message = "تمت الإضافة",
                    Data = teacherNew,
                    Code = 200
                };
            });

        }


        public async Task<Response<TeacherDto>> DeleteTeacher(int id)
        {
            return await _unitOfWork.ExecuteInTransactionAsync<Response<TeacherDto>>(async () =>
            {
                var teacher = await _unitOfWork.Teachers.FindAsync(b => b.Id == id);
                if (teacher == null)
                    return new Response<TeacherDto>
                    {
                        Message = " المعلم غير موجود ",
                        Code = 400,
                    };

                var user = await _unitOfWork.Users.FindAsync(b => b.Id == teacher.UserId);
                _unitOfWork.Users.Delete(user);
                _unitOfWork.Teachers.Delete(teacher);
                _unitOfWork.Save();
                return new Response<TeacherDto>
                {
                    Code = 200,
                    Message = "تم الحذف",
                    Data = new TeacherDto
                    {
                        Id = teacher.Id,
                        UserName = user.Name,
                        Email = user.Email,
                        UserId = teacher.UserId,
                        DateOfBirth = user.DateOfBirth,
                        Address = user.Address,
                        gender = user.gender,
                        IsActive = user.IsActive,
                        Phone = user.Phone,
                        Salary = teacher.Salary,
                        RoleID = user.RoleId,
                        SpecialtyId = teacher.SpecialtyId,
                    }
                };
            });
        }


        public async Task<Response<TeacherDto>> GetAllTeachers()
        {

            //you must ensure it is correct
            var teachers = await _unitOfWork.Teachers.FindAllAsync(b => b.Id >= 1, ["User"]);

            var dataDisplay = teachers.Select(s => new TeacherDto
            {
                Id = s.Id,
                UserId = s.UserId,
                Salary = s.Salary,
                SpecialtyId = s.SpecialtyId,
                UserName = s.User.Name,
                Email = s.User.Email,
                Phone = s.User.Phone,
                Address = s.User.Address,
                DateOfBirth = s.User.DateOfBirth,
                gender = s.User.gender,
                IsActive = s.User.IsActive,
                RoleID = s.User.RoleId
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
            var user = await _unitOfWork.Users.FindAsync(b => b.Id == teacher.UserId);

            return new Response<TeacherDto>
            {
                Code = 200,
                Message = "تم العرض",
                Data = new TeacherDto
                {
                    Id = teacher.Id,
                    UserName = user.Name,
                    Email = user.Email,
                    UserId = teacher.UserId,
                    DateOfBirth = user.DateOfBirth,
                    Address = user.Address,
                    gender = user.gender,
                    IsActive = user.IsActive,
                    Phone = user.Phone,
                    Salary = teacher.Salary,
                    RoleID = user.RoleId,
                    SpecialtyId = teacher.SpecialtyId,
                }
            };

        }


        public async Task<Response<TeacherDto>> UpdateTeacher(TeacherDto dto)
        {
            return await _unitOfWork.ExecuteInTransactionAsync<Response<TeacherDto>>(async () =>
            {
                var teacher = await _unitOfWork.Teachers.FindAsync(b => b.Id == dto.Id);
                if (teacher == null)
                    return new Response<TeacherDto>
                    {
                        Message = "المعلم غير موجود ",
                        Code = 400,

                    };

                var user = await _unitOfWork.Users.FindAsync(b => b.Id == dto.UserId);
                if (user == null)
                    return new Response<TeacherDto>
                    {
                        Message = "المستخدم غير موجود ",
                        Code = 400,

                    };


                var role = await _unitOfWork.Roles.FindAsync(b => b.Id == user.RoleId);
                //check dto.RoleId is not existed in role table or dto.RoleId exist in role table but its type not teacher
                if (role.Name != "معلم" || role == null)
                    return new Response<TeacherDto>
                    {
                        Message = "النوع غير موجود أو نوعه ليس معلما",
                        Code = 400
                    };

                var specialty = await _unitOfWork.Specialtys.FindAsync(b => b.Id == dto.SpecialtyId);
                if (specialty == null)
                {
                    return new Response<TeacherDto>
                    {
                        Message = "التخصص غير موجود",
                        Code = 400
                    };
                }


                //update teacher property
                teacher.UserId = dto.UserId;
                teacher.Salary = dto.Salary;
                teacher.SpecialtyId = dto.SpecialtyId;

                //update teacher property
                user.Name = dto.UserName;
                user.Email = dto.Email;
                user.DateOfBirth = dto.DateOfBirth;
                user.gender = dto.gender;
                user.Address = dto.Address;
                user.Password = Encoding.UTF8.GetBytes(dto.Password);
                user.Phone = dto.Phone;
                user.IsActive = dto.IsActive;

                var TeacherNew = _unitOfWork.Teachers.Update(teacher);
                var UserNew = _unitOfWork.Users.Update(user);
                _unitOfWork.Save();
                return new Response<TeacherDto>
                {
                    Code = 200,
                    Message = "تم التعديل",
                    Data = new TeacherDto
                    {
                        Id = teacher.Id,
                        UserName = user.Name,
                        Email = user.Email,
                        UserId = teacher.UserId,
                        DateOfBirth = user.DateOfBirth,
                        Address = user.Address,
                        gender = user.gender,
                        IsActive = user.IsActive,
                        Phone = user.Phone,
                        Salary = teacher.Salary,
                        RoleID = user.RoleId,
                        SpecialtyId = teacher.SpecialtyId,
                    }
                };

            });
        }

        public async Task<Response<int>> CountTeachers()
        {
            var TeacherCount = await _unitOfWork.Teachers.CountAsync();

            return new Response<int>
            {
                Message = "Success",
                Code = 200,
                Data = TeacherCount
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
        Task<Response<int>> CountTeachers();
    }

}
