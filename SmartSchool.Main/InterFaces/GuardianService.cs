using System.Text;
using SmartSchool.Core;
using SmartSchool.Core.Models;
using SmartSchool.Core.Shared;
using SmartSchool.Main.Dtos;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SmartSchool.Main.InterFaces
{
    public class GuardianService : IGuardianService
    {
        // An object is declared to handle database operations as a single unit.
        private readonly IUnitOfWork _unitOfWork;

        // The constructor for the controller 
        public GuardianService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // End Point To Add Element In This Domin Class
        public async Task<Response<GuardianDto>> AddGuardian(GuardianDto dto)
        {
            return await _unitOfWork.ExecuteInTransactionAsync<Response<GuardianDto>>(async () =>
            {
                var user = await _unitOfWork.Users.FindAsync(b => b.Name == dto.UserName && b.RoleId == dto.RoleID);
                User UserNew = new();
                if (user == null)
                {
                    var role = await _unitOfWork.Roles.FindAsync(b => b.Id == dto.RoleID);
                    //check dto.RoleId is not existed in role table
                    if (role == null)
                        return new Response<GuardianDto>
                        {
                            Message = "النوع غير موجود",
                            Code = 400
                        };
                    //check dto.RoleId exist in role table but its type not Guardian
                    else if (role.Name != "ولي أمر")
                        return new Response<GuardianDto>
                        {
                            Message = "النوع غير موجود",
                            Code = 400
                        };
                    //check dto.RoleId  exist in role table and its type Gurdian
                    else
                    {
                        User addUser = new User
                        {
                            Name = dto.UserName,
                            Email = dto.Email,
                            DateOfBirth = dto.DateOfBirth,
                            gender = dto.gender,
                            Address = dto.Address,
                            Password = Encoding.UTF8.GetBytes(dto.Password),
                            Phone = dto.Phone,
                            RoleId = dto.RoleID,
                            IsActive = dto.IsActive

                        };
                         UserNew = await _unitOfWork.Users.AddAsync(addUser);
                        _unitOfWork.Save();
                    }
                }
                else
                {
                    //if (user.RoleId != _unitOfWork.Roles.FindAsync(b => b.Name == "ولي أمر").Id)
                    var role = await _unitOfWork.Roles.FindAsync(a => a.Id == user.RoleId);
                    if (role == null)
                        return new Response<GuardianDto>
                        {
                            Message = "النوع غير موجود",
                            Code = 400
                        };
                    //check dto.RoleId exist in role table but its type not Guardian
                    else if (role.Name != "ولي أمر")
                        return new Response<GuardianDto>
                        {
                            Message = "النوع غير موجود",
                            Code = 400
                        };
                }

                var guardian = await _unitOfWork.Guardians.FindAsync(r => r.UserId == dto.UserId);
                if (guardian != null)
                {
                    return new Response<GuardianDto>
                    {
                        Message = "ولي الأمر موجود مسبقا",
                        Code = 400,
                    };
                }
                else
                {
                    var relationType = await _unitOfWork.RelationTypes.FindAsync(a => a.Id == dto.RelationTypeId);
                    if (relationType == null)
                        return new Response<GuardianDto>
                        {
                            Message = "العلاقة غير موجودة",
                            Code = 400,
                        };
                    else
                    {
                        Guardian addGuardian = new Guardian
                        {
                            RelationTypeId = dto.RelationTypeId,
                            SecondryPhone = dto.SecondryPhone,
                            UserId = UserNew.Id == 0 ? user.Id : UserNew.Id,
                            //UserId = UserNew.Id == 0 ? user.Id : UserNew.Id,
                        };


                        var guardianNew = await _unitOfWork.Guardians.AddAsync(addGuardian);
                        _unitOfWork.Save();
                        return new Response<GuardianDto>
                        {
                            Message = "تمت الإضافة بنجاح",
                            Code = 200,
                            Data = guardianNew,
                        };
                    }
                }

            });


        }

        // End Point To Delete Element In This Domin Class
        public async Task<Response<GuardianDto>> DeleteGuardian(int id)
        {
            return await _unitOfWork.ExecuteInTransactionAsync<Response<GuardianDto>>(async () =>
            {
                var guardian = await _unitOfWork.Guardians.GetByIdAsync(id);
                if (guardian == null)
                    return new Response<GuardianDto>
                    {
                        Message = "ولي الأمر الذي تريد حذفه غير موجود",
                        Code = 400,
                    };

                var user = await _unitOfWork.Users.FindAsync(x => x.Id == guardian.UserId);
                
                _unitOfWork.Guardians.Delete(guardian);
                _unitOfWork.Users.Delete(user);
                _unitOfWork.Save();
                return new Response<GuardianDto>
                {
                    Data = new GuardianDto
                    {
                        Id = guardian.Id,
                        UserName = user.Name,
                        Email = user.Email,
                        Address = user.Address,
                        DateOfBirth = user.DateOfBirth,
                        IsActive = user.IsActive,
                        gender = user.gender,
                        Phone = user.Phone,
                        RelationTypeId = guardian.RelationTypeId,
                        RoleID = user.RoleId,
                        SecondryPhone = guardian.SecondryPhone,
                        UserId = guardian.UserId,
                    },
                    Message = "تم الحذف بنجاح",
                    Code = 200,
                };
            });
        }

        // End Point For Get All Elements In This Domin Class
        public async Task<Response<GuardianDto>> GetAllGuardians()
        {
            var guardian = await _unitOfWork.Guardians.FindAllAsync(s => s.Id >= 1, ["User"]);
            // Select What Data Will Shows In Respons
            var dataDisplay = guardian.Select(s => new GuardianDto
            {
                Id = s.Id,
                UserId = s.UserId,
                SecondryPhone = s.SecondryPhone,
                RelationTypeId = s.RelationTypeId,
                UserName = s.User.Name,
                Address = s.User.Address,
                DateOfBirth= s.User.DateOfBirth,
                Email = s.User.Email,
                gender = s.User.gender,
                IsActive = s.User.IsActive,
                Phone = s.User.Phone,
                RoleID = s.User.RoleId,
            });

            return new Response<GuardianDto>
            {
                Data = dataDisplay,
                Code = 200,
                Message = "تم استدعاء جميع أولياء الأمور",
            };
        }

        // End Point For Get Element By Id In This Domin Class
        public async Task<Response<GuardianDto>> GetGuardianById(int id)
        {

            var guardian = await _unitOfWork.Guardians.GetByIdAsync(id);
            if (guardian == null)
                return new Response<GuardianDto>
                {
                    Message = "ولي الأمر الذي تبحث عنه غير موجود",
                    Code = 400,
                };

            var user = await _unitOfWork.Users.FindAsync(x => x.Id == guardian.UserId);

            return new Response<GuardianDto>
            {
                Data = new GuardianDto
                {
                    Id = guardian.Id,
                    UserName = user.Name,
                    Email = user.Email,
                    Address = user.Address,
                    DateOfBirth = user.DateOfBirth,
                    IsActive = user.IsActive,
                    gender = user.gender,
                    Phone = user.Phone,
                    RelationTypeId = guardian.RelationTypeId,
                    RoleID = user.RoleId,
                    SecondryPhone = guardian.SecondryPhone,
                    UserId = guardian.UserId,
                },
                Code = 200,
                Message = "تم استدعاء ولي الأمر برقم التعريف",
            };
        }

        // End Point To Update Element In This Domin Class
        public async Task<Response<GuardianDto>> UpdateGuardian(GuardianDto dto)
        {
            return await _unitOfWork.ExecuteInTransactionAsync<Response<GuardianDto>>(async () =>
            {
                var guardian = await _unitOfWork.Guardians.FindAsync(r => r.Id == dto.Id);
                if (guardian == null)
                    return new Response<GuardianDto>
                    {
                        Message = "ولي الأمر الذي تبحث عنه غير موجود",
                        Code = 400,
                    };
                var user = await _unitOfWork.Users.FindAsync(a => a.Id == dto.UserId);
                if(user == null)
                    return new Response<GuardianDto>
                    {
                        Message = "المستخدم الذي تبحث عنه غير موجود",
                        Code = 400,
                    };
                var role = await _unitOfWork.Roles.FindAsync(a => a.Id == user.RoleId);
                //check dto.RoleId exist in role table but its type not Guardian
                if (role.Name == "ولي أمر")
                {
                    var relationType = await _unitOfWork.RelationTypes.FindAsync(a => a.Id == dto.RelationTypeId);
                    if (relationType == null)
                        return new Response<GuardianDto>
                        {
                            Message = "العلاقة غير موجودة",
                            Code = 400,
                        };
                    else
                    {
                        // Update Guardian Property
                        guardian.SecondryPhone = dto.SecondryPhone;
                        guardian.UserId = dto.UserId;
                        guardian.RelationTypeId = dto.RelationTypeId;
                        // Update User Property
                        user.Name = dto.UserName;
                        user.Email = dto.Email;
                        user.DateOfBirth = dto.DateOfBirth;
                        user.gender = dto.gender;
                        user.Address = dto.Address;
                        user.Password = Encoding.UTF8.GetBytes(dto.Password);
                        user.Phone = dto.Phone;
                        user.IsActive = dto.IsActive;

                        var guardianNew = _unitOfWork.Guardians.Update(guardian);
                        var userNew = _unitOfWork.Users.Update(user);
                        _unitOfWork.Save();
                        return new Response<GuardianDto>
                        {
                            Data = new GuardianDto
                            {
                                Id = guardian.Id,
                                UserName = user.Name,
                                Email = user.Email,
                                Address = user.Address,
                                DateOfBirth = user.DateOfBirth,
                                IsActive = user.IsActive,
                                gender = user.gender,
                                Phone = user.Phone,
                                RelationTypeId = guardian.RelationTypeId,
                                RoleID = user.RoleId,
                                SecondryPhone = guardian.SecondryPhone,
                                UserId = guardian.UserId,
                            },
                            Message = "تم التعديل بنجاح",
                            Code = 200,
                        };
                    }
                }
                else
                    return new Response<GuardianDto>
                    {
                        Message = " النوع غير موجود أو غير متوافق",
                        Code = 400
                    };

                
            });

                
            }
    }

    public interface IGuardianService
    {
        Task<Response<GuardianDto>> GetAllGuardians();
        Task<Response<GuardianDto>> GetGuardianById(int id);
        Task<Response<GuardianDto>> AddGuardian(GuardianDto dto);
        Task<Response<GuardianDto>> UpdateGuardian(GuardianDto dto);
        Task<Response<GuardianDto>> DeleteGuardian(int id);
    }
}
