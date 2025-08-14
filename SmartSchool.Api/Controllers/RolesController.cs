using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartSchool.Core;
using SmartSchool.Main.Dtos;
using SmartSchool.Core.Models;
using SmartSchool.Main.InterFaces;

namespace SmartSchool.Main.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        // An object is declared to handle database operations as a single unit.
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRoleService _roleService;

        // The constructor for the controller 
        public RolesController(IUnitOfWork unitOfWork, IRoleService roleService)
        {
            _unitOfWork = unitOfWork;
            _roleService = roleService;
        }

        // End Point For Get All Elements In This Domin Class
        [HttpGet("GetAllRoles")]
        public async Task<IActionResult> GetAllRoles()
        {
            var result = await _roleService.GetAllRoles();
            return Ok(result);
        }

        // End Point For Get Element By Id In This Domin Class
        [HttpGet("GetByIdRole/{id}")]
        public async Task<IActionResult> GetByIdRole(int id)
        {
            var result = await _roleService.GetByIdRole(id);
            return Ok(result);
        }


        // End Point To Add Element In This Domin Class
        [HttpPost("AddRole")]
        public async Task<IActionResult> AddRole([FromBody] RoleDto dto)
        {
            var result = await _roleService.AddRole(dto);
            return Ok(result);
        }

        // End Point To Update Element In This Domin Class
        [HttpPut("UpdateRole")]
        public async Task<IActionResult> UpdateRole([FromBody] RoleDto dto)
        {
            var result = await _roleService.UpdateRole(dto);
            return Ok(result);
        }

        // End Point To Delete Element In This Domin Class
        [HttpDelete("DeleteRole")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var result = await _roleService.DeleteRole(id);
            return Ok(result);        
        }
    }
}
