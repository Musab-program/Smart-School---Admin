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
    public class TeacherHolidayService : ITeacherHolidayService
    {
        // An object is declared to handle database operations as a single unit.
        private readonly IUnitOfWork _unitOfWork;

        // The constructor for the controller 
        public TeacherHolidayService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // End Point To Add Element In This Domin Class
        public async Task<Response<TeacherHolidayDto>> AddTeacherHoliday(TeacherHolidayDto dto)
        {
            var teacherHoliday = await _unitOfWork.TeacherHolidays.FindAsync(r => r.Id == dto.Id);
            if (teacherHoliday != null)
            {
                return new Response<TeacherHolidayDto>
                {
                    Message = "العطلة التي تريد اضافتها موجودة مسبقا",
                    Code = 400,
                };
            }

            var teacher = await _unitOfWork.Teachers.FindAsync(a => a.Id == dto.Id);
            if (teacher == null)
                return new Response<TeacherHolidayDto>
                {
                    Message = "المعلم غير موجود",
                    Code = 400,
                };


            TeacherHoliday addTeacherHoliday = new TeacherHoliday
            {
                TeacherId= dto.TeacherId,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Reason = dto.Reason,
                IsAgreeded = dto.IsAgreeded,
            };

            var teacherHolidayNew = await _unitOfWork.TeacherHolidays.AddAsync(addTeacherHoliday);
            _unitOfWork.Save();
            return new Response<TeacherHolidayDto>
            {
                Message = "تمت الإضافة بنجاح",
                Code = 200,
                Data = teacherHolidayNew,
            };
        }

        // End Point To Delete Element In This Domin Class
        public async Task<Response<TeacherHolidayDto>> DeleteTeacherHoliday(int id)
        {
            var teacherHoliday = await _unitOfWork.TeacherHolidays.GetByIdAsync(id);
            if (teacherHoliday == null)
                return new Response<TeacherHolidayDto>
                {
                    Message = "العطلة التي تريد حذفها غير موجودة",
                    Code = 400,
                };
            _unitOfWork.TeacherHolidays.Delete(teacherHoliday);
            _unitOfWork.Save();
            return new Response<TeacherHolidayDto>
            {
                Message = "تم الحذف بنجاح",
                Code = 200,
            };
        }

        // End Point For Get All Elements In This Domin Class
        public async Task<Response<TeacherHolidayDto>> GetAllTeacherHolidays()
        {
            var teacherHoliday = await _unitOfWork.TeacherHolidays.GetAllAsync();
            // Select What Data Will Shows In Respons
            var dataDisplay = teacherHoliday.Select(s => new TeacherHolidayDto
            {
                Id = s.Id,
                TeacherId = s.TeacherId,
                StartDate = s.StartDate,
                EndDate = s.EndDate,
                Reason = s.Reason,
                IsAgreeded = s.IsAgreeded,
            });
            return new Response<TeacherHolidayDto>
            {
                Data = dataDisplay,
                Code = 200,
                Message = "تم استدعاء جميع العطل",
            };
        }

        // End Point For Get Element By Id In This Domin Class
        public async Task<Response<TeacherHolidayDto>> GetTeacherHolidayById(int id)
        {
            var teacherHoliday = await _unitOfWork.TeacherHolidays.GetByIdAsync(id);
            if (teacherHoliday == null)
                return new Response<TeacherHolidayDto>
                {
                    Message = "العطلة التي تبحث عنها غير موجودة",
                    Code = 400,
                };
            return new Response<TeacherHolidayDto>
            {
                Data = new TeacherHolidayDto
                {
                    Id = teacherHoliday.Id,
                    TeacherId = teacherHoliday.TeacherId,
                    StartDate = teacherHoliday.StartDate,
                    EndDate = teacherHoliday.EndDate,
                    Reason = teacherHoliday.Reason,
                    IsAgreeded = teacherHoliday.IsAgreeded,

                },
                Code = 200,
                Message = "تم استدعاء العطلة برقم التعريف",
            };
        }

        // End Point To Update Element In This Domin Class
        public async Task<Response<TeacherHolidayDto>> UpdateTeacherHoliday(TeacherHolidayDto dto)
        {
            var teacherHoliday = await _unitOfWork.TeacherHolidays.FindAsync(r => r.Id == dto.Id);
            if (teacherHoliday == null)
                return new Response<TeacherHolidayDto>
                {
                    Message = "العطلة الذي تبحث عنها غير موجودة",
                    Code = 400,
                };

            teacherHoliday.Id = dto.Id;
            teacherHoliday.TeacherId = dto.TeacherId;
            teacherHoliday.StartDate = dto.StartDate;
            teacherHoliday.EndDate = dto.EndDate;
            teacherHoliday.Reason = dto.Reason;
            teacherHoliday.IsAgreeded = dto.IsAgreeded;

            var TeacherHolidayNew = _unitOfWork.TeacherHolidays.Update(teacherHoliday);
            _unitOfWork.Save();
            return new Response<TeacherHolidayDto>
            {
                Message = "تم التعديل بنجاح",
                Code = 200,
            };
        }
    }

    public interface ITeacherHolidayService
    {
        Task<Response<TeacherHolidayDto>> GetAllTeacherHolidays();
        Task<Response<TeacherHolidayDto>> GetTeacherHolidayById(int id);
        Task<Response<TeacherHolidayDto>> AddTeacherHoliday(TeacherHolidayDto dto);
        Task<Response<TeacherHolidayDto>> UpdateTeacherHoliday(TeacherHolidayDto dto);
        Task<Response<TeacherHolidayDto>> DeleteTeacherHoliday(int id);
    }
}
