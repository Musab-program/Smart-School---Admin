using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartSchool.Main.Dtos;
using SmartSchool.Core;
using SmartSchool.Core.Models;
using SmartSchool.Core.Shared;

namespace SmartSchool.Main.InterFaces
{
    public class SubjectDetailService : ISubjectDetailService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SubjectDetailService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<SubjectDetailDto>> AddSubjectDetail(SubjectDetailDto dto)
        {


            var grade = await _unitOfWork.Grades.FindAsync(b => b.Id == dto.GradeId);
            if (grade == null)
                return new Response<SubjectDetailDto>
                {
                    Message = "الفصل غير موجود ",
                    Code = 400
                };


            var subject = await _unitOfWork.Subjects.FindAsync(b => b.Id == dto.SubjectId);
            if (subject == null)
                return new Response<SubjectDetailDto>
                {
                    Message = "المادة غير موجود ",
                    Code = 400
                };

            var subjectDetail = await _unitOfWork.SubjectDetails.FindAsync(b => b.Id == dto.Id);
            if (subjectDetail != null)
                return new Response<SubjectDetailDto>
                {
                    Message = "هذه المادة المخصصة موجود مسبقا",
                    Code = 400
                };


            SubjectDetail addSubjectDetail = new SubjectDetail
            {
                IsActive = dto.IsActive,
                GradeId = dto.GradeId,
                SubjectId = dto.SubjectId,


            };
            var subjectDetailNew = await _unitOfWork.SubjectDetails.AddAsync(addSubjectDetail);
            _unitOfWork.Save();
            return new Response<SubjectDetailDto>
            {
                Message = "تمت الاصافة",
                Data = subjectDetailNew,
                Code = 200
            };
        }


        public async Task<Response<SubjectDetailDto>> DeleteSubjectDetail(int id)
        {
            var subjectDetail = await _unitOfWork.SubjectDetails.FindAsync(b => b.Id == id);
            if (subjectDetail == null)
                return new Response<SubjectDetailDto>
                {
                    Message = " هذه المادة المخصصة غير موجود ",
                    Code = 400,
                };

            try
            {
                _unitOfWork.SubjectDetails.Delete(subjectDetail);
                _unitOfWork.Save();
                return new Response<SubjectDetailDto>
                {
                    Code = 200,
                    Message = "تم الحذف",
                    Data = subjectDetail
                };
            }
            catch
            {
                throw new Exception("هذا السجل مرتبط بجدول آخر");
            }


        }


        public async Task<Response<SubjectDetailDto>> GetAllSubjectDetails()
        {
            var subjectDetails = await _unitOfWork.SubjectDetails.GetAllAsync();


            var dataDisplay = subjectDetails.Select(s => new SubjectDetailDto
            {
                Id = s.Id,
                IsActive = s.IsActive,
                GradeId = s.GradeId,
                SubjectId = s.SubjectId,
            });

            return new Response<SubjectDetailDto>
            {
                Message = "تم جلب  المواد المخصصات بنجاح ",
                Data = dataDisplay,
                Code = 200
            };

        }


        public async Task<Response<SubjectDetailDto>> GetByIdSubjectDetail(int id)
        {
            var subjectDetail = await _unitOfWork.SubjectDetails.GetByIdAsync(id);

            if (subjectDetail == null)
            {
                return new Response<SubjectDetailDto>
                {
                    Message = "هذه المادة المخصصة غير موجود ",
                    Code = 400,
                };
            }

            return new Response<SubjectDetailDto>
            {
                Message = "تم العثور على  المادة المخصصة",
                Code = 200,
                Data = new SubjectDetailDto
                {
                    Id = subjectDetail.Id,
                    IsActive = subjectDetail.IsActive,
                    GradeId = subjectDetail.GradeId,
                    SubjectId = subjectDetail.SubjectId,
                }
            };

        }


        public async Task<Response<SubjectDetailDto>> UpdateSubjectDetail(SubjectDetailDto dto)
        {
            var subjectDetail = await _unitOfWork.SubjectDetails.FindAsync(b => b.Id == dto.Id);
            if (subjectDetail == null)
                return new Response<SubjectDetailDto>
                {
                    Message = "هذه المادة المخصصة غير موجود ",
                    Code = 400,

                };

            var grade = await _unitOfWork.Grades.FindAsync(b => b.Id == dto.GradeId);
            if (grade == null)
                return new Response<SubjectDetailDto>
                {
                    Message = "الفصل غير موجود ",
                    Code = 400
                };


            var subject = await _unitOfWork.Subjects.FindAsync(b => b.Id == dto.SubjectId);
            if (subject == null)
                return new Response<SubjectDetailDto>
                {
                    Message = "المادة غير موجود ",
                    Code = 400
                };

            subjectDetail.IsActive = dto.IsActive;
            subjectDetail.GradeId = dto.GradeId;
            subjectDetail.SubjectId = dto.SubjectId;

            var subjectDetailNew = _unitOfWork.SubjectDetails.Update(subjectDetail);
            _unitOfWork.Save();
            return new Response<SubjectDetailDto>
            {
                Code = 200,
                Message = "تم التعديل",
                Data = new SubjectDetailDto
                {
                    Id = subjectDetail.Id,
                    IsActive = subjectDetail.IsActive,
                    GradeId = subjectDetail.GradeId,
                    SubjectId = subjectDetail.SubjectId,
                }
            };
        }
    }


    public interface ISubjectDetailService
    {
        Task<Response<SubjectDetailDto>> AddSubjectDetail(SubjectDetailDto dto);
        Task<Response<SubjectDetailDto>> GetAllSubjectDetails();
        Task<Response<SubjectDetailDto>> GetByIdSubjectDetail(int id);
        Task<Response<SubjectDetailDto>> UpdateSubjectDetail(SubjectDetailDto dto);
        Task<Response<SubjectDetailDto>> DeleteSubjectDetail(int id);
    }
}
