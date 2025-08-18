using SmartSchool.Main.Dtos;
using Microsoft.AspNetCore.Mvc;
using SmartSchool.Core;
using SmartSchool.Core.Models;
using Microsoft.AspNetCore.Http;
using SmartSchool.Core.Interfaces;
using SmartSchool.Main.InterFaces;

namespace SmartSchool.Main.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UsersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;

        // The constructor for the controller
        public UsersController(IUnitOfWork unitOfWork, IUserService usersService)
        {
            _unitOfWork = unitOfWork;
            _userService = usersService;
        }

        // End Point For add Element In This Domin Class
        [HttpPost("AddUser")]
        public async Task<IActionResult> AddUsers([FromBody] UserDto dto)
        {
            var result = await _userService.AddUser(dto);
            return Ok(result);
        }


        // End Point For Get All Elements In This Domin Class
        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _userService.GetAllUsers();
            return Ok(result);
        }


        // End Point For Get  Element by id In This Domin Class
        [HttpGet("GetUserById")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var result = await _userService.GetByIdUser(id);
            return Ok(result);
        }


        // End Point For update Elements In This Domin Class
        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUserss([FromBody] UserDto dto)
        {
            var result = await _userService.UpdateUser(dto);
            return Ok(result);
        }


        // End Point For delete Element In This Domin Class
        [HttpDelete("DeleteUser")]
        public async Task<IActionResult> DeleteUseru(int id)
        {
            var result = await _userService.DeleteUser(id);
            return Ok(result);
        }
    }
}
