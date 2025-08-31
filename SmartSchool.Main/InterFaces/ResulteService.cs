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
    public class ResulteService : IResulteService
    {
        // An object is declared to handle database operations as a single unit.
        private readonly IUnitOfWork _unitOfWork;

        // The constructor for the controller 
        public ResulteService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // End Point To Add Element In This Domin Class
        public async Task<Response<ResulteDto>> AddResulte(ResulteDto dto)
        {
            var resulte = await _unitOfWork.Resultes.FindAsync(r => r.StudentId == dto.StudentId 
                                                            && r.SubjectDetailId == dto.SubjectDetailId);
            if (resulte != null)
            {
                return new Response<ResulteDto>
                {
                    Message = "النتيجة موجودة مسبقا",
                    Code = 400,
                };
            }

            var subjectDetail = await _unitOfWork.SubjectDetails.FindAsync(a => a.Id == dto.Id);
            if (subjectDetail == null)
                return new Response<ResulteDto>
                {
                    Message = "تفاصيل المادة غير موجودة",
                    Code = 400,
                };

            var student = await _unitOfWork.Students.FindAsync(a => a.Id == dto.Id);
            if (student == null)
                return new Response<ResulteDto>
                {
                    Message = "الطالب غير موجود",
                    Code = 400,
                };

            Resulte addResulte = new Resulte
            {
                Rate = dto.Rate,
                Mark = dto.Mark,
                StudentId = dto.StudentId,
                SubjectDetailId = dto.SubjectDetailId,
            };

            var resulteNew = await _unitOfWork.Resultes.AddAsync(addResulte);
            _unitOfWork.Save();
            return new Response<ResulteDto>
            {
                Message = "تمت الإضافة بنجاح",
                Code = 200,
                Data = resulteNew,
            };
        }

        // End Point To Delete Element In This Domin Class
        public async Task<Response<ResulteDto>> DeleteResulte(int id)
        {
            var resulte = await _unitOfWork.Resultes.GetByIdAsync(id);
            if (resulte == null)
                return new Response<ResulteDto>
                {
                    Message = "النتيجة التي تريد حذفها غير موجودة",
                    Code = 400,
                };
            try
            {
                _unitOfWork.Resultes.Delete(resulte);
                _unitOfWork.Save();
                return new Response<ResulteDto>
                {
                    Message = "تم الحذف بنجاح",
                    Code = 200,
                };
            }
            catch
            {
                throw new Exception("السجل مرتبط بجدول آخر");
            }
        }

        // End Point For Get All Elements In This Domin Class
        public async Task<Response<ResulteDto>> GetAllResultes()
        {
            var Resulte = await _unitOfWork.Resultes.GetAllAsync();
            // Select What Data Will Shows In Respons
            var dataDisplay = Resulte.Select(s => new ResulteDto
            {
                Id = s.Id,
                SubjectDetailId = s.SubjectDetailId,
                StudentId = s.StudentId,
                Mark = s.Mark,
                Rate = s.Rate,
            });
            return new Response<ResulteDto>
            {
                Data = dataDisplay,
                Code = 200,
                Message = "تم استدعاء جميع النتائج",
            };
        }

        // End Point For Get Element By Id In This Domin Class
        public async Task<Response<ResulteDto>> GetResulteById(int id)
        {
            var resulte = await _unitOfWork.Resultes.GetByIdAsync(id);
            if (resulte == null)
                return new Response<ResulteDto>
                {
                    Message = "النتيجة التي تبحث عنها غير موجودة",
                    Code = 400,
                };
            return new Response<ResulteDto>
            {
                Data = new ResulteDto
                {
                    Id = resulte.Id,
                   Rate = resulte.Rate,
                   Mark = resulte.Mark,
                   StudentId= resulte.StudentId,
                   SubjectDetailId = resulte.SubjectDetailId,
                },
                Code = 200,
                Message = "تم استدعاء النتيجة برقم التعريف",
            };
        }

        // End Point To Update Element In This Domin Class
        public async Task<Response<ResulteDto>> UpdateResulte(ResulteDto dto)
        {
            var resulte = await _unitOfWork.Resultes.FindAsync(r => r.Id == dto.Id);
            if (resulte == null)
                return new Response<ResulteDto>
                {
                    Message = "النتيجة التي تبحث عنها غير موجودة",
                    Code = 400,
                };

            var subjectDetail = await _unitOfWork.SubjectDetails.FindAsync(a => a.Id == dto.Id);
            if (subjectDetail == null)
                return new Response<ResulteDto>
                {
                    Message = "تفاصيل المادة غير موجودة",
                    Code = 400,
                };

            var student = await _unitOfWork.Students.FindAsync(a => a.Id == dto.Id);
            if (student == null)
                return new Response<ResulteDto>
                {
                    Message = "الطالب غير موجود",
                    Code = 400,
                };

            resulte.SubjectDetailId = dto.SubjectDetailId;
            resulte.StudentId = dto.StudentId;
            resulte.Mark = dto.Mark;
            resulte.Rate = dto.Rate;

            var resulteNew = _unitOfWork.Resultes.Update(resulte);
            _unitOfWork.Save();
            return new Response<ResulteDto>
            {
                Message = "تم التعديل بنجاح",
                Code = 200,
            };
        }
        public async Task<Response<int>> CountResulte()
        {
            var countResulte = await _unitOfWork.Resultes.CountAsync(null);

            return new Response<int>
            {
                Message = "Success",
                Code = 200,
                Data = countResulte
            };
        }
    }

    public interface IResulteService
    {
        Task<Response<ResulteDto>> GetAllResultes();
        Task<Response<ResulteDto>> GetResulteById(int id);
        Task<Response<ResulteDto>> AddResulte(ResulteDto dto);
        Task<Response<ResulteDto>> UpdateResulte(ResulteDto dto);
        Task<Response<ResulteDto>> DeleteResulte(int id);
        Task<Response<int>> CountResulte();
    }
}
