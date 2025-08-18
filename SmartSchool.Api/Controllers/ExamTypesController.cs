using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartSchool.Core;
using SmartSchool.Main.Dtos;
using SmartSchool.Main.InterFaces;

namespace SmartSchool.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamTypeController : ControllerBase
    {
        // An object is declared to handle database operations as a single unit.
        private readonly IUnitOfWork _unitOfWork;
        private readonly IExamTypeService _examTypeService;

        // The constructor for the controller 
        public ExamTypeController(IUnitOfWork unitOfWork, IExamTypeService examTypeService)
        {
            _unitOfWork = unitOfWork;
            _examTypeService = examTypeService;
        }

        // End Point For Get All Elements In This Domin Class
        [HttpGet("GetAllExamTypes")]
        public async Task<IActionResult> GetAllExamTypes()
        {
            var result = await _examTypeService.GetAllExamTypes();
            return Ok(result);
        }

        // End Point For Get Element By Id In This Domin Class
        [HttpGet("GetExamTypeById/{id}")]
        public async Task<IActionResult> GetExamTypeById(int id)
        {
            var result = await _examTypeService.GetExamTypeById(id);
            return Ok(result);
        }


        // End Point To Add Element In This Domin Class
        [HttpPost("AddExamType")]
        public async Task<IActionResult> AddExamType([FromBody] ExamTypeDto dto)
        {
            var result = await _examTypeService.AddExamType(dto);
            return Ok(result);
        }

        // End Point To Update Element In This Domin Class
        [HttpPut("UpdateExamType")]
        public async Task<IActionResult> UpdateExamType([FromBody] ExamTypeDto dto)
        {
            var result = await _examTypeService.UpdateExamType(dto);
            return Ok(result);
        }

        // End Point To Delete Element In This Domin Class
        [HttpDelete("DeleteExamType")]
        public async Task<IActionResult> DeleteExamType(int id)
        {
            var result = await _examTypeService.DeleteExamType(id);
            return Ok(result);
        }
    }
}
