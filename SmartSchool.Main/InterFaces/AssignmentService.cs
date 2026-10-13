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
    public class AssignmentService : IAssignmentService
    {
        // An object is declared to handle database operations as a single unit.
        private readonly IUnitOfWork _unitOfWork;

        // The constructor for the controller 
        public AssignmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<AssignmentDto>> AddAssignment(AssignmentDto dto)
        {
            var subjectDetail = await _unitOfWork.SubjectDetails.FindAsync(r => r.Id == dto.SubjectDetailId);
            if (subjectDetail == null)
            {
                return new Response<AssignmentDto>
                {
                    Message = "تفاصيل المادة الدراسية غير موجودة",
                    Code = 400,
                };
            }

            Assignment addAssignment = new Assignment
            {
                Title = dto.Title,
                SubjectDetailId = dto.SubjectDetailId,
                LastDate = dto.LastDate,
                UploadDate = DateTime.UtcNow,
            };

            var assignmentNew = await _unitOfWork.Assignments.AddAsync(addAssignment);
            _unitOfWork.Save();
            return new Response<AssignmentDto>
            {
                Message = "تمت إضافة الواجب بنجاح",
                Code = 200,
                Data = new AssignmentDto
                {
                    Id = assignmentNew.Id,
                    Title = assignmentNew.Title,
                    SubjectDetailId = assignmentNew.SubjectDetailId,
                    LastDate = assignmentNew.LastDate,
                },
            };
        }

        public async Task<Response<AssignmentDto>> UpdateAssignment(AssignmentDto dto)
        {
            var assignment = await _unitOfWork.Assignments.FindAsync(r => r.Id == dto.Id);
            if (assignment == null)
            {
                return new Response<AssignmentDto>
                {
                    Message = "الواجب غير موجود",
                    Code = 400,
                };
            }

            var subjectDetail = await _unitOfWork.SubjectDetails.FindAsync(r => r.Id == dto.SubjectDetailId);
            if (subjectDetail == null)
            {
                return new Response<AssignmentDto>
                {
                    Message = "تفاصيل المادة الدراسية غير موجودة",
                    Code = 400,
                };
            }

            assignment.Title = dto.Title;
            assignment.SubjectDetailId = dto.SubjectDetailId;
            assignment.LastDate = dto.LastDate;

            _unitOfWork.Assignments.Update(assignment);
            _unitOfWork.Save();

            return new Response<AssignmentDto>
            {
                Message = "تم تعديل الواجب بنجاح",
                Code = 200,
                Data = new AssignmentDto
                {
                    Id = assignment.Id,
                    Title = assignment.Title,
                    SubjectDetailId = assignment.SubjectDetailId,
                    LastDate = assignment.LastDate,
                },
            };
        }

        public async Task<Response<AssignmentDto>> DeleteAssignment(int id)
        {
            var assignment = await _unitOfWork.Assignments.GetByIdAsync(id);
            if (assignment == null)
            {
                return new Response<AssignmentDto>
                {
                    Message = "الواجب غير موجود",
                    Code = 400,
                };
            }

            try
            {
                _unitOfWork.Assignments.Delete(assignment);
                _unitOfWork.Save();
                return new Response<AssignmentDto>
                {
                    Message = "تم حذف الواجب بنجاح",
                    Code = 200,
                };
            }
            catch
            {
                throw new Exception("السجل مرتبط ببيانات تسليمات واجبات أخرى");
            }
        }

        // End Point For Get All Elements In This Domin Class
        public async Task<Response<AssignmentDto>> GetAllAssignments()
        {
            var assignment = await _unitOfWork.Assignments.FindAllAsync(s => s.Id > 1,new[] {"SubjectDetail"});
            // Select What Data Will Shows In Respons
            var dataDisplay = assignment.Select(s => new AssignmentDto
            {
                Id = s.Id,
                Title = s.Title,
                LastDate = s.LastDate,
                SubjectDetailId = s.SubjectDetailId,
                
            });
            return new Response<AssignmentDto>
            {
                Data = dataDisplay,
                Code = 200,
                Message = "تم استدعاء جميع الأنواع",
            };
        }

        // End Point For Get Element By Id In This Domin Class
        public async Task<Response<AssignmentDto>> GetAssignmentById(int id)
        {
            var assignment = await _unitOfWork.Assignments.GetByIdAsync(id);
            if (assignment == null)
                return new Response<AssignmentDto>
                {
                    Message = "النوع غير موجود",
                    Code = 400,
                };
            return new Response<AssignmentDto>
            {   
                Data = new AssignmentDto {
                    Id = assignment.Id,
                    Title = assignment.Title,
                    LastDate = assignment.LastDate,
                    SubjectDetailId = assignment.SubjectDetailId,
                },
                Code = 200,
                Message = "تم استدعاء النوع برقم التعريف",
            };
        }

        public async Task<Response<int>> CountAssignment()
        {
            var assignmentCount = await _unitOfWork.Assignments.CountAsync();

            return new Response<int>
            {
                Message = "Success",
                Code = 200,
                Data = assignmentCount
            };
        }
    }

    public interface IAssignmentService
    {
        Task<Response<AssignmentDto>> GetAllAssignments();
        Task<Response<AssignmentDto>> GetAssignmentById(int id);
        Task<Response<AssignmentDto>> AddAssignment(AssignmentDto dto);
        Task<Response<AssignmentDto>> UpdateAssignment(AssignmentDto dto);
        Task<Response<AssignmentDto>> DeleteAssignment(int id);
        Task<Response<int>> CountAssignment();
    }
}
