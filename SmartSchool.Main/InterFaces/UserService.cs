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
    /// <summary>
    /// This service provides functionalities for managing users, including
    /// adding, updating, deleting, retrieving, and counting user records.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work for database operations.</param>
        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        /// <summary>
        /// Adds a new user to the database.
        /// </summary>
        /// <param name="dto">The data transfer object containing the details of the new user.</param>
        /// <returns>
        /// A response object containing the newly created user's data on success,
        /// or an error message if the role does not exist.
        /// </returns>
        public async  Task<Response<UserDto>> AddUser(UserDto dto)
        {

            var role = await _unitOfWork.Roles.FindAsync(b => b.Id == dto.RoleID);
            if (role == null)
                return new Response<UserDto>
                {
                    Message = "النوع غير موجود",
                    Code = 400
                };
            
            // the user is not uniq because we need to add a user as teeacher or guardian
            //var user = await _unitOfWork.Users.FindAsync(b => b.Id == dto.Id);
            //if (user != null)
            //    return new Response<UserDto>
            //    {
            //        Message = "المستخدم موجود مسبقا",
            //        Code = 400
            //    };


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



        /// <summary>
        /// Deletes a user from the database by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier (ID) of the user to be deleted.</param>
        /// <returns>
        /// A response object confirming the successful deletion,
        /// or an error message if the user is not found.
        /// </returns>
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


        /// <summary>
        /// Retrieves a list of all users from the database.
        /// </summary>
        /// <returns>
        /// A response object containing a list of user data transfer objects (UserDto),
        /// or an empty list if no users are found.
        /// </returns>
        public async Task<Response<UserDto>> GetAllUsers()
        {
            var users = await _unitOfWork.Users.GetAllAsync();

            //StringBuilder sp = new StringBuilder();
            //sp.AppendLine("select * from vewUsers ");


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


        /// <summary>
        /// Retrieves a user from the database by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier (ID) of the user.</param>
        /// <returns>
        /// A response object containing the user's data on success,
        /// or an error message if the user is not found.
        /// </returns>
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


        /// <summary>
        /// Updates an existing user in the database.
        /// </summary>
        /// <param name="dto">The data transfer object containing the updated user information.</param>
        /// <returns>
        /// A response object with the updated user's data,
        /// or an error message if the user is not found.
        /// </returns>
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


        /// <summary>
        /// Counts the total number of users in the database.
        /// </summary>
        /// <returns>A response object containing the total count of users.</returns>
        public async Task<Response<int>> CountUsers()
        {
            var UserCount = await _unitOfWork.Users.CountAsync();

            return new Response<int>
            {
                Message = "Success",
                Code = 200,
                Data = UserCount
            };
        }
    }

    public interface IUserService
    {
        /// <summary>
        /// Adds a new user.
        /// </summary>
        /// <param name="dto">The user data transfer object.</param>
        /// <returns>A response object with the added user's details.</returns>
        Task<Response<UserDto>> AddUser(UserDto dto);

        /// <summary>
        /// Retrieves all users.
        /// </summary>
        /// <returns>A response object containing a list of all users.</returns>
        Task<Response<UserDto>> GetAllUsers();

        /// <summary>
        /// Retrieves a user by their ID.
        /// </summary>
        /// <param name="id">The ID of the user to retrieve.</param>
        /// <returns>A response object with the user's details.</returns>
        Task<Response<UserDto>> GetByIdUser(int id);

        /// <summary>
        /// Updates an existing user.
        /// </summary>
        /// <param name="dto">The user data transfer object with updated information.</param>
        /// <returns>A response object with the updated user's details.</returns>
        Task<Response<UserDto>> UpdateUser(UserDto dto);

        /// <summary>
        /// Deletes a user by their ID.
        /// </summary>
        /// <param name="id">The ID of the user to delete.</param>
        /// <returns>A response object confirming the deletion.</returns>
        Task<Response<UserDto>> DeleteUser(int id);

        /// <summary>
        /// Counts the total number of users.
        /// </summary>
        /// <returns>A response object containing the total count.</returns>
        Task<Response<int>> CountUsers();
    }
}
