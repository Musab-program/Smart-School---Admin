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
    public class TimeTableService : ITimeTableService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TimeTableService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<TimeTableDto>> AddTimeTable(TimeTableDto dto)
        {

            var group = await _unitOfWork.Groups.FindAsync(b => b.Id == dto.GroupId);
            if (group == null)
                return new Response<TimeTableDto>
                {
                    Message = "المستخدم غير موجود ",
                    Code = 400
                };

            var subjectDetail = await _unitOfWork.SubjectDetails.FindAsync(b => b.Id == dto.SubjectDetailId);
            if (subjectDetail == null)
                return new Response<TimeTableDto>
                {
                    Message = "المستخدم غير موجود ",
                    Code = 400
                };

            var teacher = await _unitOfWork.Teachers.FindAsync(b => b.Id == dto.TeacherId);
            if (teacher == null)
                return new Response<TimeTableDto>
                {
                    Message = "المستخدم غير موجود ",
                    Code = 400
                };

            var timeTable = await _unitOfWork.TimeTables.FindAsync(b => b.Id == dto.Id);
            if (timeTable != null)
                return new Response<TimeTableDto>
                {
                    Message = "المعلم موجود ",
                    Code = 400
                };


            TimeTable addTimeTable = new TimeTable
            {
                SubjectDetailId = dto.SubjectDetailId,
                TeacherId = dto.TeacherId,
                GroupId = dto.GroupId,
                DayOfWeek = dto.DayOfWeek,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
            };
            var timeTableNew = await _unitOfWork.TimeTables.AddAsync(addTimeTable);
            _unitOfWork.Save();
            return new Response<TimeTableDto>
            {
                Message = "تمت الاصافة",
                Data = timeTableNew,
                Code = 200
            };
        }


        public async Task<Response<TimeTableDto>> DeleteTimeTable(int id)
        {
            var timeTable = await _unitOfWork.TimeTables.FindAsync(b => b.Id == id);
            if (timeTable == null)
                return new Response<TimeTableDto>
                {
                    Message = " المعلم غير موجود ",
                    Code = 400,
                };


            try
            {
                _unitOfWork.TimeTables.Delete(timeTable);
                _unitOfWork.Save();
                return new Response<TimeTableDto>
                {
                    Code = 200,
                    Message = "تم الحذف",
                    Data = timeTable
                };
            }
            catch
            {
                throw new Exception("هذا السجل مرتبط بجدول آخر");
            }

        }


        public async Task<Response<TimeTableDto>> GetAllTimeTables()
        {
            var timeTables = await _unitOfWork.TimeTables.GetAllAsync();


            var dataDisplay = timeTables.Select(s => new TimeTableDto
            {
                Id = s.Id,
                SubjectDetailId = s.SubjectDetailId,
                TeacherId = s.TeacherId,
                GroupId = s.GroupId,
                DayOfWeek = s.DayOfWeek,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
            });

            return new Response<TimeTableDto>
            {
                Message = "تم جلب المعلمين بنجاح ",
                Data = dataDisplay,
                Code = 200
            };

        }


        public async Task<Response<TimeTableDto>> GetByIdTimeTable(int id)
        {
            var timeTable = await _unitOfWork.TimeTables.GetByIdAsync(id);

            if (timeTable == null)
            {
                return new Response<TimeTableDto>
                {
                    Message = "المعلم غير موجود ",
                    Code = 400,
                };
            }

            return new Response<TimeTableDto>
            {
                Message = "تم العثور على المعلم",
                Code = 200,
                Data = new TimeTableDto
                {
                    Id = timeTable.Id,
                    SubjectDetailId = timeTable.SubjectDetailId,
                    TeacherId = timeTable.TeacherId,
                    GroupId = timeTable.GroupId,
                    DayOfWeek = timeTable.DayOfWeek,
                    StartTime = timeTable.StartTime,
                    EndTime = timeTable.EndTime,
                }
            };

        }


        public async Task<Response<TimeTableDto>> UpdateTimeTable(TimeTableDto dto)
        {
            var timeTable = await _unitOfWork.TimeTables.FindAsync(b => b.Id == dto.Id);
            if (timeTable == null)
                return new Response<TimeTableDto>
                {
                    Message = "المعلم غير موجود ",
                    Code = 400,

                };

            var group = await _unitOfWork.Groups.FindAsync(b => b.Id == dto.GroupId);
            if (group == null)
                return new Response<TimeTableDto>
                {
                    Message = "المستخدم غير موجود ",
                    Code = 400
                };

            var subjectDetail = await _unitOfWork.SubjectDetails.FindAsync(b => b.Id == dto.SubjectDetailId);
            if (subjectDetail == null)
                return new Response<TimeTableDto>
                {
                    Message = "المستخدم غير موجود ",
                    Code = 400
                };

            var teacher = await _unitOfWork.Teachers.FindAsync(b => b.Id == dto.TeacherId);
            if (teacher == null)
                return new Response<TimeTableDto>
                {
                    Message = "المستخدم غير موجود ",
                    Code = 400
                };

            timeTable.SubjectDetailId = dto.SubjectDetailId;
            timeTable.TeacherId = dto.TeacherId;
            timeTable.GroupId = dto.GroupId;
            timeTable.DayOfWeek = dto.DayOfWeek;
            timeTable.StartTime = dto.StartTime;
            timeTable.EndTime = dto.EndTime;

            var TimeTableNew = _unitOfWork.TimeTables.Update(timeTable);
            _unitOfWork.Save();
            return new Response<TimeTableDto>
            {
                Code = 200,
                Message = "تم التعديل",
                Data = new TimeTableDto
                {
                    Id = timeTable.Id,
                    SubjectDetailId = timeTable.SubjectDetailId,
                    TeacherId = timeTable.TeacherId,
                    GroupId = timeTable.GroupId,
                    DayOfWeek = timeTable.DayOfWeek,
                    StartTime = timeTable.StartTime,
                    EndTime = timeTable.EndTime,
                }
            };
        }
    }


    public interface ITimeTableService
    {
        Task<Response<TimeTableDto>> AddTimeTable(TimeTableDto dto);
        Task<Response<TimeTableDto>> GetAllTimeTables();
        Task<Response<TimeTableDto>> GetByIdTimeTable(int id);
        Task<Response<TimeTableDto>> UpdateTimeTable(TimeTableDto dto);
        Task<Response<TimeTableDto>> DeleteTimeTable(int id);
    }
}
