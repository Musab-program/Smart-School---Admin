using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartSchool.Core;
using SmartSchool.Main.Dtos;
using SmartSchool.Main.InterFaces;

namespace SmartSchool.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuardianController : ControllerBase
    {
        // An object is declared to handle database operations as a single unit.
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGuardianService _guardianService;

        // The constructor for the controller 
        public GuardianController(IUnitOfWork unitOfWork, IGuardianService guardianService)
        {
            _unitOfWork = unitOfWork;
            _guardianService = guardianService;
        }

        // End Point For Get All Elements In This Domin Class
        [HttpGet("GetAllGuardians")]
        public async Task<IActionResult> GetAllGuardians()
        {
            var result = await _guardianService.GetAllGuardians();
            return Ok(result);
        }

        // End Point For Get Element By Id In This Domin Class
        [HttpGet("GetGuardianById/{id}")]
        public async Task<IActionResult> GetGuardianById(int id)
        {
            var result = await _guardianService.GetGuardianById(id);
            return Ok(result);
        }


        // End Point To Add Element In This Domin Class
        [HttpPost("AddGuardian")]
        public async Task<IActionResult> AddGuardian([FromBody] GuardianDto dto)
        {
            var result = await _guardianService.AddGuardian(dto);
            return Ok(result);
        }

        // End Point To Update Element In This Domin Class
        [HttpPut("UpdateGuardian")]
        public async Task<IActionResult> UpdateGuardian([FromBody] GuardianDto dto)
        {
            var result = await _guardianService.UpdateGuardian(dto);
            return Ok(result);
        }

        // End Point To Delete Element In This Domin Class
        [HttpDelete("DeleteGuardian")]
        public async Task<IActionResult> DeleteGuardian(int id)
        {
            var result = await _guardianService.DeleteGuardian(id);
            return Ok(result);
        }
    }
}
