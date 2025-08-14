using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartSchool.Api.Dtos;
using SmartSchool.Core;
using SmartSchool.Core.Models;
using SmartSchool.Main.Dto;
using SmartSchool.Main.InterFaces;

namespace SmartSchool.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // An object is declared to handle database operations as a single unit.
    public class RelationTypesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRelationTypeService _relationTypeService;

        // The constructor for the controller
        public RelationTypesController(IUnitOfWork unitOfWork, IRelationTypeService relationTypeService)
        {
            _unitOfWork = unitOfWork;
            _relationTypeService = relationTypeService;
        }

        // End Point For add Element In This Domin Class
        [HttpPost("AddRelationType")]
        public async  Task<IActionResult> AddRelationType([FromBody]RelationTypeDto dto)
        {
            var result = await _relationTypeService.AddRelationType(dto);            
            return Ok(result);
        }


        // End Point For Get All Elements In This Domin Class
        [HttpGet("GetAllRelation")]
        public async Task<IActionResult> GetAllRelation()
        {
            var result = await _relationTypeService.GetAllRelationType();
            return Ok(result);
        }

        // End Point For Get  Element by id In This Domin Class
        [HttpGet("GetRelationById")]
        public async Task<IActionResult> GetRelationById(int id)
        {
            var result= await _relationTypeService.GetByIdRelationType(id);
            return Ok(result);
        }

        // End Point For update Elements In This Domin Class
        [HttpPut("UpdateRelationTypes")]
        public async Task<IActionResult> UpdateRelationTypes([FromBody]RelationTypeDto dto)
        {
            var result = await _relationTypeService.UpdateRelationType(dto);
            return Ok(result);
        }


        // End Point For delete Element In This Domin Class
        [HttpDelete("DeleteRelationTypes")]
        public async Task<IActionResult> DeleteRelationTypes(int id)
        {
            var result = await _relationTypeService.DeleteRelationType(id);
            return Ok(result);
        }

    }
}
