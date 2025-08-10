using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartSchool.Core;
using SmartSchool.Core.Interfaces;
using SmartSchool.Core.Models;

namespace SmartSchool.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public UsersController(IUnitOfWork unitOfWork)
        {
             _unitOfWork = unitOfWork;
        }

        //[HttpGet]
        //public IActionResult GetById()
        //{
        //    return Ok(_usersRepository.GetById(1));
        //}
    }
}
