
using Microsoft.EntityFrameworkCore;
using SmartSchool.Core;
using SmartSchool.Core.Models;
using SmartSchool.Core.Shared;
using SmartSchool.Main.Dtos;

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
                // 1. تأكد من جلب دور المعلم
                var role = await _unitOfWork.Roles.FindAsync(b => b.Name == "معلم")
                           ?? await _unitOfWork.Roles.FindAsync(b => b.Id == dto.RoleID);
                if (role == null)
                {
                    return new Response<TeacherDto>
                    {
                        Message = "دور 'معلم' غير موجود في النظام.",
                        Code = 400
                    };
                }

                // 2. ابحث عن المستخدم الحالي بالبريد الإلكتروني أو الاسم
                var existingUser = await _unitOfWork.Users.FindAsync(b => b.Email == dto.Email)
                                   ?? await _unitOfWork.Users.FindAsync(b => b.Name == dto.UserName && b.RoleId == role.Id);

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

                // 3. تحقق من وجود التخصص
                var specialty = await _unitOfWork.Specialtys.FindAsync(b => b.Id == dto.SpecialtyId);
                if (specialty == null)
                {
                    return new Response<TeacherDto>
                    {
                        Message = "التخصص غير موجود.",
                        Code = 400
                    };
                }

                // 4. إذا لم يكن المستخدم موجوداً، قم بإنشاء مستخدم جديد
                User userToAddOrUpdate;
                if (existingUser == null)
                {
                    userToAddOrUpdate = new User
                    {
                        Name = dto.UserName,
                        UserName = dto.Email,
                        Email = dto.Email,
                        PhoneNumber = dto.Phone,
                        DateOfBirth = dto.DateOfBirth,
                        gender = dto.gender,
                        Address = dto.Address,
                        RoleId = role.Id,
                        IsActive = dto.IsActive,
                        CreatedAt = DateTime.UtcNow
                    };
                    await _unitOfWork.Users.AddAsync(userToAddOrUpdate);
                }
                else
                {
                    userToAddOrUpdate = existingUser;
                    userToAddOrUpdate.RoleId = role.Id;
                }

                // 5. إنشاء المعلم
                Teacher addTeacher = new Teacher
                {
                    User = userToAddOrUpdate,
                    SpecialtyId = dto.SpecialtyId,
                    Salary = dto.Salary,
                    HireDate = DateTime.UtcNow
                };
                var teacherNew = await _unitOfWork.Teachers.AddAsync(addTeacher);

                // 6. الحفظ
                _unitOfWork.Save();

                return new Response<TeacherDto>
                {
                    Message = "تمت الإضافة بنجاح",
                    Data = new TeacherDto
                    {
                        Id = teacherNew.Id,
                        UserId = userToAddOrUpdate.Id,
                        UserName = userToAddOrUpdate.Name,
                        Email = userToAddOrUpdate.Email,
                        Phone = userToAddOrUpdate.PhoneNumber,
                        Address = userToAddOrUpdate.Address,
                        DateOfBirth = userToAddOrUpdate.DateOfBirth,
                        gender = userToAddOrUpdate.gender,
                        IsActive = userToAddOrUpdate.IsActive,
                        Salary = teacherNew.Salary,
                        RoleID = role.Id,
                        SpecialtyId = teacherNew.SpecialtyId
                    },
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
                        Phone = user.PhoneNumber,
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
                Phone = s.User.PhoneNumber,
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
                    Phone = user.PhoneNumber,
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

                var user = await _unitOfWork.Users.FindAsync(b => b.Id == teacher.UserId);
                if (user == null)
                    return new Response<TeacherDto>
                    {
                        Message = "المستخدم غير موجود ",
                        Code = 400,

                    };


                var role = await _unitOfWork.Roles.FindAsync(b => b.Name == "معلم")
                           ?? await _unitOfWork.Roles.FindAsync(b => b.Id == user.RoleId);
                if (role == null)
                    return new Response<TeacherDto>
                    {
                        Message = "دور 'معلم' غير موجود في النظام",
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
                teacher.UserId = teacher.UserId;
                teacher.Salary = dto.Salary;
                teacher.SpecialtyId = dto.SpecialtyId;

                //update user property
                user.Name = dto.UserName;
                user.UserName = dto.Email;
                user.Email = dto.Email;
                user.PhoneNumber = dto.Phone;
                user.DateOfBirth = dto.DateOfBirth;
                user.gender = dto.gender;
                user.Address = dto.Address;
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
                        Phone = user.PhoneNumber,
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
