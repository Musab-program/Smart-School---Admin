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
    public class TeachingSubjectService : ITeachingSubjectService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TeachingSubjectService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<TeachingSubjectDto>> AddTeachingSubject(TeachingSubjectDto dto)
        {

            var subjectDetail = await _unitOfWork.SubjectDetails.FindAsync(b => b.Id == dto.SubjectDetailId);
            if (subjectDetail == null)
                return new Response<TeachingSubjectDto>
                {
                    Message = "المادة المخصصة غير موجود ",
                    Code = 400
                };

            var teacher = await _unitOfWork.Teachers.FindAsync(b => b.Id == dto.TeacherId);
            if (teacher == null)
                return new Response<TeachingSubjectDto>
                {
                    Message = "المعلم غير موجود ",
                    Code = 400
                };

            var teachingSubject = await _unitOfWork.TeachingSubjects.FindAsync(b => b.Id == dto.Id);
            if (teachingSubject != null)
                return new Response<TeachingSubjectDto>
                {
                    Message = "التخصيص هذا موجود مسبقا",
                    Code = 400
                };


            TeachingSubject addTeachingSubject = new TeachingSubject
            {
                TeacherId = dto.TeacherId,
                SubjectDetailId = dto.SubjectDetailId,
                AcademicYear = dto.AcademicYear,
                Semster = dto.Semster,



            };
            var teachingSubjectNew = await _unitOfWork.TeachingSubjects.AddAsync(addTeachingSubject);
            _unitOfWork.Save();
            return new Response<TeachingSubjectDto>
            {
                Message = "تمت الاصافة",
                Data = teachingSubjectNew,
                Code = 200
            };
        }


        public async Task<Response<TeachingSubjectDto>> DeleteTeachingSubject(int id)
        {
            var teachingSubject = await _unitOfWork.TeachingSubjects.FindAsync(b => b.Id == id);
            if (teachingSubject == null)
                return new Response<TeachingSubjectDto>
                {
                    Message = " المعلم غير موجود ",
                    Code = 400,
                };

            try
            {
                _unitOfWork.TeachingSubjects.Delete(teachingSubject);
                _unitOfWork.Save();
                return new Response<TeachingSubjectDto>
                {
                    Code = 200,
                    Message = "تم الحذف",
                    Data = teachingSubject
                };
            }
            catch
            {
                throw new Exception("هذا السجل مرتبط بجدول آخر");
            }


        }


        public async Task<Response<TeachingSubjectDto>> GetAllTeachingSubjects()
        {
            var teachingSubjects = await _unitOfWork.TeachingSubjects.GetAllAsync();


            var dataDisplay = teachingSubjects.Select(s => new TeachingSubjectDto
            {
                Id = s.Id,
                TeacherId = s.TeacherId,
                SubjectDetailId = s.SubjectDetailId,
                AcademicYear = s.AcademicYear,
                Semster = s.Semster,
            });

            return new Response<TeachingSubjectDto>
            {
                Message = "تم جلب المعلمين بنجاح ",
                Data = dataDisplay,
                Code = 200
            };

        }


        public async Task<Response<TeachingSubjectDto>> GetByIdTeachingSubject(int id)
        {
            var teachingSubject = await _unitOfWork.TeachingSubjects.GetByIdAsync(id);

            if (teachingSubject == null)
            {
                return new Response<TeachingSubjectDto>
                {
                    Message = "المعلم غير موجود ",
                    Code = 400,
                };
            }

            return new Response<TeachingSubjectDto>
            {
                Message = "تم العثور على المعلم",
                Code = 200,
                Data = new TeachingSubjectDto
                {
                    Id = teachingSubject.Id,
                    TeacherId = teachingSubject.TeacherId,
                    SubjectDetailId = teachingSubject.SubjectDetailId,
                    AcademicYear = teachingSubject.AcademicYear,
                    Semster = teachingSubject.Semster,
                }
            };

        }


        public async Task<Response<TeachingSubjectDto>> UpdateTeachingSubject(TeachingSubjectDto dto)
        {
            var teachingSubject = await _unitOfWork.TeachingSubjects.FindAsync(b => b.Id == dto.Id);
            if (teachingSubject == null)
                return new Response<TeachingSubjectDto>
                {
                    Message = "المعلم غير موجود ",
                    Code = 400,

                };

            var subjectDetail = await _unitOfWork.SubjectDetails.FindAsync(b => b.Id == dto.SubjectDetailId);
            if (subjectDetail == null)
                return new Response<TeachingSubjectDto>
                {
                    Message = "المادة المخصصة غير موجود ",
                    Code = 400
                };

            var teacher = await _unitOfWork.Teachers.FindAsync(b => b.Id == dto.TeacherId);
            if (teacher == null)
                return new Response<TeachingSubjectDto>
                {
                    Message = "المعلم غير موجود ",
                    Code = 400
                };

            teachingSubject.TeacherId = dto.TeacherId;
            teachingSubject.SubjectDetailId = dto.SubjectDetailId;
            teachingSubject.AcademicYear = dto.AcademicYear;
            teachingSubject.Semster = dto.Semster;

            var TeachingSubjectNew = _unitOfWork.TeachingSubjects.Update(teachingSubject);
            _unitOfWork.Save();
            return new Response<TeachingSubjectDto>
            {
                Code = 200,
                Message = "تم التعديل",
                Data = new TeachingSubjectDto
                {
                    Id = teachingSubject.Id,
                    TeacherId = teachingSubject.TeacherId,
                    SubjectDetailId = teachingSubject.SubjectDetailId,
                    AcademicYear = teachingSubject.AcademicYear,
                    Semster = teachingSubject.Semster,


                }
            };
        }
    }


    public interface ITeachingSubjectService
    {
        Task<Response<TeachingSubjectDto>> AddTeachingSubject(TeachingSubjectDto dto);
        Task<Response<TeachingSubjectDto>> GetAllTeachingSubjects();
        Task<Response<TeachingSubjectDto>> GetByIdTeachingSubject(int id);
        Task<Response<TeachingSubjectDto>> UpdateTeachingSubject(TeachingSubjectDto dto);
        Task<Response<TeachingSubjectDto>> DeleteTeachingSubject(int id);
    }
}
