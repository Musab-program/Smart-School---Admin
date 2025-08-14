using SmartSchool.Core;
using SmartSchool.Core.Models;
using SmartSchool.Core.Shared;
using SmartSchool.Main.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSchool.Main.InterFaces
{
    public class RelationTypeService : IRelationTypeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RelationTypeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<RelationTypeDto>> AddRelationType(RelationTypeDto dto)
        {
            var relationType = await _unitOfWork.RelationTypes.FindAsync(b => b.Name == dto.Name);
            if (relationType != null)
                return new Response<RelationTypeDto>
                {
                    Message = "نوع القريب موجود مسبقا",
                    Code = 400
                };


            RelationType addRelationType = new RelationType
            {
                Name = dto.Name,
            };
            var relationTypeNew = await _unitOfWork.RelationTypes.AddAsync(addRelationType);
            _unitOfWork.Save();
            return new Response<RelationTypeDto>
            {
                Message = "تمت الاصافة",
                Data = relationTypeNew,
                Code = 200
            };
        }

        public async Task<Response<RelationTypeDto>> GetAllRelationType()
        {
            var relationTypes = await _unitOfWork.RelationTypes.GetAllAsync();


            var dataDisplay = relationTypes.Select(s => new RelationTypeDto
            {
                Id = s.Id,
                Name = s.Name,
            });

            return new Response<RelationTypeDto>
            {
                Message = "تم جلب أنواع الأقارب بنجاح ",
                Data = dataDisplay,
                Code = 200
            };
        }


        public async Task<Response<RelationTypeDto>> GetByIdRelationType(int id)
        {
            var relationType = await _unitOfWork.RelationTypes.GetByIdAsync(id);

            if (relationType == null) {
                return new Response<RelationTypeDto>
                {
                    Message = "نوع القريب غير موجود ",
                    Code = 400,
                };
            }

            return new Response<RelationTypeDto>
            {
                Message = "تم العثور على القريب",
                Code = 200,
                Data = new RelationTypeDto { Id = relationType.Id, Name = relationType.Name }
            };
        }


        public async Task<Response<RelationTypeDto>> DeleteRelationType(int id)
        {
            var relationType = await _unitOfWork.RelationTypes.FindAsync(b => b.Id == id);
            if (relationType == null)
                return new Response<RelationTypeDto>
                {
                    Message = "نوع القريب غير موجود ",
                    Code = 400,
                };

            _unitOfWork.RelationTypes.Delete(relationType);
            _unitOfWork.Save();
            return new Response<RelationTypeDto>
            {
                Code = 200,
                Message = "تم الحذف",
                Data = relationType
            };
        }


        public async Task<Response<RelationTypeDto>> UpdateRelationType(RelationTypeDto dto)
        {
            var relationType = await _unitOfWork.RelationTypes.FindAsync(b => b.Id == dto.Id);
            if (relationType == null)
                return new Response<RelationTypeDto>
                {
                    Message = "نوع القريب غير موجود ",
                    Code = 400,

                };


            relationType.Name = dto.Name;
            var relationTypeNew = _unitOfWork.RelationTypes.Update(relationType);
            _unitOfWork.Save();
            return new Response<RelationTypeDto>
            {
                Code = 200,
                Message = "تم التعديل",
                Data = new RelationTypeDto { Id = relationType.Id, Name = relationType.Name }
            };
        }
    }


    public interface IRelationTypeService
    {
        Task<Response<RelationTypeDto>> AddRelationType(RelationTypeDto dto);
        Task<Response<RelationTypeDto>> GetAllRelationType();  
        Task<Response<RelationTypeDto>> GetByIdRelationType(int id);
        Task<Response<RelationTypeDto>> UpdateRelationType(RelationTypeDto dto);
        Task<Response<RelationTypeDto>> DeleteRelationType(int id);
    }


}
