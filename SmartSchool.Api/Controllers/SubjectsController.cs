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

    public class SubjectsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISubjectService _subjectService;

        // The constructor for the controller
        public SubjectsController(IUnitOfWork unitOfWork, ISubjectService subjectService)
        {
            _unitOfWork = unitOfWork;
            _subjectService = subjectService;
        }

        // End Point For add Element In This Domin Class
        [HttpPost("AddSubject")]
        public async Task<IActionResult> AddSubject([FromBody] SubjectDto dto)
        {
            var result = await _subjectService.AddSubject(dto);
            return Ok(result);
        }


        // End Point For Get All Elements In This Domin Class
        [HttpGet("GetAllSubject")]
        public async Task<IActionResult> GetAllSubject()
        {
            var result = await _subjectService.GetAllSubjects();
            return Ok(result);
        }


        // End Point For Get  Element by id In This Domin Class
        [HttpGet("GetSubjectById")]
        public async Task<IActionResult> GetSubjectById(int id)
        {
            var result = await _subjectService.GetByIdSubject(id);
            return Ok(result);
        }


        // End Point For update Elements In This Domin Class
        [HttpPut("UpdateSubjects")]
        public async Task<IActionResult> UpdateSubjects([FromBody] SubjectDto dto)
        {
            var result = await _subjectService.UpdateSubject(dto);
            return Ok(result);
        }


        // End Point For delete Element In This Domin Class
        [HttpDelete("DeleteSubject")]
        public async Task<IActionResult> DeleteSubject(int id)
        {
            var result = await _subjectService.DeleteSubject(id);
            return Ok(result);
        }
    }
}
