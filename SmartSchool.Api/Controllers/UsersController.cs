using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartSchool.Main.Dtos;
using SmartSchool.Core;
using SmartSchool.Core.Interfaces;
using SmartSchool.Core.Models;

namespace SmartSchool.Main.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public UsersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var user = await _unitOfWork.Users.GetAllAsync();
            return Ok(user);
        }

        [HttpPost("AddUser")]
        public async Task<IActionResult> AddUser([FromBody] UserDto dto)
        {

            var user = await _unitOfWork.Users.FindAsync(a => a.Name == dto.UserName);
            if (user != null)
                return BadRequest("المستخدم موجود مسبقا");

            var role = await _unitOfWork.Roles.FindAsync(a => a.Name == dto.RoleName);
            if (role == null)
                return BadRequest("النوع المدخل للمستخدم غير صحيح");

            User addUser = new();
            addUser.RoleId = role.Id;

            var usernew = await _unitOfWork.Users.AddAsync(addUser);
            
            _unitOfWork.Save();
            return Ok(user);
        }


        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] UserDto dto)
        {

            var user = await _unitOfWork.Users.FindAsync(a => a.Id == dto.Id);
            if (user == null)
                return BadRequest("المستخدم غير موجود مسبقا");

            var role = await _unitOfWork.Roles.FindAsync(a => a.Name == dto.RoleName);
            if (role == null)
                return BadRequest("النوع المدخل للمستخدم غير صحيح");

            user.Name = dto.UserName;


            var usernew =  _unitOfWork.Users.Update(user);

            _unitOfWork.Save();

            return Ok(user);
        }

    }
}
