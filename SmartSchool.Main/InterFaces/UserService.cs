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
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public  async  Task<Response<UserDto>> AddUser(UserDto dto)
        {

            var role = await _unitOfWork.Roles.FindAsync(b => b.Id == dto.RoleID);
            if (role == null)
                return new Response<UserDto>
                {
                    Message = "النوع غير موجود",
                    Code = 400
                };

            var user = await _unitOfWork.Users.FindAsync(b => b.Id == dto.Id);
            if (user != null)
                return new Response<UserDto>
                {
                    Message = "المستخدم موجود مسبقا",
                    Code = 400
                };


            User addUser = new User
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
            var UserNew = await _unitOfWork.Users.AddAsync(addUser);
            _unitOfWork.Save();
            return new Response<UserDto>
            {
                Message = "تمت الاصافة",
                Data = UserNew,
                Code = 200
            };
        }

        public async Task<Response<UserDto>> DeleteUser(int id)
        {
            var user = await _unitOfWork.Users.FindAsync(b => b.Id == id);
            if (user == null)
                return new Response<UserDto>
                {
                    Message = " المستخدم غير موجود ",
                    Code = 400,
                };

            _unitOfWork.Users.Delete(user);
            _unitOfWork.Save();
            return new Response<UserDto>
            {
                Code = 200,
                Message = "تم الحذف",
                Data = user
            };

        }


        public async Task<Response<UserDto>> GetAllUsers()
        {
            var users = await _unitOfWork.Users.GetAllAsync();


            var dataDisplay = users.Select(s => new UserDto
            {
                Id = s.Id,
                UserName = s.Name,
                Email = s.Email,
                DateOfBirth = s.DateOfBirth,
                gender = s.gender,
                Address = s.Address,
                Phone = s.Phone,
                RoleID = s.RoleId,
                IsActive = s.IsActive

            });

            return new Response<UserDto>
            {
                Message = "تم جلب المستخدمين بنجاح ",
                Data = dataDisplay,
                Code = 200
            };

        }


        public async Task<Response<UserDto>> GetByIdUser(int id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);

            if (user == null)
            {
                return new Response<UserDto>
                {
                    Message = "المستخدم غير موجود ",
                    Code = 400,
                };
            }

            return new Response<UserDto>
            {
                Message = "تم العثور على المستخدم",
                Code = 200,
                Data = new UserDto
                {
                    Id = user.Id,
                    UserName = user.Name,
                    Email = user.Email,
                    DateOfBirth = user.DateOfBirth,
                    gender = user.gender,
                    Address = user.Address,
                    Phone = user.Phone,
                    RoleID = user.RoleId,
                    IsActive = user.IsActive
                }
            };

        }


        public async Task<Response<UserDto>> UpdateUser(UserDto dto)
        {
            var user = await _unitOfWork.Users.FindAsync(b => b.Id == dto.Id);
            if (user == null)
                return new Response<UserDto>
                {
                    Message = "المستخدم غير موجود ",
                    Code = 400,

                };


            user.Name = dto.UserName;
            user.Email = dto.Email;
            user.DateOfBirth = dto.DateOfBirth;
            user.gender = dto.gender;
            user.Address = dto.Address;
            user.Password = Encoding.UTF8.GetBytes(dto.Password);
            user.Phone = dto.Phone;
            user.RoleId = dto.RoleID;
            user.IsActive = dto.IsActive;
            var UserNew = _unitOfWork.Users.Update(user);
            _unitOfWork.Save();
            return new Response<UserDto>
            {
                Code = 200,
                Message = "تم التعديل",
                Data = new UserDto
                {
                    Id = user.Id,
                    UserName = user.Name,
                    Email = user.Email,
                    DateOfBirth = user.DateOfBirth,
                    gender = user.gender,
                    Address = user.Address,
                    Phone = user.Phone,
                    RoleID = user.RoleId,
                    IsActive = user.IsActive
                }
            };
        }
    }

    public interface IUserService
    {
        Task<Response<UserDto>> AddUser(UserDto dto);
        Task<Response<UserDto>> GetAllUsers();
        Task<Response<UserDto>> GetByIdUser(int id);
        Task<Response<UserDto>> UpdateUser(UserDto dto);
        Task<Response<UserDto>> DeleteUser(int id);
    }
}
