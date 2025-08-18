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
    public class SubjectService : ISubjectService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SubjectService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<SubjectDto>> AddSubject(SubjectDto dto)
        {
            var subject = await _unitOfWork.Subjects.FindAsync(b => b.Id == dto.Id);
            if (subject != null)
                return new Response<SubjectDto>
                {
                    Message = "المعلم موجود مسبقا",
                    Code = 400
                };


            Subject addSubject = new Subject
            {
                Name = dto.Name,

            };
            var subjectNew = await _unitOfWork.Subjects.AddAsync(addSubject);
            _unitOfWork.Save();
            return new Response<SubjectDto>
            {
                Message = "تمت الاصافة",
                Data = subjectNew,
                Code = 200
            };
        }


        public async Task<Response<SubjectDto>> DeleteSubject(int id)
        {
            var subject = await _unitOfWork.Subjects.FindAsync(b => b.Id == id);
            if (subject == null)
                return new Response<SubjectDto>
                {
                    Message = " المعلم غير موجود ",
                    Code = 400,
                };

            _unitOfWork.Subjects.Delete(subject);
            _unitOfWork.Save();
            return new Response<SubjectDto>
            {
                Code = 200,
                Message = "تم الحذف",
                Data = subject
            };

        }


        public async Task<Response<SubjectDto>> GetAllSubjects()
        {
            var subjects = await _unitOfWork.Subjects.GetAllAsync();


            var dataDisplay = subjects.Select(s => new SubjectDto
            {
                Id = s.Id,  
                Name = s.Name,
            });

            return new Response<SubjectDto>
            {
                Message = "تم جلب المعلمين بنجاح ",
                Data = dataDisplay,
                Code = 200
            };

        }


        public async Task<Response<SubjectDto>> GetByIdSubject(int id)
        {
            var subject = await _unitOfWork.Subjects.GetByIdAsync(id);

            if (subject == null)
            {
                return new Response<SubjectDto>
                {
                    Message = "المعلم غير موجود ",
                    Code = 400,
                };
            }

            return new Response<SubjectDto>
            {
                Message = "تم العثور على المعلم",
                Code = 200,
                Data = new SubjectDto
                {
                    Id = subject.Id,
                    Name = subject.Name,
                }
            };

        }


        public async Task<Response<SubjectDto>> UpdateSubject(SubjectDto dto)
        {
            var subject = await _unitOfWork.Subjects.FindAsync(b => b.Id == dto.Id);
            if (subject == null)
                return new Response<SubjectDto>
                {
                    Message = "المعلم غير موجود ",
                    Code = 400,

                };


            subject.Name = dto.Name;
            var SubjectNew = _unitOfWork.Subjects.Update(subject);
            _unitOfWork.Save();
            return new Response<SubjectDto>
            {
                Code = 200,
                Message = "تم التعديل",
                Data = new SubjectDto
                {
                    Id = subject.Id,
                    Name = subject.Name,
                }
            };
        }
    }


    public interface ISubjectService
    {
        Task<Response<SubjectDto>> AddSubject(SubjectDto dto);
        Task<Response<SubjectDto>> GetAllSubjects();
        Task<Response<SubjectDto>> GetByIdSubject(int id);
        Task<Response<SubjectDto>> UpdateSubject(SubjectDto dto);
        Task<Response<SubjectDto>> DeleteSubject(int id);
    }
}
