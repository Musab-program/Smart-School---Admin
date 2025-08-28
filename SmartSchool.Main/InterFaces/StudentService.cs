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
            return await _unitOfWork.ExecuteInTransactionAsync<Response<StudentDto>>(async () =>
            {

                var user = await _unitOfWork.Users.FindAsync(b => b.Id == dto.UserId);
                if (user == null)
                {
                    var role = await _unitOfWork.Roles.FindAsync(b => b.Id == dto.RoleId);
                    //check dto.RoleId is not existed in role table
                    if (role == null)
                        return new Response<StudentDto>
                        {
                            Message = "النوع غير موجود",
                            Code = 400
                        };
                    //check dto.RoleId exist in role table but its type not student  
                    else if (role.Name != "طالب")
                        return new Response<StudentDto>
                        {
                            Message = "يجب أن يكون النوع طالب",
                            Code = 400
                        };

                    //check dto.RoleId  exist in role table and its type student  
                    User addUser = new User
                    {
                        Name = dto.UserName,
                        Email = dto.UserEmail,
                        DateOfBirth = dto.UserDateOfBirth,
                        gender = dto.UserGender,
                        Address = dto.UserAddress,
                        Password = Encoding.UTF8.GetBytes(dto.UserPassword),
                        Phone = dto.UserPhone,
                        RoleId = dto.RoleId,
                        IsActive = dto.IsActiveUser

                    };

                    var UserNew = await _unitOfWork.Users.AddAsync(addUser);
                    _unitOfWork.Save();
                }

                else
                {
                    //user exist but we check if he student
                    var role = await _unitOfWork.Roles.FindAsync(b => b.Id == user.RoleId);
                    //check dto.RoleId is not existed in role table
                    if (role == null)
                        return new Response<StudentDto>
                        {
                            Message = "النوع غير موجود",
                            Code = 400
                        };
                    //check dto.RoleId exist in role table but its type not student  
                    else if (role.Name != "طالب")
                        return new Response<StudentDto>
                        {
                            Message = "يجب أن يكون النوع طالب",
                            Code = 400
                        };
                }


                var student = await _unitOfWork.Students.FindAsync(b => b.UserId == dto.UserId);
                //if the userId is exist in student table that means the student alrady existed
                if (student != null)
                    return new Response<StudentDto>
                    {
                        Message = "طالب موجود ",
                        Code = 400
                    };

                var guardian = await _unitOfWork.Guardians.FindAsync(b => b.Id == dto.GuardianId);
                if (guardian == null)
                    return new Response<StudentDto>
                    {
                        Message = "القريب غير موجود",
                        Code = 400
                    };

                var group = await _unitOfWork.Groups.FindAsync(b => b.Id == dto.GroupId);
                if (group == null)
                    return new Response<StudentDto>
                    {
                        Message = "المجموعة غير موجودة",
                        Code = 400
                    };


                Student addstudent = new Student
                {
                    UserId = dto.UserId,
                    GuardianId = dto.GuardianId,
                    GroupId = dto.GroupId,
                    RegisterDate = dto.RegisterDate,
                };
                var studentNew = await _unitOfWork.Students.AddAsync(addstudent);
                _unitOfWork.Save();
                return new Response<StudentDto>
                {
                    Message = "تمت الاصافة",
                    Data = studentNew,
                    Code = 200
                };

            });

        }

        public async Task<Response<StudentDto>> DeleteStudent(int id)
        {
            return await _unitOfWork.ExecuteInTransactionAsync<Response<StudentDto>>(async () =>
            {
                var student = await _unitOfWork.Students.FindAsync(b => b.Id == id);
                if (student == null)
                    return new Response<StudentDto>
                    {
                        Message = " طالب غير موجود ",
                        Code = 400,
                    };

                var user = await _unitOfWork.Users.FindAsync(b => b.Id == student.UserId);
                _unitOfWork.Users.Delete(user);
                _unitOfWork.Students.Delete(student);
                _unitOfWork.Save();
                return new Response<StudentDto>
                {
                    Code = 200,
                    Message = "تم الحذف",
                    Data = new StudentDto
                    {
                        Id = student.Id,
                        UserName = user.Name,
                        UserEmail = user.Email,
                        UserId = student.UserId,
                        UserDateOfBirth = user.DateOfBirth,
                        UserAddress = user.Address,
                        UserGender = user.gender,
                        IsActiveUser = user.IsActive,
                        UserPhone = user.Phone,
                        RoleId = user.RoleId,
                        GuardianId = student.GuardianId,
                        GroupId = student.GroupId,
                    }
                };
            });
        }


        public async Task<Response<StudentDto>> GetAllStudents()
        {

            //you must ensure it is correct
            var students = await _unitOfWork.Students.FindAllAsync(b => b.Id >= 1, ["User"]);

            var dataDisplay = students.Select(s => new StudentDto
            {
                Id = s.Id,
                UserId = s.UserId,
                GuardianId = s.GuardianId,
                GroupId = s.GroupId,
                UserName = s.User.Name,
                UserEmail = s.User.Email,
                UserPhone = s.User.Phone,
                UserAddress = s.User.Address,
                UserDateOfBirth = s.User.DateOfBirth,
                UserGender = s.User.gender,
                IsActiveUser = s.User.IsActive,
                RoleId = s.User.RoleId,
            });

            return new Response<StudentDto>
            {
                Message = "تم جلب الطلاب بنجاح ",
                Data = dataDisplay,
                Code = 200
            };

        }


        public async Task<Response<StudentDto>> GetByIdStudent(int id)
        {
            var student = await _unitOfWork.Students.GetByIdAsync(id);

            if (student == null)
            {
                return new Response<StudentDto>
                {
                    Message = "الطالب غير موجود ",
                    Code = 400,
                };
            }
            var user = await _unitOfWork.Users.FindAsync(b => b.Id == student.UserId);

            return new Response<StudentDto>
            {
                Code = 200,
                Message = "تم جلب الطالب بالرقم التعريفي",
                Data = new StudentDto
                {
                    Id = student.Id,
                    UserId = student.UserId,
                    GroupId = student.GroupId,
                    GuardianId = student.GuardianId,
                    UserName = user.Name,
                    UserEmail = user.Email,
                    UserDateOfBirth = user.DateOfBirth,
                    UserAddress = user.Address,
                    UserGender = user.gender,
                    IsActiveUser = user.IsActive,
                    UserPhone = user.Phone,
                    RoleId = user.RoleId,
                }
            };

        }


        public async Task<Response<StudentDto>> UpdateStudent(StudentDto dto)
        {
            return await _unitOfWork.ExecuteInTransactionAsync<Response<StudentDto>>(async () =>
            {
                var student = await _unitOfWork.Students.FindAsync(b => b.Id == dto.Id);
                if (student == null)
                    return new Response<StudentDto>
                    {
                        Message = "طالب غير موجود ",
                        Code = 400,

                    };

                var user = await _unitOfWork.Users.FindAsync(b => b.Id == dto.UserId);
                if (user == null)
                    return new Response<StudentDto>
                    {
                        Message = "المستخدم غير موجود ",
                        Code = 400,

                    };


                var role = await _unitOfWork.Roles.FindAsync(b => b.Id == user.RoleId);
                //check dto.RoleId is not existed in role table or dto.RoleId exist in role table but its type not Student
                if (role.Name != "طالب" || role == null)
                    return new Response<StudentDto>
                    {
                        Message = "النوع غير موجود أو ليس طالبا",
                        Code = 400
                    };

                var guardian = await _unitOfWork.Guardians.FindAsync(b => b.Id == dto.GuardianId);
                if (guardian == null)
                    return new Response<StudentDto>
                    {
                        Message = "ولي الأمر غير موجود",
                        Code = 400
                    };

                var group = await _unitOfWork.Groups.FindAsync(b => b.Id == dto.GroupId);
                if (group == null)
                    return new Response<StudentDto>
                    {
                        Message = "الشعبة غير موجود",
                        Code = 400
                    };



                //update Student property
                student.UserId = dto.UserId;
                student.GuardianId = dto.GuardianId;
                student.GroupId = dto.GroupId;
                student.RegisterDate = dto.RegisterDate;

                //update Student property
                user.Name = dto.UserName;
                user.Email = dto.UserEmail;
                user.DateOfBirth = dto.UserDateOfBirth;
                user.gender = dto.UserGender;
                user.Address = dto.UserAddress;
                user.Password = Encoding.UTF8.GetBytes(dto.UserPassword);
                user.Phone = dto.UserPhone;
                user.IsActive = dto.IsActiveUser;

                var StudentNew = _unitOfWork.Students.Update(student);
                var UserNew = _unitOfWork.Users.Update(user);
                _unitOfWork.Save();
                return new Response<StudentDto>
                {
                    Code = 200,
                    Message = "تم التعديل",
                    Data = new StudentDto
                    {
                        Id = student.Id,
                        UserName = user.Name,
                        UserEmail = user.Email,
                        UserId = student.UserId,
                        UserDateOfBirth = user.DateOfBirth,
                        RegisterDate = student.RegisterDate,
                        UserAddress = user.Address,
                        UserGender = user.gender,
                        IsActiveUser = user.IsActive,
                        UserPhone = user.Phone,
                        GuardianId = student.GuardianId,
                        RoleId = user.RoleId,
                        GroupId = student.GroupId,
                    }
                };

            });
        }

    }


    public interface IStudentService
    {

        Task<Response<StudentDto>> AddStudent(StudentDto dto);
        Task<Response<StudentDto>> GetAllStudents();
        Task<Response<StudentDto>> GetByIdStudent(int id);
        Task<Response<StudentDto>> UpdateStudent(StudentDto dto);
        Task<Response<StudentDto>> DeleteStudent(int id);

    }
}
