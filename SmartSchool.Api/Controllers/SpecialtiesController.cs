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
    public class SpecialtiesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISpecialtyService _specialtyService;

        // The constructor for the controller
        public SpecialtiesController(IUnitOfWork unitOfWork , ISpecialtyService specialtyService)
        {
            _unitOfWork = unitOfWork;
            _specialtyService= specialtyService;
        }

        // End Point For add Element In This Domin Class
        [HttpPost("AddSpecialty")]
        public async Task<IActionResult> AddSpecialty([FromBody] SpecialtyDto dto)
        {
            var result = await _specialtyService.AddSpecialty(dto);
            return Ok(result);
        }


        // End Point For Get All Elements In This Domin Class
        [HttpGet("GetAllSpecialty")]
        public async Task<IActionResult> GetAllSpecialties()
        {
            var result = await _specialtyService.GetAllSpecialties();
            return Ok(result);
        }


        // End Point For Get  Element by id In This Domin Class
        [HttpGet("GetSpecialtyById")]
        public async Task<IActionResult> GetSpecialtyById(int id)
        {
            var result = await _specialtyService.GetByIdSpecialty(id);
            return Ok(result);
        }


        // End Point For update Elements In This Domin Class
        [HttpPut("UpdateSpecialtys")]
        public async Task<IActionResult> UpdateSpecialtys([FromBody] SpecialtyDto dto)
        {
            var result = await _specialtyService.UpdateSpecialty(dto);
            return Ok(result);
        }


          // End Point For delete Element In This Domin Class
        [HttpDelete("DeleteSpecialty")]
        public async Task<IActionResult> DeleteSpecialty(int id)
        {
            var result = await _specialtyService.DeleteSpecialty(id);
            return Ok(result);
        }
    }
}
