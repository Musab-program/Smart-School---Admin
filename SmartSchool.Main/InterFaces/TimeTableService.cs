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
    /// <summary>
    /// This service provides functionalities for managing school timetables,
    /// including adding, updating, deleting, retrieving, and counting schedule entries.
    /// </summary>
    public class TimeTableService : ITimeTableService
    {
        private readonly IUnitOfWork _unitOfWork;


        /// <summary>
        /// Initializes a new instance of the <see cref="TimeTableService"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work for database operations.</param>
        public TimeTableService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        /// <summary>
        /// Adds a new timetable entry to the database.
        /// </summary>
        /// <param name="dto">The data transfer object containing the timetable details.</param>
        /// <returns>
        /// A response object with the newly created timetable entry on success,
        /// or an error message if related entities or the entry already exist.
        /// </returns>
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

            var timeTable = await _unitOfWork.TimeTables.FindAsync(b => b.GroupId == dto.GroupId
            && b.DayOfWeek == dto.DayOfWeek && b.StartTime == dto.StartTime);
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



        /// <summary>
        /// Deletes a timetable entry by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier (ID) of the timetable entry to be deleted.</param>
        /// <returns>
        /// A response object confirming the successful deletion,
        /// or an error message if the timetable entry is not found.
        /// </returns>
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


        /// <summary>
        /// Retrieves a list of all timetable entries from the database.
        /// </summary>
        /// <returns>A response object containing a list of all timetable data transfer objects (TimeTableDto).</returns>
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



        /// <summary>
        /// Retrieves a single timetable entry by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier (ID) of the timetable entry.</param>
        /// <returns>
        /// A response object with the timetable entry data on success,
        /// or an error message if the entry is not found.
        /// </returns>
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


        /// <summary>
        /// Updates an existing timetable entry in the database.
        /// </summary>
        /// <param name="dto">The data transfer object with the updated timetable information.</param>
        /// <returns>
        /// A response object with the updated timetable entry data,
        /// or an error message if the entry or its related entities are not found.
        /// </returns>
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


        /// <summary>
        /// Counts the total number of timetable entries in the database.
        /// </summary>
        /// <returns>A response object containing the total count of timetable entries.</returns>
        public async Task<Response<int>> CountTimeTables()
        {
            var TimeTableCount = await _unitOfWork.TimeTables.CountAsync();

            return new Response<int>
            {
                Message = "Success",
                Code = 200,
                Data = TimeTableCount
            };
        }
    }


    public interface ITimeTableService
    {
        /// <summary>
        /// Adds a new timetable entry.
        /// </summary>
        /// <param name="dto">The timetable data transfer object.</param>
        /// <returns>A response object with the added entry's details.</returns>
        Task<Response<TimeTableDto>> AddTimeTable(TimeTableDto dto);

        /// <summary>
        /// Retrieves all timetable entries.
        /// </summary>
        /// <returns>A response object containing a list of all timetable entries.</returns>
        Task<Response<TimeTableDto>> GetAllTimeTables();

        /// <summary>
        /// Retrieves a timetable entry by its ID.
        /// </summary>
        /// <param name="id">The ID of the entry to retrieve.</param>
        /// <returns>A response object with the entry's details.</returns>
        Task<Response<TimeTableDto>> GetByIdTimeTable(int id);

        /// <summary>
        /// Updates an existing timetable entry.
        /// </summary>
        /// <param name="dto">The timetable data transfer object with updated information.</param>
        /// <returns>A response object with the updated entry's details.</returns>
        Task<Response<TimeTableDto>> UpdateTimeTable(TimeTableDto dto);

        /// <summary>
        /// Deletes a timetable entry by its ID.
        /// </summary>
        /// <param name="id">The ID of the entry to delete.</param>
        /// <returns>A response object confirming the deletion.</returns>
        Task<Response<TimeTableDto>> DeleteTimeTable(int id);

        /// <summary>
        /// Counts the total number of timetable entries.
        /// </summary>
        /// <returns>A response object containing the total count.</returns>
        Task<Response<int>> CountTimeTables();
    }
}
