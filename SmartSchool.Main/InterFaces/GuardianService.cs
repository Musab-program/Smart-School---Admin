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
    public class GuardianService : IGuardianService
    {
        // An object is declared to handle database operations as a single unit.
        private readonly IUnitOfWork _unitOfWork;

        // The constructor for the controller 
        public GuardianService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // End Point To Add Element In This Domin Class
        public async Task<Response<GuardianDto>> AddGuardian(GuardianDto dto)
        {
            var guardian = await _unitOfWork.Guardians.FindAsync(r => r.Id == dto.Id);
            if (guardian != null)
            {
                return new Response<GuardianDto>
                {
                    Message = "ولي الأمر موجود مسبقا",
                    Code = 400,
                };
            }

            var relationType = await _unitOfWork.RelationTypes.FindAsync(a => a.Id == dto.Id);
            if (relationType == null)
                return new Response<GuardianDto>
                {
                    Message = "العلاقة غير موجودة",
                    Code = 400,
                };

            var user = await _unitOfWork.Users.FindAsync(a => a.Id == dto.Id);
            if (user == null)
                return new Response<GuardianDto>
                {
                    Message = "المستخدم غير موجود",
                    Code = 400,
                };

            Guardian addGuardian = new Guardian
            {
                Id = dto.Id,
                RelationTypeId = dto.RelationTypeId,
                SecondryPhone = dto.SecondryPhone,
                UserId = dto.UserId,
            };

            var guardianNew = await _unitOfWork.Guardians.AddAsync(addGuardian);
            _unitOfWork.Save();
            return new Response<GuardianDto>
            {
                Message = "تمت الإضافة بنجاح",
                Code = 200,
                Data = guardianNew,
            };
        }

        // End Point To Delete Element In This Domin Class
        public async Task<Response<GuardianDto>> DeleteGuardian(int id)
        {
            var guardian = await _unitOfWork.Guardians.GetByIdAsync(id);
            if (guardian == null)
                return new Response<GuardianDto>
                {
                    Message = "ولي الأمر الذي تريد حذفه غير موجود",
                    Code = 400,
                };
            _unitOfWork.Guardians.Delete(guardian);
            _unitOfWork.Save();
            return new Response<GuardianDto>
            {
                Message = "تم الحذف بنجاح",
                Code = 200,
            };
        }

        // End Point For Get All Elements In This Domin Class
        public async Task<Response<GuardianDto>> GetAllGuardians()
        {
            var Guardian = await _unitOfWork.Guardians.GetAllAsync();
            // Select What Data Will Shows In Respons
            var dataDisplay = Guardian.Select(s => new GuardianDto
            {
                Id = s.Id,
                UserId = s.UserId,
                SecondryPhone = s.SecondryPhone,
                RelationTypeId = s.RelationTypeId,
            });
            return new Response<GuardianDto>
            {
                Data = dataDisplay,
                Code = 200,
                Message = "تم استدعاء جميع أولياء الأمور",
            };
        }

        // End Point For Get Element By Id In This Domin Class
        public async Task<Response<GuardianDto>> GetGuardianById(int id)
        {
            var guardian = await _unitOfWork.Guardians.GetByIdAsync(id);
            if (guardian == null)
                return new Response<GuardianDto>
                {
                    Message = "ولي الأمر الذي تبحث عنه غير موجود",
                    Code = 400,
                };
            return new Response<GuardianDto>
            {
                Data = new GuardianDto
                {
                    Id = guardian.Id,
                    RelationTypeId= guardian.RelationTypeId,
                    UserId = guardian.UserId,   
                    SecondryPhone = guardian.SecondryPhone,
                },
                Code = 200,
                Message = "تم استدعاء ولي الأمر برقم التعريف",
            };
        }

        // End Point To Update Element In This Domin Class
        public async Task<Response<GuardianDto>> UpdateGuardian(GuardianDto dto)
        {
            var guardian = await _unitOfWork.Guardians.FindAsync(r => r.Id == dto.Id);
            if (guardian == null)
                return new Response<GuardianDto>
                {
                    Message = "ولي الأمر الذي تبحث عنه غير موجود",
                    Code = 400,
                };

            guardian.Id = dto.Id;
            guardian.SecondryPhone = dto.SecondryPhone;
            guardian.UserId = dto.UserId;
            guardian.RelationTypeId = dto.RelationTypeId;

            var guardianNew = _unitOfWork.Guardians.Update(guardian);
            _unitOfWork.Save();
            return new Response<GuardianDto>
            {
                Message = "تم التعديل بنجاح",
                Code = 200,
            };
        }
    }

    public interface IGuardianService
    {
        Task<Response<GuardianDto>> GetAllGuardians();
        Task<Response<GuardianDto>> GetGuardianById(int id);
        Task<Response<GuardianDto>> AddGuardian(GuardianDto dto);
        Task<Response<GuardianDto>> UpdateGuardian(GuardianDto dto);
        Task<Response<GuardianDto>> DeleteGuardian(int id);
    }
}
