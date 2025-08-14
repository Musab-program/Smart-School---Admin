using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartSchool.Core;
using SmartSchool.Main.Dtos;
using SmartSchool.Main.InterFaces;

namespace SmartSchool.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
        public class ContentsController : ControllerBase
        {
            // An object is declared to handle database operations as a single unit.
            private readonly IUnitOfWork _unitOfWork;
            private readonly IContentService _contentService;

            // The constructor for the controller 
            public ContentsController(IUnitOfWork unitOfWork, IContentService contentService)
            {
                _unitOfWork = unitOfWork;
                _contentService = contentService;
            }

            // End Point For Get All Elements In This Domin Class
            [HttpGet("GetAllRoles")]
            public async Task<IActionResult> GetAllRoles()
            {
                var result = await _contentService.GetAllContents();
                return Ok(result);
            }

            // End Point For Get Element By Id In This Domin Class
            [HttpGet("GetByIdRole/{id}")]
            public async Task<IActionResult> GetByIdRole(int id)
            {
                var result = await _contentService.GetContentById(id);
                return Ok(result);
            }


            // End Point To Add Element In This Domin Class
            [HttpPost("AddRole")]
            public async Task<IActionResult> AddRole([FromBody] ContentDto dto)
            {
                var result = await _contentService.AddContent(dto);
                return Ok(result);
            }

            // End Point To Update Element In This Domin Class
            [HttpPut("UpdateRole")]
            public async Task<IActionResult> UpdateRole([FromBody] ContentDto dto)
            {
                var result = await _contentService.UpdateContent(dto);
                return Ok(result);
            }

            // End Point To Delete Element In This Domin Class
            [HttpDelete("DeleteRole")]
            public async Task<IActionResult> DeleteRole(int id)
            {
                var result = await _contentService.DeleteContent(id);
                return Ok(result);
            }
        }
}
