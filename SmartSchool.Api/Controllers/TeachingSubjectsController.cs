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
    public class TeachingSubjectsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITeachingSubjectService _teachingSubjectService;

        // The constructor for the controller
        public TeachingSubjectsController(IUnitOfWork unitOfWork, ITeachingSubjectService teachingSubjectService)
        {
            _unitOfWork = unitOfWork;
            _teachingSubjectService = teachingSubjectService;
        }

        // End Point For add Element In This Domin Class
        [HttpPost("AddTeachingSubject")]
        public async Task<IActionResult> AddTeachingSubject([FromBody] TeachingSubjectDto dto)
        {
            var result = await _teachingSubjectService.AddTeachingSubject(dto);
            return Ok(result);
        }


        // End Point For Get All Elements In This Domin Class
        [HttpGet("GetAllTeachingSubject")]
        public async Task<IActionResult> GetAllTeachingSubject()
        {
            var result = await _teachingSubjectService.GetAllTeachingSubjects();
            return Ok(result);
        }


        // End Point For Get  Element by id In This Domin Class
        [HttpGet("GetTeachingSubjectById")]
        public async Task<IActionResult> GetTeachingSubjectById(int id)
        {
            var result = await _teachingSubjectService.GetByIdTeachingSubject(id);
            return Ok(result);
        }


        // End Point For update Elements In This Domin Class
        [HttpPut("UpdateTeachingSubjects")]
        public async Task<IActionResult> UpdateTeachingSubjects([FromBody] TeachingSubjectDto dto)
        {
            var result = await _teachingSubjectService.UpdateTeachingSubject(dto);
            return Ok(result);
        }


        // End Point For delete Element In This Domin Class
        [HttpDelete("DeleteTeachingSubject")]
        public async Task<IActionResult> DeleteTeachingSubject(int id)
        {
            var result = await _teachingSubjectService.DeleteTeachingSubject(id);
            return Ok(result);
        }
    }
}
