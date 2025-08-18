using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartSchool.Core;
using SmartSchool.Main.Dtos;
using SmartSchool.Main.InterFaces;

namespace SmartSchool.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamController : ControllerBase
    {
        // An object is declared to handle database operations as a single unit.
        private readonly IUnitOfWork _unitOfWork;
        private readonly IExamService _examService;

        // The constructor for the controller 
        public ExamController(IUnitOfWork unitOfWork, IExamService examService)
        {
            _unitOfWork = unitOfWork;
            _examService = examService;
        }

        // End Point For Get All Elements In This Domin Class
        [HttpGet("GetAllExams")]
        public async Task<IActionResult> GetAllExams()
        {
            var result = await _examService.GetAllExams();
            return Ok(result);
        }

        // End Point For Get Element By Id In This Domin Class
        [HttpGet("GetExamById/{id}")]
        public async Task<IActionResult> GetExamById(int id)
        {
            var result = await _examService.GetExamById(id);
            return Ok(result);
        }


        // End Point To Add Element In This Domin Class
        [HttpPost("AddExam")]
        public async Task<IActionResult> AddExam([FromBody] ExamDto dto)
        {
            var result = await _examService.AddExam(dto);
            return Ok(result);
        }

        // End Point To Update Element In This Domin Class
        [HttpPut("UpdateExam")]
        public async Task<IActionResult> UpdateExam([FromBody] ExamDto dto)
        {
            var result = await _examService.UpdateExam(dto);
            return Ok(result);
        }

        // End Point To Delete Element In This Domin Class
        [HttpDelete("DeleteExam")]
        public async Task<IActionResult> DeleteExam(int id)
        {
            var result = await _examService.DeleteExam(id);
            return Ok(result);
        }
    }
}
