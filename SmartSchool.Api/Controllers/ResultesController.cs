using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartSchool.Core;
using SmartSchool.Main.Dtos;
using SmartSchool.Main.InterFaces;

namespace SmartSchool.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResulteController : ControllerBase
    {
        // An object is declared to handle database operations as a single unit.
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResulteService _resulteService;

        // The constructor for the controller 
        public ResulteController(IUnitOfWork unitOfWork, IResulteService resulteService)
        {
            _unitOfWork = unitOfWork;
            _resulteService = resulteService;
        }

        // End Point For Get All Elements In This Domin Class
        [HttpGet("GetAllResultes")]
        public async Task<IActionResult> GetAllResultes()
        {
            var result = await _resulteService.GetAllResultes();
            return Ok(result);
        }

        // End Point For Get Element By Id In This Domin Class
        [HttpGet("GetResulteById/{id}")]
        public async Task<IActionResult> GetResulteById(int id)
        {
            var result = await _resulteService.GetResulteById(id);
            return Ok(result);
        }


        // End Point To Add Element In This Domin Class
        [HttpPost("AddResulte")]
        public async Task<IActionResult> AddResulte([FromBody] ResulteDto dto)
        {
            var result = await _resulteService.AddResulte(dto);
            return Ok(result);
        }

        // End Point To Update Element In This Domin Class
        [HttpPut("UpdateResulte")]
        public async Task<IActionResult> UpdateResulte([FromBody] ResulteDto dto)
        {
            var result = await _resulteService.UpdateResulte(dto);
            return Ok(result);
        }

        // End Point To Delete Element In This Domin Class
        [HttpDelete("DeleteResulte")]
        public async Task<IActionResult> DeleteResulte(int id)
        {
            var result = await _resulteService.DeleteResulte(id);
            return Ok(result);
        }
    }
}
