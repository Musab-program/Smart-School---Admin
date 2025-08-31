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

        // End Point To Add Element In This Domin Class
        //public async Task<Response<AssignmentDto>> AddAssignment(AssignmentDto dto)
        //{
        //    var assignment = await _unitOfWork.Assignments.FindAsync(r => r.Id == dto.Id);
        //    if (assignment != null)
        //    {
        //        return new Response<AssignmentDto>
        //        {
        //            Message = "الواجب موجود مسبقا",
        //            Code = 400,
        //        };
        //    }

        //    /// **************************
        //    /// هل لازم نربط الواجب بالشعبة؟
        //    /// اذا ما ربطناه لما نظيف واجب لمن ينشر؟
        //    Assignment addAssignment = new Assignment
        //    {
        //        Title = dto.Title,

        //    };
        //    var roleNew = await _unitOfWork.Assignments.AddAsync(addRole);
        //    _unitOfWork.Save();
        //    return new Response<RoleDto>
        //    {
        //        Message = "تمت الإضافة بنجاح",
        //        Code = 200,
        //        Data = roleNew,
        //    };
        //}

        //// End Point To Delete Element In This Domin Class
        //public async Task<Response<RoleDto>> DeleteAssignment(int id)
        //{
        //    var role = await _unitOfWork.Assignments.GetByIdAsync(id);
        //    if (role == null)
        //        return new Response<RoleDto>
        //        {
        //            Message = "النوع غير موجود",
        //            Code = 400,
        //        };
        //    _unitOfWork.Assignments.Delete(role);
        //    _unitOfWork.Save();
        //    return new Response<RoleDto>
        //    {
        //        Message = "تم المسح بنجاح",
        //        Code = 200,
        //    };
        //}

        // End Point For Get All Elements In This Domin Class
        public async Task<Response<AssignmentDto>> GetAllAssignments()
        {
            var assignment = await _unitOfWork.Assignments.FindAllAsync(s => s.Id > 1, ["SubjectDetail"]);
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
        Task<Response<int>> CountAssignment();
    }
}
