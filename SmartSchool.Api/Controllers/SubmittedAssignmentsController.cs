using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartSchool.Core;
using SmartSchool.Core.Models;
using SmartSchool.Main.InterFaces;
using SmartSchool.Main.Dtos;

namespace SmartSchool.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubmittedAssignmentsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISubmittedAssignmentService _submittedAssignmentService;

        // The constructor for the controller
        public SubmittedAssignmentsController(IUnitOfWork unitOfWork, ISubmittedAssignmentService submittedAssignmentService)
        {
            _unitOfWork = unitOfWork;
            _submittedAssignmentService = submittedAssignmentService;
        }

        // End Point For add Element In This Domin Class
        [HttpPost("AddSubmittedAssignment")]
        public async Task<IActionResult> AddSubmittedAssignment([FromBody] SubmittedAssignmentDto dto)
        {
            var result = await _submittedAssignmentService.AddSubmittedAssignment(dto);
            return Ok(result);
        }


        // End Point For Get All Elements In This Domin Class
        [HttpGet("GetAllSubmittedAssignment")]
        public async Task<IActionResult> GetAllSubmittedAssignment()
        {
            var result = await _submittedAssignmentService.GetAllSubmittedAssignments();
            return Ok(result);
        }


        // End Point For Get  Element by id In This Domin Class
        [HttpGet("GetSubmittedAssignmentById")]
        public async Task<IActionResult> GetSubmittedAssignmentById(int id)
        {
            var result = await _submittedAssignmentService.GetByIdSubmittedAssignment(id);
            return Ok(result);
        }


        // End Point For update Elements In This Domin Class
        [HttpPut("UpdateSubmittedAssignments")]
        public async Task<IActionResult> UpdateSubmittedAssignments([FromBody] SubmittedAssignmentDto dto)
        {
            var result = await _submittedAssignmentService.UpdateSubmittedAssignment(dto);
            return Ok(result);
        }


        // End Point For delete Element In This Domin Class
        [HttpDelete("DeleteSubmittedAssignment")]
        public async Task<IActionResult> DeleteSubmittedAssignment(int id)
        {
            var result = await _submittedAssignmentService.DeleteSubmittedAssignment(id);
            return Ok(result);
        }
    }
}
