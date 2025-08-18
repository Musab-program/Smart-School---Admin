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
    public class GroupService : IGroupService
    {
        // An object is declared to handle database operations as a single unit.
        private readonly IUnitOfWork _unitOfWork;

        // The constructor for the controller 
        public GroupService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // End Point To Add Element In This Domin Class
        public async Task<Response<GroupDto>> AddGroup(GroupDto dto)
        {
            var group = await _unitOfWork.Groups.FindAsync(r => r.Id == dto.Id);
            if (group != null)
            {
                return new Response<GroupDto>
                {
                    Message = "الشعبة الدراسية موجودة مسبقا",
                    Code = 400,
                };
            }

            var grade = await _unitOfWork.Grades.FindAsync(a => a.Id == dto.Id);
            if (grade == null)
                return new Response<GroupDto>
                {
                    Message = "الفصل الدراسي غير موجود",
                    Code = 400,
                };

            Group addGroup = new Group
            {
                Id = dto.Id,
                Name = dto.Name,
                AcademicYear = dto.AcademicYear,
                GradeId = dto.GradeId,
            };

            var groupNew = await _unitOfWork.Groups.AddAsync(addGroup);
            _unitOfWork.Save();
            return new Response<GroupDto>
            {
                Message = "تمت الإضافة بنجاح",
                Code = 200,
                Data = groupNew,
            };
        }

        // End Point To Delete Element In This Domin Class
        public async Task<Response<GroupDto>> DeleteGroup(int id)
        {
            var group = await _unitOfWork.Groups.GetByIdAsync(id);
            if (group == null)
                return new Response<GroupDto>
                {
                    Message = "الشعبة الدراسية الذي تريد حذفها غير موجوده",
                    Code = 400,
                };
            _unitOfWork.Groups.Delete(group);
            _unitOfWork.Save();
            return new Response<GroupDto>
            {
                Message = "تم الحذف بنجاح",
                Code = 200,
            };
        }

        // End Point For Get All Elements In This Domin Class
        public async Task<Response<GroupDto>> GetAllGroups()
        {
            var Group = await _unitOfWork.Groups.GetAllAsync();
            // Select What Data Will Shows In Respons
            var dataDisplay = Group.Select(s => new GroupDto
            {
                Id = s.Id,
                Name = s.Name,
                GradeId=s.GradeId,
                AcademicYear = s.AcademicYear,
            });
            return new Response<GroupDto>
            {
                Data = dataDisplay,
                Code = 200,
                Message = "تم استدعاء جميع الشعب الدراسية",
            };
        }

        // End Point For Get Element By Id In This Domin Class
        public async Task<Response<GroupDto>> GetGroupById(int id)
        {
            var group = await _unitOfWork.Groups.GetByIdAsync(id);
            if (group == null)
                return new Response<GroupDto>
                {
                    Message = "الشعبة الدراسية الذي تبحث عنها غير موجودة",
                    Code = 400,
                };
            return new Response<GroupDto>
            {
                Data = new GroupDto
                {
                    Id = group.Id,
                    Name = group.Name,
                    AcademicYear=group.AcademicYear,
                    GradeId = group.GradeId,
                },
                Code = 200,
                Message = "تم استدعاء الشعبة الدراسية برقم التعريف",
            };
        }

        // End Point To Update Element In This Domin Class
        public async Task<Response<GroupDto>> UpdateGroup(GroupDto dto)
        {
            var group = await _unitOfWork.Groups.FindAsync(r => r.Id == dto.Id);
            if (group == null)
                return new Response<GroupDto>
                {
                    Message = "الشعبة الدراسية الذي تبحث عنها غير موجودة",
                    Code = 400,
                };

            group.Id = dto.Id;
            group.Name = dto.Name;
            group.AcademicYear = dto.AcademicYear;
            group.GradeId = dto.GradeId;

            var groupNew = _unitOfWork.Groups.Update(group);
            _unitOfWork.Save();
            return new Response<GroupDto>
            {
                Message = "تم التعديل بنجاح",
                Code = 200,
            };
        }
    }

    public interface IGroupService
    {
        Task<Response<GroupDto>> GetAllGroups();
        Task<Response<GroupDto>> GetGroupById(int id);
        Task<Response<GroupDto>> AddGroup(GroupDto dto);
        Task<Response<GroupDto>> UpdateGroup(GroupDto dto);
        Task<Response<GroupDto>> DeleteGroup(int id);
    }
}
