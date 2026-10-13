using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartSchool.Core;
using SmartSchool.Core.Models;
using SmartSchool.Main.InterFaces;
using SmartSchool.Main.Dtos;

namespace SmartSchool.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TimeTablesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITimeTableService _timeTableService;

        // The constructor for the controller
        public TimeTablesController(IUnitOfWork unitOfWork, ITimeTableService timeTableService)
        {
            _unitOfWork = unitOfWork;
            _timeTableService = timeTableService;
        }

        // End Point For add Element In This Domin Class
        [HttpPost("AddTimeTable")]
        public async Task<IActionResult> AddTimeTable([FromBody] TimeTableDto dto)
        {
            var result = await _timeTableService.AddTimeTable(dto);
            return result.Code != 200 ? BadRequest(result) : Ok(result);
        }


        // End Point For Get All Elements In This Domin Class
        [HttpGet("GetAllTimeTable")]
        public async Task<IActionResult> GetAllTimeTable()
        {
            var result = await _timeTableService.GetAllTimeTables();
            return result.Code != 200 ? BadRequest(result) : Ok(result);
        }


        // End Point For Get  Element by id In This Domin Class
        [HttpGet("GetTimeTableById")]
        public async Task<IActionResult> GetTimeTableById(int id)
        {
            var result = await _timeTableService.GetByIdTimeTable(id);
            return result.Code != 200 ? BadRequest(result) : Ok(result);
        }


        // End Point For update Elements In This Domin Class
        [HttpPut("UpdateTimeTables")]
        public async Task<IActionResult> UpdateTimeTables([FromBody] TimeTableDto dto)
        {
            var result = await _timeTableService.UpdateTimeTable(dto);
            return result.Code != 200 ? BadRequest(result) : Ok(result);
        }


        // End Point For delete Element In This Domin Class
        [HttpDelete("DeleteTimeTable")]
        public async Task<IActionResult> DeleteTimeTable(int id)
        {
            var result = await _timeTableService.DeleteTimeTable(id);
            return result.Code != 200 ? BadRequest(result) : Ok(result);
        }
    }
}
