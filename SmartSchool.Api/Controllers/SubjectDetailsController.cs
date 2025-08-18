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
    public class SubjectDetailsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISubjectDetailService _subjectDetailService;

        // The constructor for the controller
        public SubjectDetailsController(IUnitOfWork unitOfWork, ISubjectDetailService subjectDetailService)
        {
            _unitOfWork = unitOfWork;
            _subjectDetailService = subjectDetailService;
        }

        // End Point For add Element In This Domin Class
        [HttpPost("AddSubjectDetail")]
        public async Task<IActionResult> AddSubjectDetail([FromBody] SubjectDetailDto dto)
        {
            var result = await _subjectDetailService.AddSubjectDetail(dto);
            return Ok(result);
        }


        // End Point For Get All Elements In This Domin Class
        [HttpGet("GetAllSubjectDetail")]
        public async Task<IActionResult> GetAllSubjectDetail()
        {
            var result = await _subjectDetailService.GetAllSubjectDetails();
            return Ok(result);
        }


        // End Point For Get  Element by id In This Domin Class
        [HttpGet("GetSubjectDetailById")]
        public async Task<IActionResult> GetSubjectDetailById(int id)
        {
            var result = await _subjectDetailService.GetByIdSubjectDetail(id);
            return Ok(result);
        }


        // End Point For update Elements In This Domin Class
        [HttpPut("UpdateSubjectDetails")]
        public async Task<IActionResult> UpdateSubjectDetails([FromBody] SubjectDetailDto dto)
        {
            var result = await _subjectDetailService.UpdateSubjectDetail(dto);
            return Ok(result);
        }


        // End Point For delete Element In This Domin Class
        [HttpDelete("DeleteSubjectDetail")]
        public async Task<IActionResult> DeleteSubjectDetail(int id)
        {
            var result = await _subjectDetailService.DeleteSubjectDetail(id);
            return Ok(result);
        }
    }
}
