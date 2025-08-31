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
    /////////
    /// <summary>
    /// 
    /// </summary>
    public class ExamTypeService : IExamTypeService
    {
    // An object is declared to handle database operations as a single unit.
    private readonly IUnitOfWork _unitOfWork;

    // The constructor for the controller 
    public ExamTypeService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    // End Point To Add Element In This Domin Class
    public async Task<Response<ExamTypeDto>> AddExamType(ExamTypeDto dto)
    {
        var examType = await _unitOfWork.ExamTypes.FindAsync(r => r.Name == dto.Name && r.Year == dto.Year);
        if (examType != null)
        {
            return new Response<ExamTypeDto>
            {
                Message = "نوع الاختبار موجود مسبقا",
                Code = 400,
            };
        }

        ExamType addExamType = new ExamType
        { 
            Name = dto.Name,
            Year = dto.Year,
        };

        var examTypeNew = await _unitOfWork.ExamTypes.AddAsync(addExamType);
        _unitOfWork.Save();
        return new Response<ExamTypeDto>
        {
            Message = "تمت الإضافة بنجاح",
            Code = 200,
            Data = examTypeNew,
        };
    }

    // End Point To Delete Element In This Domin Class
    public async Task<Response<ExamTypeDto>> DeleteExamType(int id)
    {
        var examType = await _unitOfWork.ExamTypes.GetByIdAsync(id);
        if (examType == null)
            return new Response<ExamTypeDto>
            {
                Message = "الاختبار الذي تريد حذفه غير موجود",
                Code = 400,
            };
        try
        { 
            _unitOfWork.ExamTypes.Delete(examType);
            _unitOfWork.Save();
            return new Response<ExamTypeDto>
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
    public async Task<Response<ExamTypeDto>> GetAllExamTypes()
    {
        var ExamType = await _unitOfWork.ExamTypes.GetAllAsync();
        // Select What Data Will Shows In Respons
        var dataDisplay = ExamType.Select(s => new ExamTypeDto
        {
            Id = s.Id,
            Name = s.Name,
            Year = s.Year,
        });
        return new Response<ExamTypeDto>
        {
            Data = dataDisplay,
            Code = 200,
            Message = "تم استدعاء جميع أنواع الاختبارات",
        };
    }

    // End Point For Get Element By Id In This Domin Class
    public async Task<Response<ExamTypeDto>> GetExamTypeById(int id)
    {
        var ExamType = await _unitOfWork.ExamTypes.GetByIdAsync(id);
        if (ExamType == null)
            return new Response<ExamTypeDto>
            {
                Message = "نوع الاختبار الذي تبحث عنه غير موجود",
                Code = 400,
            };
        return new Response<ExamTypeDto>
        {
            Data = new ExamTypeDto
            {
                Id = ExamType.Id,
                Name = ExamType.Name,
                Year = ExamType.Year,
            },
            Code = 200,
            Message = "تم استدعاء نوع الاختبار برقم التعريف",
        };
    }

    // End Point To Update Element In This Domin Class
    public async Task<Response<ExamTypeDto>> UpdateExamType(ExamTypeDto dto)
    {
        var examType = await _unitOfWork.ExamTypes.FindAsync(r => r.Id == dto.Id);
        if (examType == null)
            return new Response<ExamTypeDto>
            {
                Message = "نوع الاختبار الذي تبحث عنه غير موجود",
                Code = 400,
            };

        examType.Name = dto.Name;
        examType.Year = dto.Year;

        var ExamTypeNew = _unitOfWork.ExamTypes.Update(examType);
        _unitOfWork.Save();
        return new Response<ExamTypeDto>
        {
            Message = "تم التعديل بنجاح",
            Code = 200,
        };
    }

        public async Task<Response<int>> CountExamType()
        {
            var countExamType = await _unitOfWork.ExamTypes.CountAsync();

            return new Response<int>
            {
                Message = "Success",
                Code = 200,
                Data = countExamType
            };
        }
    }

    public interface IExamTypeService
    {
        Task<Response<ExamTypeDto>> GetAllExamTypes();
        Task<Response<ExamTypeDto>> GetExamTypeById(int id);
        Task<Response<ExamTypeDto>> AddExamType(ExamTypeDto dto);
        Task<Response<ExamTypeDto>> UpdateExamType(ExamTypeDto dto);
        Task<Response<ExamTypeDto>> DeleteExamType(int id);
        Task<Response<int>> CountExamType();
    }
}
