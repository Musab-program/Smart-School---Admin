using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartSchool.Core;
using SmartSchool.Main.Dtos;
using SmartSchool.Main.InterFaces;

namespace SmartSchool.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GradeController : ControllerBase
    {
        // An object is declared to handle database operations as a single unit.
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGradeService _gradeService;

        // The constructor for the controller 
        public GradeController(IUnitOfWork unitOfWork, IGradeService gradeService)
        {
            _unitOfWork = unitOfWork;
            _gradeService = gradeService;
        }

        // End Point For Get All Elements In This Domin Class
        [HttpGet("GetAllGrades")]
        public async Task<IActionResult> GetAllGrades()
        {
            var result = await _gradeService.GetAllGrades();
            return Ok(result);
        }

        // End Point For Get Element By Id In This Domin Class
        [HttpGet("GetGradeById/{id}")]
        public async Task<IActionResult> GetGradeById(int id)
        {
            var result = await _gradeService.GetGradeById(id);
            return Ok(result);
        }


        // End Point To Add Element In This Domin Class
        [HttpPost("AddGrade")]
        public async Task<IActionResult> AddGrade([FromBody] GradeDto dto)
        {
            var result = await _gradeService.AddGrade(dto);
            return Ok(result);
        }

        // End Point To Update Element In This Domin Class
        [HttpPut("UpdateGrade")]
        public async Task<IActionResult> UpdateGrade([FromBody] GradeDto dto)
        {
            var result = await _gradeService.UpdateGrade(dto);
            return Ok(result);
        }

        // End Point To Delete Element In This Domin Class
        [HttpDelete("DeleteGrade")]
        public async Task<IActionResult> DeleteGrade(int id)
        {
            var result = await _gradeService.DeleteGrade(id);
            return Ok(result);
        }
    }
}
