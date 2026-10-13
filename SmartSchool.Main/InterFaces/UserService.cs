using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Main.Dtos;
using SmartSchool.Core;
using SmartSchool.Core.Models;
using SmartSchool.Core.Shared;

namespace SmartSchool.Main.InterFaces
{
    /// <summary>
    /// Manages users using ASP.NET Core Identity's UserManager.
    /// Passwords are correctly hashed by Identity (PBKDF2/SHA-256).
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;

        public UserService(IUnitOfWork unitOfWork, UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        /// <summary>
        /// Adds a new user using Identity's CreateAsync (handles password hashing).
        /// </summary>
        public async Task<Response<UserDto>> AddUser(UserDto dto)
        {
            var role = await _unitOfWork.Roles.FindAsync(b => b.Id == dto.RoleID);
            if (role == null)
                return new Response<UserDto> { Message = "الدور غير موجود", Code = 400 };

            var user = new User
            {
                Name = dto.UserName,
                UserName = dto.Email,   // Identity uses UserName for login
                Email = dto.Email,
                PhoneNumber = dto.Phone,
                DateOfBirth = dto.DateOfBirth,
                gender = dto.gender,
                Address = dto.Address,
                RoleId = dto.RoleID,
                IsActive = dto.IsActive,
            };

            // Identity hashes the password using PBKDF2 - never store plain text
            var result = await _userManager.CreateAsync(user, dto.PasswordInput ?? string.Empty);
            if (!result.Succeeded)
                return new Response<UserDto>
                {
                    Message = string.Join(", ", result.Errors.Select(e => e.Description)),
                    Code = 400
                };

            _unitOfWork.Save();
            return new Response<UserDto>
            {
                Message = "تمت الإضافة",
                Data = MapToDto(user),
                Code = 200
            };
        }

        /// <summary>
        /// Deletes a user by ID.
        /// </summary>
        public async Task<Response<UserDto>> DeleteUser(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
                return new Response<UserDto> { Message = "المستخدم غير موجود", Code = 400 };

            await _userManager.DeleteAsync(user);
            return new Response<UserDto> { Code = 200, Message = "تم الحذف", Data = MapToDto(user) };
        }

        /// <summary>
        /// Retrieves all users.
        /// </summary>
        public async Task<Response<UserDto>> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            var dataDisplay = users.Select(MapToDto);
            return new Response<UserDto>
            {
                Message = "تم جلب المستخدمين بنجاح",
                Data = dataDisplay,
                Code = 200
            };
        }

        /// <summary>
        /// Retrieves a user by their ID.
        /// </summary>
        public async Task<Response<UserDto>> GetByIdUser(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
                return new Response<UserDto> { Message = "المستخدم غير موجود", Code = 400 };

            return new Response<UserDto>
            {
                Message = "تم العثور على المستخدم",
                Code = 200,
                Data = MapToDto(user)
            };
        }

        /// <summary>
        /// Updates user info. If PasswordInput is provided, changes the password using Identity.
        /// </summary>
        public async Task<Response<UserDto>> UpdateUser(UserDto dto)
        {
            var user = await _userManager.FindByIdAsync(dto.Id.ToString());
            if (user == null)
                return new Response<UserDto> { Message = "المستخدم غير موجود", Code = 400 };

            user.Name = dto.UserName;
            user.Email = dto.Email;
            user.UserName = dto.Email;
            user.PhoneNumber = dto.Phone;
            user.DateOfBirth = dto.DateOfBirth;
            user.gender = dto.gender;
            user.Address = dto.Address;
            user.RoleId = dto.RoleID;
            user.IsActive = dto.IsActive;

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
                return new Response<UserDto>
                {
                    Message = string.Join(", ", updateResult.Errors.Select(e => e.Description)),
                    Code = 400
                };

            // Change password only if provided
            if (!string.IsNullOrWhiteSpace(dto.PasswordInput))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var passResult = await _userManager.ResetPasswordAsync(user, token, dto.PasswordInput);
                if (!passResult.Succeeded)
                    return new Response<UserDto>
                    {
                        Message = string.Join(", ", passResult.Errors.Select(e => e.Description)),
                        Code = 400
                    };
            }

            return new Response<UserDto> { Code = 200, Message = "تم التعديل", Data = MapToDto(user) };
        }

        /// <summary>
        /// Counts total users.
        /// </summary>
        public async Task<Response<int>> CountUsers()
        {
            var count = await _userManager.Users.CountAsync();
            return new Response<int> { Message = "Success", Code = 200, Data = count };
        }

        // ────────────────────────────────────────────────────────────
        // Mapping helper
        // ────────────────────────────────────────────────────────────
        private static UserDto MapToDto(User u) => new UserDto
        {
            Id = u.Id,
            UserName = u.Name,
            Email = u.Email,
            Phone = u.PhoneNumber,
            DateOfBirth = u.DateOfBirth,
            gender = u.gender,
            Address = u.Address,
            RoleID = u.RoleId,
            IsActive = u.IsActive
            // PasswordInput intentionally omitted from responses
        };
    }

    public interface IUserService
    {
        Task<Response<UserDto>> AddUser(UserDto dto);
        Task<Response<UserDto>> GetAllUsers();
        Task<Response<UserDto>> GetByIdUser(int id);
        Task<Response<UserDto>> UpdateUser(UserDto dto);
        Task<Response<UserDto>> DeleteUser(int id);
        Task<Response<int>> CountUsers();
    }
}
