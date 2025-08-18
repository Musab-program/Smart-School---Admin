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
    public class ExamService : IExamService
    {
        // An object is declared to handle database operations as a single unit.
        private readonly IUnitOfWork _unitOfWork;

        // The constructor for the controller 
        public ExamService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // End Point To Add Element In This Domin Class
        public async Task<Response<ExamDto>> AddExam(ExamDto dto)
        {
            var exam = await _unitOfWork.Exams.FindAsync(r => r.Id == dto.Id);
            if (exam != null)
            {
                return new Response<ExamDto>
                {
                    Message = "الاختبار موجود مسبقا",
                    Code = 400,
                };
            }

            var subjectDetail = await _unitOfWork.SubjectDetails.FindAsync(a => a.Id == dto.Id);
            if (subjectDetail == null)
                return new Response<ExamDto>
                {
                    Message = "تفاصيل المادة غير موجودة",
                    Code = 400,
                };
            
            var group = await _unitOfWork.Groups.FindAsync(a => a.Id == dto.Id);
            if (group == null)
                return new Response<ExamDto>
                {
                    Message = "الطالب غير موجود",
                    Code = 400,
                };

            var examType = await _unitOfWork.ExamTypes.FindAsync(a => a.Id == dto.Id);
            if (examType == null)
                return new Response<ExamDto>
                {
                    Message = "نوع الاختبار غير موجود",
                    Code = 400,
                };


            Exam addExam = new Exam
            {
                ExamDate = dto.ExamDate,
                LimitTime = dto.LimitTime,
                GroupId = dto.GroupId,
                SubjectDetailId = dto.SubjectDetailId,
                ExamTypeId = dto.ExamTypeId,         
            };

            var examNew = await _unitOfWork.Exams.AddAsync(addExam);
            _unitOfWork.Save();
            return new Response<ExamDto>
            {
                Message = "تمت الإضافة بنجاح",
                Code = 200,
                Data = examNew,
            };
        }

        // End Point To Delete Element In This Domin Class
        public async Task<Response<ExamDto>> DeleteExam(int id)
        {
            var exam = await _unitOfWork.Exams.GetByIdAsync(id);
            if (exam == null)
                return new Response<ExamDto>
                {
                    Message = "الاختبار الذي تريد حذفه غير موجود",
                    Code = 400,
                };
            _unitOfWork.Exams.Delete(exam);
            _unitOfWork.Save();
            return new Response<ExamDto>
            {
                Message = "تم الحذف بنجاح",
                Code = 200,
            };
        }

        // End Point For Get All Elements In This Domin Class
        public async Task<Response<ExamDto>> GetAllExams()
        {
            var exam = await _unitOfWork.Exams.GetAllAsync();
            // Select What Data Will Shows In Respons
            var dataDisplay = exam.Select(s => new ExamDto
            {
                Id = s.Id,
                SubjectDetailId = s.SubjectDetailId,
                ExamDate = s.ExamDate,
                LimitTime = s.LimitTime,
                GroupId = s.GroupId,
                ExamTypeId = s.ExamTypeId,
            });
            return new Response<ExamDto>
            {
                Data = dataDisplay,
                Code = 200,
                Message = "تم استدعاء جميع محتوى المواد",
            };
        }

        // End Point For Get Element By Id In This Domin Class
        public async Task<Response<ExamDto>> GetExamById(int id)
        {
            var exam = await _unitOfWork.Exams.GetByIdAsync(id);
            if (exam == null)
                return new Response<ExamDto>
                {
                    Message = "الاختبار الذي تبحث عنه غير موجود",
                    Code = 400,
                };
            return new Response<ExamDto>
            {
                Data = new ExamDto
                {
                    Id = exam.Id,
                    SubjectDetailId = exam.SubjectDetailId,
                    ExamDate = exam.ExamDate,
                    LimitTime = exam.LimitTime,
                    GroupId = exam.GroupId,
                    ExamTypeId = exam.ExamTypeId,
                },
                Code = 200,
                Message = "تم استدعاء الاختبار برقم التعريف",
            };
        }

        // End Point To Update Element In This Domin Class
        public async Task<Response<ExamDto>> UpdateExam(ExamDto dto)
        {
            var exam = await _unitOfWork.Exams.FindAsync(r => r.Id == dto.Id);
            if (exam == null)
                return new Response<ExamDto>
                {
                    Message = "الاختبار الذي تبحث عنه غير موجود",
                    Code = 400,
                };

            exam.Id = dto.Id;
            exam.SubjectDetailId = dto.SubjectDetailId;
            exam.ExamDate = dto.ExamDate;
            exam.LimitTime = dto.LimitTime;
            exam.GroupId = dto.GroupId;
            exam.ExamTypeId = dto.ExamTypeId;

            var examNew = _unitOfWork.Exams.Update(exam);
            _unitOfWork.Save();
            return new Response<ExamDto>
            {
                Message = "تم التعديل بنجاح",
                Code = 200,
            };
        }
    }

    public interface IExamService
    {
        Task<Response<ExamDto>> GetAllExams();
        Task<Response<ExamDto>> GetExamById(int id);
        Task<Response<ExamDto>> AddExam(ExamDto dto);
        Task<Response<ExamDto>> UpdateExam(ExamDto dto);
        Task<Response<ExamDto>> DeleteExam(int id);
    }
}
