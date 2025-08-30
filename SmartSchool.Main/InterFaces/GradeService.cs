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
    public class GradeService : IGradeService
    {
        // An object is declared to handle database operations as a single unit.
        private readonly IUnitOfWork _unitOfWork;

        // The constructor for the controller 
        public GradeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // End Point To Add Element In This Domin Class
        public async Task<Response<GradeDto>> AddGrade(GradeDto dto)
        {
            var grade = await _unitOfWork.Grades.FindAsync(r => r.Name == dto.Name && r.Stage == dto.Stage);
            if (grade != null)
            {
                return new Response<GradeDto>
                {
                    Message = "الفصل الدراسي موجود مسبقا",
                    Code = 400,
                };
            }

            Grade addGrade = new Grade
            {
                Name = dto.Name,
                Capacity = dto.Capacity,
                Stage = dto.Stage,
            };

            var GradeNew = await _unitOfWork.Grades.AddAsync(addGrade);
            _unitOfWork.Save();
            return new Response<GradeDto>
            {
                Message = "تمت الإضافة بنجاح",
                Code = 200,
                Data = GradeNew,
            };
        }

        // End Point To Delete Element In This Domin Class
        public async Task<Response<GradeDto>> DeleteGrade(int id)
        {
            var grade = await _unitOfWork.Grades.GetByIdAsync(id);
            if (grade == null)
                return new Response<GradeDto>
                {
                    Message = "الفصل الدراسي الذي تريد حذفه غير موجود",
                    Code = 400,
                };
            try
            {
                _unitOfWork.Grades.Delete(grade);
                _unitOfWork.Save();
                return new Response<GradeDto>
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
        public async Task<Response<GradeDto>> GetAllGrades()
        {
            var grade = await _unitOfWork.Grades.GetAllAsync();
            // Select What Data Will Shows In Respons
            var dataDisplay = grade.Select(s => new GradeDto
            {
                Id = s.Id,
                Name = s.Name,
                Stage = s.Stage,
                Capacity = s.Capacity,
            });
            return new Response<GradeDto>
            {
                Data = dataDisplay,
                Code = 200,
                Message = "تم استدعاء جميع الفصول الدراسية",
            };
        }

        // End Point For Get Element By Id In This Domin Class
        public async Task<Response<GradeDto>> GetGradeById(int id)
        {
            var grade = await _unitOfWork.Grades.GetByIdAsync(id);
            if (grade == null)
                return new Response<GradeDto>
                {
                    Message = "الفصل الدراسي الذي تبحث عنه غير موجود",
                    Code = 400,
                };
            return new Response<GradeDto>
            {
                Data = new GradeDto
                {
                    Id = grade.Id,
                    Name = grade.Name,
                    Stage = grade.Stage,
                    Capacity = grade.Capacity,
                },
                Code = 200,
                Message = "تم استدعاء الفصل الدراسي برقم التعريف",
            };
        }

        // End Point To Update Element In This Domin Class
        public async Task<Response<GradeDto>> UpdateGrade(GradeDto dto)
        {
            var grade = await _unitOfWork.Grades.FindAsync(r => r.Id == dto.Id);
            if (grade == null)
                return new Response<GradeDto>
                {
                    Message = "الفصل الدراسي الذي تبحث عنه غير موجود",
                    Code = 400,
                };

            grade.Name = dto.Name;
            grade.Stage = dto.Stage;
            grade.Capacity = dto.Capacity;

            var GradeNew = _unitOfWork.Grades.Update(grade);
            _unitOfWork.Save();
            return new Response<GradeDto>
            {
                Message = "تم التعديل بنجاح",
                Code = 200,
            };
        }
    }
    public interface IGradeService
    {
        Task<Response<GradeDto>> GetAllGrades();
        Task<Response<GradeDto>> GetGradeById(int id);
        Task<Response<GradeDto>> AddGrade(GradeDto dto);
        Task<Response<GradeDto>> UpdateGrade(GradeDto dto);
        Task<Response<GradeDto>> DeleteGrade(int id);
    }
}
