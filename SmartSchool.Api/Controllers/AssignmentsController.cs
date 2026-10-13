using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartSchool.Main.Dtos;
using SmartSchool.Core;
using SmartSchool.Core.Models;
using SmartSchool.Main.InterFaces;

namespace SmartSchool.Main.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AssignmentsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAssignmentService _assignmentService;

        public AssignmentsController(IUnitOfWork unitOfWork, IAssignmentService assignmentService)
        {
            _unitOfWork = unitOfWork;
            _assignmentService = assignmentService;
        }

        [HttpGet("GetAllAssignments")]
        public async Task<IActionResult> GetAllAssignments()
        {
            var result = await _assignmentService.GetAllAssignments();
            return result.Code != 200 ? BadRequest(result) : Ok(result);
        }

        [HttpGet("GetAssignmentById/{id}")]
        public async Task<IActionResult> GetAssignmentById (int id)
        {
            var result = await _assignmentService.GetAssignmentById(id);
            return result.Code != 200 ? BadRequest(result) : Ok(result);
        }

        [HttpPost("AddAssignment")]
        public async Task<IActionResult> AddAssignment([FromBody] AssignmentDto dto)
        {
            var result = await _assignmentService.AddAssignment(dto);
            return result.Code != 200 ? BadRequest(result) : Ok(result);
        }

        [HttpPut("UpdateAssignment")]
        public async Task<IActionResult> UpdateAssignment([FromBody] AssignmentDto dto)
        {
            var result = await _assignmentService.UpdateAssignment(dto);
            return result.Code != 200 ? BadRequest(result) : Ok(result);
        }

        [HttpDelete("DeleteAssignment/{id}")]
        public async Task<IActionResult> DeleteAssignment(int id)
        {
            var result = await _assignmentService.DeleteAssignment(id);
            return result.Code != 200 ? BadRequest(result) : Ok(result);
        }
    }
}
