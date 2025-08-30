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
    public class SubmittedAssignmentService : ISubmittedAssignmentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SubmittedAssignmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<SubmittedAssignmentDto>> AddSubmittedAssignment(SubmittedAssignmentDto dto)
        {

            var student = await _unitOfWork.Students.FindAsync(b => b.Id == dto.StudentId);
            if (student == null)
                return new Response<SubmittedAssignmentDto>
                {
                    Message = "الطالب غير موجود ",
                    Code = 400
                };

            var assignment = await _unitOfWork.Assignments.FindAsync(b => b.Id == dto.AssignmentId);
            if (assignment == null)
                return new Response<SubmittedAssignmentDto>
                {
                    Message = "الواجب غير موجود ",
                    Code = 400
                };

            var submittedAssignment = await _unitOfWork.SubmittedAssignments.FindAsync(b => b.StudentId == dto.StudentId && b.AssignmentId == dto.AssignmentId);
            if (submittedAssignment != null)
                return new Response<SubmittedAssignmentDto>
                {
                    Message = "تم تقديم الواجب مسبقا",
                    Code = 400
                };


            SubmittedAssignment addSubmittedAssignment = new SubmittedAssignment
            {
                StudentId = dto.StudentId,
                AssignmentId = dto.AssignmentId,
                FilePath = dto.FilePath,
                Mark = dto.Mark,
                Notes = dto.Notes,



            };
            var submittedAssignmentNew = await _unitOfWork.SubmittedAssignments.AddAsync(addSubmittedAssignment);
            _unitOfWork.Save();
            return new Response<SubmittedAssignmentDto>
            {
                Message = "تمت الاصافة",
                Data = submittedAssignmentNew,
                Code = 200
            };
        }


        public async Task<Response<SubmittedAssignmentDto>> DeleteSubmittedAssignment(int id)
        {
            var submittedAssignment = await _unitOfWork.SubmittedAssignments.FindAsync(b => b.Id == id);
            if (submittedAssignment == null)
                return new Response<SubmittedAssignmentDto>
                {
                    Message = " الواجب  غير موجود ",
                    Code = 400,
                };

            try
            {
                _unitOfWork.SubmittedAssignments.Delete(submittedAssignment);
                _unitOfWork.Save();
                return new Response<SubmittedAssignmentDto>
                {
                    Code = 200,
                    Message = "تم الحذف",
                    Data = submittedAssignment
                };
            }
            catch
            {
                throw new Exception("هذا السجل مرتبط بجدول آخر");
            }


        }


        public async Task<Response<SubmittedAssignmentDto>> GetAllSubmittedAssignments()
        {
            var submittedAssignments = await _unitOfWork.SubmittedAssignments.GetAllAsync();


            var dataDisplay = submittedAssignments.Select(s => new SubmittedAssignmentDto
            {
                Id = s.Id,
                StudentId = s.StudentId,
                AssignmentId = s.AssignmentId,
                FilePath = s.FilePath,
                Mark = s.Mark,
                Notes = s.Notes,
            });

            return new Response<SubmittedAssignmentDto>
            {
                Message = "تم جلب الواجبات بنجاح ",
                Data = dataDisplay,
                Code = 200
            };

        }


        public async Task<Response<SubmittedAssignmentDto>> GetByIdSubmittedAssignment(int id)
        {
            var submittedAssignment = await _unitOfWork.SubmittedAssignments.GetByIdAsync(id);

            if (submittedAssignment == null)
            {
                return new Response<SubmittedAssignmentDto>
                {
                    Message = " الواجب غير موجود ",
                    Code = 400,
                };
            }

            return new Response<SubmittedAssignmentDto>
            {
                Message = "تم العثور على الواجب",
                Code = 200,
                Data = new SubmittedAssignmentDto
                {
                    Id = submittedAssignment.Id,
                    StudentId = submittedAssignment.StudentId,
                    AssignmentId = submittedAssignment.AssignmentId,
                    FilePath = submittedAssignment.FilePath,
                    Mark = submittedAssignment.Mark,
                    Notes = submittedAssignment.Notes,
                }
            };

        }


        public async Task<Response<SubmittedAssignmentDto>> UpdateSubmittedAssignment(SubmittedAssignmentDto dto)
        {
            var submittedAssignment = await _unitOfWork.SubmittedAssignments.FindAsync(b => b.Id == dto.Id);
            if (submittedAssignment == null)
                return new Response<SubmittedAssignmentDto>
                {
                    Message = "الواجب غير موجود ",
                    Code = 400,

                };

            var student = await _unitOfWork.Students.FindAsync(b => b.Id == dto.StudentId);
            if (student == null)
                return new Response<SubmittedAssignmentDto>
                {
                    Message = "الطالب غير موجود ",
                    Code = 400
                };

            var assignment = await _unitOfWork.Assignments.FindAsync(b => b.Id == dto.AssignmentId);
            if (assignment == null)
                return new Response<SubmittedAssignmentDto>
                {
                    Message = "الواجب غير موجود ",
                    Code = 400
                };

            submittedAssignment.StudentId = dto.StudentId;
            submittedAssignment.AssignmentId = dto.AssignmentId;
            submittedAssignment.FilePath = dto.FilePath;
            submittedAssignment.Mark = dto.Mark;
            submittedAssignment.Notes = dto.Notes;


            var submittedAssignmentNew = _unitOfWork.SubmittedAssignments.Update(submittedAssignment);
            _unitOfWork.Save();
            return new Response<SubmittedAssignmentDto>
            {
                Code = 200,
                Message = "تم التعديل",
                Data = new SubmittedAssignmentDto
                {
                    Id = submittedAssignment.Id,
                    StudentId = submittedAssignment.StudentId,
                    AssignmentId = submittedAssignment.AssignmentId,
                    FilePath = submittedAssignment.FilePath,
                    Mark = submittedAssignment.Mark,
                    Notes = submittedAssignment.Notes,
                }
            };
        }
    }


    public interface ISubmittedAssignmentService
    {
        Task<Response<SubmittedAssignmentDto>> AddSubmittedAssignment(SubmittedAssignmentDto dto);
        Task<Response<SubmittedAssignmentDto>> GetAllSubmittedAssignments();
        Task<Response<SubmittedAssignmentDto>> GetByIdSubmittedAssignment(int id);
        Task<Response<SubmittedAssignmentDto>> UpdateSubmittedAssignment(SubmittedAssignmentDto dto);
        Task<Response<SubmittedAssignmentDto>> DeleteSubmittedAssignment(int id);
    }
}
