using SmartSchool.Main.Dtos;
using SmartSchool.Core;
using SmartSchool.Core.Models;
using SmartSchool.Core.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSchool.Main.InterFaces
{
    public class SpecialtyService : ISpecialtyService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SpecialtyService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<SpecialtyDto>> AddSpecialty(SpecialtyDto dto)
        {
            var specialty = await _unitOfWork.Specialtys.FindAsync(b => b.Name == dto.Name);
            if (specialty != null)
                return new Response<SpecialtyDto>
                {
                    Message = "التخصص موجود مسبقا",
                    Code = 400
                };


            Specialty addspecialty = new Specialty
            {
                Name = dto.Name,
                Qualification = dto.Qualification,
            };
            var specialtyNew = await _unitOfWork.Specialtys.AddAsync(addspecialty);
            _unitOfWork.Save();
            return new Response<SpecialtyDto>
            {
                Message = "تمت الاصافة",
                Data = specialtyNew,
                Code = 200
            };
        }


        public async Task<Response<SpecialtyDto>> DeleteSpecialty(int id)
        {
            var specialty = await _unitOfWork.Specialtys.FindAsync(b => b.Id == id);
            if (specialty == null)
                return new Response<SpecialtyDto>
                {
                    Message = " التخصص غير موجود ",
                    Code = 400,
                };

            _unitOfWork.Specialtys.Delete(specialty);
            _unitOfWork.Save();
            return new Response<SpecialtyDto>
            {
                Code = 200,
                Message = "تم الحذف",
                Data = specialty
            };

        }


        public async Task<Response<SpecialtyDto>> GetAllSpecialty()
        {
            var specialties = await _unitOfWork.Specialtys.GetAllAsync();


            var dataDisplay = specialties.Select(s => new SpecialtyDto
            {
                Id = s.Id,
                Name = s.Name,
                Qualification = s.Qualification,
            });

            return new Response<SpecialtyDto>
            {
                Message = "تم جلب التخصصات بنجاح ",
                Data = dataDisplay,
                Code = 200
            };

        }


        public async Task<Response<SpecialtyDto>> GetByIdSpecialty(int id)
        {
            var specialty = await _unitOfWork.Specialtys.GetByIdAsync(id);

            if (specialty == null)
            {
                return new Response<SpecialtyDto>
                {
                    Message = "التخصص غير موجود ",
                    Code = 400,
                };
            }

            return new Response<SpecialtyDto>
            {
                Message = "تم العثور على التخصص",
                Code = 200,
                Data = new SpecialtyDto { Id = specialty.Id,
                    Name = specialty.Name ,
                    Qualification = specialty.Qualification }
            };

        }


        public async Task<Response<SpecialtyDto>> UpdateSpecialty(SpecialtyDto dto)
        {
            var specialty = await _unitOfWork.Specialtys.FindAsync(b => b.Id == dto.Id);
            if (specialty == null)
                return new Response<SpecialtyDto>
                {
                    Message = "التخصص غير موجود ",
                    Code = 400,

                };


            specialty.Name = dto.Name;
            specialty.Qualification = dto.Qualification;
            var SpecialtyNew = _unitOfWork.Specialtys.Update(specialty);
            _unitOfWork.Save();
            return new Response<SpecialtyDto>
            {
                Code = 200,
                Message = "تم التعديل",
                Data = new SpecialtyDto { Id = specialty.Id,
                    Name = specialty.Name,
                    Qualification = specialty.Qualification
                }
            };
        }
    }


    public interface ISpecialtyService
    {
        Task<Response<SpecialtyDto>> AddSpecialty(SpecialtyDto dto);
        Task<Response<SpecialtyDto>> GetAllSpecialty();
        Task<Response<SpecialtyDto>> GetByIdSpecialty(int id);
        Task<Response<SpecialtyDto>> UpdateSpecialty(SpecialtyDto dto);
        Task<Response<SpecialtyDto>> DeleteSpecialty(int id);
    }

}
