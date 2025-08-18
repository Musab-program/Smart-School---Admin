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
            [HttpGet("GetAllContents")]
            public async Task<IActionResult> GetAllContents()
            {
                var result = await _contentService.GetAllContents();
                return Ok(result);
            }

            // End Point For Get Element By Id In This Domin Class
            [HttpGet("GetContentById/{id}")]
            public async Task<IActionResult> GetContentById(int id)
            {
                var result = await _contentService.GetContentById(id);
                return Ok(result);
            }


            // End Point To Add Element In This Domin Class
            [HttpPost("AddContent")]
            public async Task<IActionResult> AddContent([FromBody] ContentDto dto)
            {
                var result = await _contentService.AddContent(dto);
                return Ok(result);
            }

            // End Point To Update Element In This Domin Class
            [HttpPut("UpdateContent")]
            public async Task<IActionResult> UpdateContent([FromBody] ContentDto dto)
            {
                var result = await _contentService.UpdateContent(dto);
                return Ok(result);
            }

            // End Point To Delete Element In This Domin Class
            [HttpDelete("DeleteContent")]
            public async Task<IActionResult> DeleteContent(int id)
            {
                var result = await _contentService.DeleteContent(id);
                return Ok(result);
            }
        }
}
