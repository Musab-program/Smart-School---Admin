using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartSchool.Main.Dtos;
using SmartSchool.Core;
using SmartSchool.Core.Models;
using SmartSchool.Main.InterFaces;

namespace SmartSchool.Main.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
            return Ok(result);
        }

        [HttpGet("GetAssignmentById/{id}")]
        public async Task<IActionResult> GetAssignmentById (int id)
        {
            var result = await _assignmentService.GetAssignmentById(id);
            return Ok(result);
        }


        // *************************
        // From Here depend on Teacher
        //[HttpPost("AddAssignment")]
        //public async Task<IActionResult> AddAssignment([FromBody] AssignmentDto dto)
        //{
        //    var assignment = await _unitOfWork.Assignments.FindAsync(r => r.Id == dto.Id);
        //    if (assignment != null)
        //        return BadRequest("الواجب موجود من قبل");
            
        //    var subjectDetail = await _unitOfWork.SubjectDetails.FindAsync(r => r.Id == dto.Id);
        //        if (subjectDetail == null)
        //            return BadRequest("المادة الدراسية المدخلة غير صحيحة");
            
        //    var student = await _unitOfWork.Students.FindAsync(r => r.Id == dto.Id);
        //    if (subjectDetail == null)
        //        return BadRequest("الطالب المختار غير موجود");

        //    var addassignment = new Assignment
        //    {
        //        SubjectDetailId = dto.SubjectDetailId,
        //        StudentId = dto.StudentId,
        //        Title = dto.Title,
        //        LastDate = dto.LastDate,
        //        ChekeState = "Not Submitted",
        //        Mark = 0,
        //        SubmitedDate = DateTime.MinValue
        //    };

        //    var assignmentNew = await _unitOfWork.Assignments.AddAsync(addassignment);

        //    _unitOfWork.Save();
        //    return Ok("تم الإضافة بنجاح");
        //}
    }
}
