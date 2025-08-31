using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartSchool.Core;
using SmartSchool.Core.Models;
using SmartSchool.Core.Shared;
using SmartSchool.Main.Dtos;

namespace SmartSchool.Main.InterFaces
{
    public class RoleService : IRoleService
    {
        // An object is declared to handle database operations as a single unit.
        private readonly IUnitOfWork _unitOfWork;

        // The constructor for the controller 
        public RoleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // End Point To Add Element In This Domin Class
        public async Task<Response<RoleDto>> AddRole(RoleDto dto)
        {
            var role = await _unitOfWork.Roles.FindAsync(r => r.Name == dto.Name);
            if (role != null)
            {
                return new Response<RoleDto>
                {
                    Message = "النوع موجود مسبقا",
                    Code = 400,
                };
            }
            Role addRole = new Role
            {
                Name = dto.Name,
            };
            var roleNew = await _unitOfWork.Roles.AddAsync(addRole);
            _unitOfWork.Save();
            return new Response<RoleDto>
            {
                Message = "تمت الإضافة بنجاح",
                Code = 200,
                Data = roleNew,
            };
        }

        // End Point To Delete Element In This Domin Class
        public async Task<Response<RoleDto>> DeleteRole(int id)
        {
            var role = await _unitOfWork.Roles.GetByIdAsync(id);
            if (role == null)
                return new Response<RoleDto>
                {
                    Message = "النوع غير موجود",
                    Code = 400,
                };
            try
            {
                _unitOfWork.Roles.Delete(role);
                _unitOfWork.Save();
                return new Response<RoleDto>
                {
                    Message = "تم المسح بنجاح",
                    Code = 200,
                };
            }
            catch
            {
                throw new Exception("هذا السجل مرتبط بجدول آخر");
            }

        }

        // End Point For Get All Elements In This Domin Class
        public async Task<Response<RoleDto>> GetAllRoles()
        {
            var role = await _unitOfWork.Roles.GetAllAsync();
            // Select What Data Will Shows In Respons
            var dataDisplay = role.Select(s => new RoleDto
            {
                Id = s.Id,
                Name = s.Name,
            });
            return new Response<RoleDto>
            {
                Data = dataDisplay,
                Code = 200,
                Message = "تم استدعاء جميع الأنواع",
            };
        }

        // End Point For Get Element By Id In This Domin Class
        public async Task<Response<RoleDto>> GetByIdRole(int id)
        {
            var role = await _unitOfWork.Roles.GetByIdAsync(id);
            if (role == null)
                return new Response<RoleDto>
                {
                    Message = "النوع غير موجود",
                    Code = 400,
                };
            return new Response<RoleDto>
            {
                Data = new RoleDto { Id = role.Id, Name = role.Name },
                Code = 200,
                Message = "تم استدعاء النوع برقم التعريف",
            };
        }

        // End Point To Update Element In This Domin Class
        public async Task<Response<RoleDto>> UpdateRole(RoleDto dto)
        {
            var role = await _unitOfWork.Roles.FindAsync(r => r.Id == dto.Id);
            if (role == null)
                return new Response<RoleDto>
                {
                    Message = "النوع غير موجود مسبقا",
                    Code = 400,
                };

            role.Name = dto.Name;

            var roleNew = _unitOfWork.Roles.Update(role);
            _unitOfWork.Save();
            return new Response<RoleDto>
            {
                Message = "تم التعديل بنجاح",
                Code = 200,

            };
        }

        public async Task<Response<int>> CountRole()
        {
            var countRole = await _unitOfWork.Roles.CountAsync(null);

            return new Response<int>
            {
                Message = "Success",
                Code = 200,
                Data = countRole
            };
        }
    }

    public interface IRoleService
    {
        Task<Response<RoleDto>> GetAllRoles();
        Task<Response<RoleDto>> GetByIdRole(int id);
        Task<Response<RoleDto>> AddRole(RoleDto dto);
        Task<Response<RoleDto>> UpdateRole(RoleDto dto);
        Task<Response<RoleDto>> DeleteRole(int id);
        Task<Response<int>> CountRole();
    }
}
