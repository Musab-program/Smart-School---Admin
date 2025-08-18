using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartSchool.Core;
using SmartSchool.Main.Dtos;
using SmartSchool.Main.InterFaces;

namespace SmartSchool.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        // An object is declared to handle database operations as a single unit.
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGroupService _groupService;

        // The constructor for the controller 
        public GroupController(IUnitOfWork unitOfWork, IGroupService groupService)
        {
            _unitOfWork = unitOfWork;
            _groupService = groupService;
        }

        // End Point For Get All Elements In This Domin Class
        [HttpGet("GetAllGroups")]
        public async Task<IActionResult> GetAllGroups()
        {
            var result = await _groupService.GetAllGroups();
            return Ok(result);
        }

        // End Point For Get Element By Id In This Domin Class
        [HttpGet("GetGroupById/{id}")]
        public async Task<IActionResult> GetGroupById(int id)
        {
            var result = await _groupService.GetGroupById(id);
            return Ok(result);
        }


        // End Point To Add Element In This Domin Class
        [HttpPost("AddGroup")]
        public async Task<IActionResult> AddGroup([FromBody] GroupDto dto)
        {
            var result = await _groupService.AddGroup(dto);
            return Ok(result);
        }

        // End Point To Update Element In This Domin Class
        [HttpPut("UpdateGroup")]
        public async Task<IActionResult> UpdateGroup([FromBody] GroupDto dto)
        {
            var result = await _groupService.UpdateGroup(dto);
            return Ok(result);
        }

        // End Point To Delete Element In This Domin Class
        [HttpDelete("DeleteGroup")]
        public async Task<IActionResult> DeleteGroup(int id)
        {
            var result = await _groupService.DeleteGroup(id);
            return Ok(result);
        }
    }
}
