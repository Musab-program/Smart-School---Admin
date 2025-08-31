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
    public class ContentService : IContentService
    {
        // An object is declared to handle database operations as a single unit.
        private readonly IUnitOfWork _unitOfWork;

        // The constructor for the controller 
        public ContentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // End Point To Add Element In This Domin Class
        public async Task<Response<ContentDto>> AddContent(ContentDto dto)
        {
            var content = await _unitOfWork.Contents.FindAsync(r => r.Name == dto.Name && r.SubjectDetailId == dto.SubjectDetailId);
            if (content != null)
            {
                return new Response<ContentDto>
                {
                    Message = "المحتوى موجود مسبقا",
                    Code = 400,
                };
            }

            var subjectDetail = await _unitOfWork.SubjectDetails.FindAsync(a => a.Id == dto.Id);
            if (subjectDetail == null)
                return new Response<ContentDto>
                {
                    Message = "تفاصيل المادة غير موجودة",
                    Code = 400,
                };

            Content addcontent = new Content
            {
                Name = dto.Name,
                Descripion = dto.Descripion,
                VedioUrl = dto.VedioUrl,
                SubjectDetailId = dto.SubjectDetailId,
                AttachmentFile = dto.AttachmentFile,
            };
            var contentNew = await _unitOfWork.Contents.AddAsync(addcontent);
            _unitOfWork.Save();
            return new Response<ContentDto>
            {
                Message = "تمت الإضافة بنجاح",
                Code = 200,
                Data = contentNew,
            };
        }

        // End Point To Delete Element In This Domin Class
        public async Task<Response<ContentDto>> DeleteContent(int id)
        {
            var content = await _unitOfWork.Contents.GetByIdAsync(id);
            if (content == null)
                return new Response<ContentDto>
                {
                    Message = "محتوى المادة الذي تريد حذفه غير موجود",
                    Code = 400,
                };
            try
            {
                _unitOfWork.Contents.Delete(content);
                _unitOfWork.Save();
                return new Response<ContentDto>
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
        public async Task<Response<ContentDto>> GetAllContents()
        {
            var content = await _unitOfWork.Contents.GetAllAsync();
            // Select What Data Will Shows In Respons
            var dataDisplay = content.Select(s => new ContentDto
            {
                Id = s.Id,
                Name = s.Name,
                Descripion = s.Descripion,
                VedioUrl = s.VedioUrl,
                SubjectDetailId = s.SubjectDetailId,
                AttachmentFile = s.AttachmentFile,
            });
            return new Response<ContentDto>
            {
                Data = dataDisplay,
                Code = 200,
                Message = "تم استدعاء جميع محتوى المواد",
            };
        }

        // End Point For Get Element By Id In This Domin Class
        public async Task<Response<ContentDto>> GetContentById(int id)
        {
            var content = await _unitOfWork.Contents.GetByIdAsync(id);
            if (content == null)
                return new Response<ContentDto>
                {
                    Message = "المحتوى الذي تبحث عنه غير موجود",
                    Code = 400,
                };
            return new Response<ContentDto>
            {
                Data = new ContentDto
                {
                    Id = content.Id,
                    Name = content.Name,
                    Descripion = content.Descripion,
                    VedioUrl = content.VedioUrl,
                    SubjectDetailId = content.SubjectDetailId,
                    AttachmentFile = content.AttachmentFile,
                },
                Code = 200,
                Message = "تم استدعاء المحتوى برقم التعريف",
            };
        }

        // End Point To Update Element In This Domin Class
        public async Task<Response<ContentDto>> UpdateContent(ContentDto dto)
        {
            var content = await _unitOfWork.Contents.FindAsync(r => r.Id == dto.Id);
            if (content == null)
                return new Response<ContentDto>
                {
                    Message = "المحتوى الذي تبحث عنه غير موجود مسبقا",
                    Code = 400,
                };

            var subjectDetail = await _unitOfWork.SubjectDetails.FindAsync(a => a.Id == dto.Id);
            if (subjectDetail == null)
                return new Response<ContentDto>
                {
                    Message = "تفاصيل المادة غير موجودة",
                    Code = 400,
                };

            content.Name = dto.Name;
            content.AttachmentFile = dto.AttachmentFile;
            content.Descripion = dto.Descripion;
            content.VedioUrl = dto.VedioUrl;
            content.SubjectDetailId = dto.SubjectDetailId;

            var contentNew = _unitOfWork.Contents.Update(content);
            _unitOfWork.Save();
            return new Response<ContentDto>
            {
                Message = "تم التعديل بنجاح",
                Code = 200,
            };
        }

        public async Task<Response<int>> CountContent()
        {
            var contentCount = await _unitOfWork.Contents.CountAsync();

            return new Response<int>
            {
                Message = "Success",
                Code = 200,
                Data = contentCount
            };
        }
    }

    public interface IContentService
    {
        Task<Response<ContentDto>> GetAllContents();
        Task<Response<ContentDto>> GetContentById(int id);
        Task<Response<ContentDto>> AddContent(ContentDto dto);
        Task<Response<ContentDto>> UpdateContent(ContentDto dto);
        Task<Response<ContentDto>> DeleteContent(int id);
        Task<Response<int>> CountContent();
    }
}
