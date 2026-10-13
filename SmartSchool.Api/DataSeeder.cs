using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Core.Models;
using SmartSchool.EF;

namespace SmartSchool.Api
{
    public static class DataSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // Ensure database is migrated
            await context.Database.MigrateAsync();

            // 1. Create standard system roles
            string[] standardRoles = { "أدمن", "معلم", "ولي أمر", "طالب" };
            foreach (var roleName in standardRoles)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new Role { Name = roleName });
                }
            }

            var adminRole = await context.Roles.FirstAsync(r => r.Name == "أدمن");
            var teacherRole = await context.Roles.FirstAsync(r => r.Name == "معلم");
            var guardianRole = await context.Roles.FirstAsync(r => r.Name == "ولي أمر");
            var studentRole = await context.Roles.FirstAsync(r => r.Name == "طالب");

            // 2. Seed Admin user
            if (!await userManager.Users.AnyAsync(u => u.RoleId == adminRole.Id))
            {
                var admin = new User
                {
                    Name = "مدير النظام",
                    UserName = "admin@school.com",
                    Email = "admin@school.com",
                    PhoneNumber = "0500000000",
                    RoleId = adminRole.Id,
                    IsActive = true,
                    Address = "صنعاء - الإدارة العامة",
                    gender = "ذكر",
                    DateOfBirth = new DateTime(1985, 1, 1),
                    CreatedAt = DateTime.UtcNow
                };
                await userManager.CreateAsync(admin, "Admin@1234");
            }

            // 3. Seed 12 Grades (الصف الأول الابتدائي إلى الصف الثالث الثانوي)
            var allGrades = new List<Grade>
            {
                new Grade { Name = "الصف الأول الابتدائي", Stage = "ابتدائي", Capacity = 100 },
                new Grade { Name = "الصف الثاني الابتدائي", Stage = "ابتدائي", Capacity = 100 },
                new Grade { Name = "الصف الثالث الابتدائي", Stage = "ابتدائي", Capacity = 100 },
                new Grade { Name = "الصف الرابع الابتدائي", Stage = "ابتدائي", Capacity = 100 },
                new Grade { Name = "الصف الخامس الابتدائي", Stage = "ابتدائي", Capacity = 100 },
                new Grade { Name = "الصف السادس الابتدائي", Stage = "ابتدائي", Capacity = 100 },
                new Grade { Name = "الصف السابع الأساسي", Stage = "أساسي", Capacity = 120 },
                new Grade { Name = "الصف الثامن الأساسي", Stage = "أساسي", Capacity = 120 },
                new Grade { Name = "الصف التاسع الأساسي", Stage = "أساسي", Capacity = 120 },
                new Grade { Name = "الصف الأول الثانوي", Stage = "ثانوي", Capacity = 120 },
                new Grade { Name = "الصف الثاني الثانوي", Stage = "ثانوي", Capacity = 120 },
                new Grade { Name = "الصف الثالث الثانوي", Stage = "ثانوي", Capacity = 120 }
            };

            foreach (var grade in allGrades)
            {
                if (!await context.Grades.AnyAsync(g => g.Name == grade.Name))
                {
                    await context.Grades.AddAsync(grade);
                }
            }
            await context.SaveChangesAsync();
            var gradesInDb = await context.Grades.OrderBy(g => g.Id).ToListAsync();

            // 4. Seed Specialties if count < 5
            if (await context.Specialties.CountAsync() < 5)
            {
                var existingSpecNames = await context.Specialties.Select(s => s.Name).ToListAsync();
                var defaultSpecs = new List<Specialty>
                {
                    new Specialty { Name = "رياضيات", Qualification = "بكالوريوس علوم ورياضيات" },
                    new Specialty { Name = "لغة عربية", Qualification = "بكالوريوس لغة عربية وآدابها" },
                    new Specialty { Name = "لغة إنجليزية", Qualification = "بكالوريوس ترجمة ولغة إنجليزية" },
                    new Specialty { Name = "علوم وفيزياء", Qualification = "بكالوريوس فيزياء تطبيقية" },
                    new Specialty { Name = "حاسوب وتكنولوجيا", Qualification = "بكالوريوس علوم حاسوب" }
                };

                foreach (var s in defaultSpecs)
                {
                    if (!existingSpecNames.Contains(s.Name))
                    {
                        await context.Specialties.AddAsync(s);
                    }
                }
                await context.SaveChangesAsync();
            }
            var specialties = await context.Specialties.ToListAsync();

            // 5. Seed RelationTypes if count < 5
            if (await context.RelationTypes.CountAsync() < 5)
            {
                var existingRelNames = await context.RelationTypes.Select(r => r.Name).ToListAsync();
                var defaultRelations = new List<RelationType>
                {
                    new RelationType { Name = "أب" },
                    new RelationType { Name = "أم" },
                    new RelationType { Name = "أخ" },
                    new RelationType { Name = "عم" },
                    new RelationType { Name = "خال" }
                };

                foreach (var r in defaultRelations)
                {
                    if (!existingRelNames.Contains(r.Name))
                    {
                        await context.RelationTypes.AddAsync(r);
                    }
                }
                await context.SaveChangesAsync();
            }
            var relationTypes = await context.RelationTypes.ToListAsync();

            // 6. Seed Groups for grades if count < 10
            if (await context.Groups.CountAsync() < 10)
            {
                var defaultGroups = new List<Group>();
                foreach (var grade in gradesInDb.Take(4))
                {
                    defaultGroups.Add(new Group { Name = $"{grade.Name} - شعبة (أ)", GradeId = grade.Id, AcademicYear = new DateTime(2026, 9, 1) });
                    defaultGroups.Add(new Group { Name = $"{grade.Name} - شعبة (ب)", GradeId = grade.Id, AcademicYear = new DateTime(2026, 9, 1) });
                }

                foreach (var g in defaultGroups)
                {
                    if (!await context.Groups.AnyAsync(grp => grp.Name == g.Name))
                    {
                        await context.Groups.AddAsync(g);
                    }
                }
                await context.SaveChangesAsync();
            }
            var groups = await context.Groups.ToListAsync();

            // 7. Seed 5 Teachers
            if (await context.Teachers.CountAsync() < 5)
            {
                var teacherSeeds = new[]
                {
                    new { Name = "أحمد محمد العلي", Email = "teacher.ahmed@school.com", Phone = "771111001", Gender = "ذكر", SpecIndex = 0, Salary = 120000.0, Addr = "صنعاء - حدة" },
                    new { Name = "فاطمة حسن القاسمي", Email = "teacher.fatima@school.com", Phone = "771111002", Gender = "أنثى", SpecIndex = 1, Salary = 115000.0, Addr = "صنعاء - الأصبحي" },
                    new { Name = "خالد عبدالله الشامي", Email = "teacher.khaled@school.com", Phone = "771111003", Gender = "ذكر", SpecIndex = 2, Salary = 130000.0, Addr = "صنعاء - الستين" },
                    new { Name = "ريم سالم باوزير", Email = "teacher.reem@school.com", Phone = "771111004", Gender = "أنثى", SpecIndex = 3, Salary = 125000.0, Addr = "صنعاء - الصافية" },
                    new { Name = "طارق يحيى الكبسي", Email = "teacher.tariq@school.com", Phone = "771111005", Gender = "ذكر", SpecIndex = 4, Salary = 140000.0, Addr = "صنعاء - مذبح" },
                };

                foreach (var t in teacherSeeds)
                {
                    if (!await userManager.Users.AnyAsync(u => u.Email == t.Email))
                    {
                        var user = new User
                        {
                            Name = t.Name,
                            UserName = t.Email,
                            Email = t.Email,
                            PhoneNumber = t.Phone,
                            RoleId = teacherRole.Id,
                            IsActive = true,
                            Address = t.Addr,
                            gender = t.Gender,
                            DateOfBirth = new DateTime(1988, 5, 12),
                            CreatedAt = DateTime.UtcNow
                        };
                        var userRes = await userManager.CreateAsync(user, "Teacher@1234");
                        if (userRes.Succeeded)
                        {
                            var specId = specialties[t.SpecIndex % specialties.Count].Id;
                            var teacher = new Teacher
                            {
                                UserId = user.Id,
                                SpecialtyId = specId,
                                Salary = t.Salary,
                                HireDate = DateTime.UtcNow
                            };
                            await context.Teachers.AddAsync(teacher);
                        }
                    }
                }
                await context.SaveChangesAsync();
            }

            // 8. Seed 5 Guardians
            if (await context.Guardians.CountAsync() < 5)
            {
                var guardianSeeds = new[]
                {
                    new { Name = "صالح ناصر المهدي", Email = "guardian.saleh@school.com", Phone = "772222001", Gender = "ذكر", RelIndex = 0, Addr = "صنعاء - المطار", SecPhone = "733333001" },
                    new { Name = "سعاد عمر بازرعة", Email = "guardian.suad@school.com", Phone = "772222002", Gender = "أنثى", RelIndex = 1, Addr = "صنعاء - بغداد", SecPhone = "733333002" },
                    new { Name = "منصور إبراهيم الحاشدي", Email = "guardian.mansoor@school.com", Phone = "772222003", Gender = "ذكر", RelIndex = 0, Addr = "صنعاء - التحرير", SecPhone = "733333003" },
                    new { Name = "هدى عبده الظفيري", Email = "guardian.huda@school.com", Phone = "772222004", Gender = "أنثى", RelIndex = 1, Addr = "صنعاء - الدائري", SecPhone = "733333004" },
                    new { Name = "عصام فؤاد الأنسي", Email = "guardian.issam@school.com", Phone = "772222005", Gender = "ذكر", RelIndex = 3, Addr = "صنعاء - شميلة", SecPhone = "733333005" },
                };

                foreach (var g in guardianSeeds)
                {
                    if (!await userManager.Users.AnyAsync(u => u.Email == g.Email))
                    {
                        var user = new User
                        {
                            Name = g.Name,
                            UserName = g.Email,
                            Email = g.Email,
                            PhoneNumber = g.Phone,
                            RoleId = guardianRole.Id,
                            IsActive = true,
                            Address = g.Addr,
                            gender = g.Gender,
                            DateOfBirth = new DateTime(1978, 3, 20),
                            CreatedAt = DateTime.UtcNow
                        };
                        var userRes = await userManager.CreateAsync(user, "Guardian@1234");
                        if (userRes.Succeeded)
                        {
                            var relId = relationTypes[g.RelIndex % relationTypes.Count].Id;
                            var guardian = new Guardian
                            {
                                UserId = user.Id,
                                RelationTypeId = relId,
                                SecondryPhone = g.SecPhone
                            };
                            await context.Guardians.AddAsync(guardian);
                        }
                    }
                }
                await context.SaveChangesAsync();
            }
            var guardians = await context.Guardians.ToListAsync();

            // 9. Seed 5 Students
            if (await context.Students.CountAsync() < 5 && guardians.Any() && groups.Any())
            {
                var studentSeeds = new[]
                {
                    new { Name = "محمد صالح المهدي", Email = "student.mohammed@school.com", Phone = "773333001", Gender = "ذكر", GuardIdx = 0, GroupIdx = 0, Addr = "صنعاء - المطار" },
                    new { Name = "سارة ناصر بازرعة", Email = "student.sara@school.com", Phone = "773333002", Gender = "أنثى", GuardIdx = 1, GroupIdx = 1, Addr = "صنعاء - بغداد" },
                    new { Name = "يوسف منصور الحاشدي", Email = "student.youssef@school.com", Phone = "773333003", Gender = "ذكر", GuardIdx = 2, GroupIdx = 2, Addr = "صنعاء - التحرير" },
                    new { Name = "مريم خالد الظفيري", Email = "student.maryam@school.com", Phone = "773333004", Gender = "أنثى", GuardIdx = 3, GroupIdx = 3, Addr = "صنعاء - الدائري" },
                    new { Name = "حمزة عصام الأنسي", Email = "student.hamza@school.com", Phone = "773333005", Gender = "ذكر", GuardIdx = 4, GroupIdx = 4, Addr = "صنعاء - شميلة" },
                };

                foreach (var s in studentSeeds)
                {
                    if (!await userManager.Users.AnyAsync(u => u.Email == s.Email))
                    {
                        var user = new User
                        {
                            Name = s.Name,
                            UserName = s.Email,
                            Email = s.Email,
                            PhoneNumber = s.Phone,
                            RoleId = studentRole.Id,
                            IsActive = true,
                            Address = s.Addr,
                            gender = s.Gender,
                            DateOfBirth = new DateTime(2009, 8, 15),
                            CreatedAt = DateTime.UtcNow
                        };
                        var userRes = await userManager.CreateAsync(user, "Student@1234");
                        if (userRes.Succeeded)
                        {
                            var gId = guardians[s.GuardIdx % guardians.Count].Id;
                            var grpId = groups[s.GroupIdx % groups.Count].Id;
                            var student = new Student
                            {
                                UserId = user.Id,
                                GuardianId = gId,
                                GroupId = grpId,
                                RegisterDate = DateTime.UtcNow
                            };
                            await context.Students.AddAsync(student);
                        }
                    }
                }
                await context.SaveChangesAsync();
            }

            // 10. Seed Phase 3 Subjects if none exist
            if (!await context.Subjects.AnyAsync())
            {
                var defaultSubjects = new List<Subject>
                {
                    new Subject { Name = "القرآن الكريم والتربية الإسلامية" },
                    new Subject { Name = "اللغة العربية" },
                    new Subject { Name = "اللغة الإنجليزية" },
                    new Subject { Name = "الرياضيات" },
                    new Subject { Name = "العلوم العامة" },
                    new Subject { Name = "الفيزياء" },
                    new Subject { Name = "الكيمياء" },
                    new Subject { Name = "الأحياء" },
                    new Subject { Name = "الحاسوب وتكنولوجيا المعلومات" },
                    new Subject { Name = "الاجتماعيات والتاريخ" }
                };

                await context.Subjects.AddRangeAsync(defaultSubjects);
                await context.SaveChangesAsync();
            }
            var subjects = await context.Subjects.ToListAsync();
            var teachers = await context.Teachers.ToListAsync();

            // 11. Seed SubjectDetails (Curriculum & Study Plan) if none exist
            if (!await context.SubjectDetails.AnyAsync() && subjects.Any() && gradesInDb.Any())
            {
                var subjectDetailsList = new List<SubjectDetail>();
                foreach (var grade in gradesInDb.Take(4))
                {
                    foreach (var sub in subjects.Take(5))
                    {
                        subjectDetailsList.Add(new SubjectDetail
                        {
                            GradeId = grade.Id,
                            SubjectId = sub.Id,
                            IsActive = true
                        });
                    }
                }
                await context.SubjectDetails.AddRangeAsync(subjectDetailsList);
                await context.SaveChangesAsync();
            }
            var subjectDetails = await context.SubjectDetails.ToListAsync();

            // 12. Seed TeachingSubjects (Teacher Assignments to Subjects)
            if (!await context.TeachingSubjects.AnyAsync() && teachers.Any() && subjectDetails.Any())
            {
                var teachingList = new List<TeachingSubject>();
                for (int i = 0; i < Math.Min(teachers.Count, subjectDetails.Count); i++)
                {
                    teachingList.Add(new TeachingSubject
                    {
                        TeacherId = teachers[i].Id,
                        SubjectDetailId = subjectDetails[i].Id
                    });
                }
                await context.TeachingSubjects.AddRangeAsync(teachingList);
                await context.SaveChangesAsync();
            }

            // 13. Seed ExamTypes if none exist
            if (!await context.ExamTypes.AnyAsync())
            {
                var examTypesList = new List<ExamType>
                {
                    new ExamType { Name = "اختبار الشهر الأول", Year = new DateTime(2026, 9, 1) },
                    new ExamType { Name = "اختبار منتصف الفصل الأول", Year = new DateTime(2026, 11, 15) },
                    new ExamType { Name = "اختبار نهائي الفصل الأول", Year = new DateTime(2027, 1, 10) },
                    new ExamType { Name = "اختبار الشهر الثاني", Year = new DateTime(2027, 3, 1) },
                    new ExamType { Name = "اختبار نهائي الفصل الثاني", Year = new DateTime(2027, 5, 20) }
                };
                await context.ExamTypes.AddRangeAsync(examTypesList);
                await context.SaveChangesAsync();
            }
            var examTypes = await context.ExamTypes.ToListAsync();

            // 14. Seed Exams & Resulte if none exist
            if (!await context.Exams.AnyAsync() && subjectDetails.Any() && groups.Any() && examTypes.Any())
            {
                var examsList = new List<Exam>();
                var baseDate = DateTime.UtcNow.AddDays(7);
                for (int i = 0; i < Math.Min(5, subjectDetails.Count); i++)
                {
                    examsList.Add(new Exam
                    {
                        SubjectDetailId = subjectDetails[i].Id,
                        GroupId = groups[i % groups.Count].Id,
                        ExamTypeId = examTypes[i % examTypes.Count].Id,
                        ExamDate = baseDate.AddDays(i * 2),
                        LimitTime = 90
                    });
                }
                await context.Exams.AddRangeAsync(examsList);
                await context.SaveChangesAsync();
            }

            var studentsInDb = await context.Students.ToListAsync();
            if (!await context.Resultes.AnyAsync() && subjectDetails.Any() && studentsInDb.Any())
            {
                var resultsList = new List<Resulte>();
                var marks = new[] { 95, 88, 76, 92, 84 };
                var rates = new[] { "ممتاز", "جيد جداً", "جيد", "ممتاز", "جيد جداً" };
                for (int i = 0; i < Math.Min(studentsInDb.Count, marks.Length); i++)
                {
                    resultsList.Add(new Resulte
                    {
                        SubjectDetailId = subjectDetails[i % subjectDetails.Count].Id,
                        StudentId = studentsInDb[i].Id,
                        Mark = marks[i],
                        Rate = rates[i]
                    });
                }
                await context.Resultes.AddRangeAsync(resultsList);
                await context.SaveChangesAsync();
            }

            // 15. Seed Assignments & SubmittedAssignments
            if (!await context.Assignments.AnyAsync() && subjectDetails.Any())
            {
                var assignmentsList = new List<Assignment>
                {
                    new Assignment { SubjectDetailId = subjectDetails[0].Id, Title = "واجب الرياضيات: حل تمارين الوحدة الأولى", LastDate = DateTime.UtcNow.AddDays(5), UploadDate = DateTime.UtcNow },
                    new Assignment { SubjectDetailId = subjectDetails[1 % subjectDetails.Count].Id, Title = "واجب اللغة العربية: كتابة مقال تعبيري", LastDate = DateTime.UtcNow.AddDays(4), UploadDate = DateTime.UtcNow },
                    new Assignment { SubjectDetailId = subjectDetails[2 % subjectDetails.Count].Id, Title = "واجب العلوم: تقرير تجربة الحركة والقوى", LastDate = DateTime.UtcNow.AddDays(6), UploadDate = DateTime.UtcNow },
                    new Assignment { SubjectDetailId = subjectDetails[3 % subjectDetails.Count].Id, Title = "واجب اللغة الإنجليزية: Grammar Worksheet 1", LastDate = DateTime.UtcNow.AddDays(3), UploadDate = DateTime.UtcNow },
                    new Assignment { SubjectDetailId = subjectDetails[4 % subjectDetails.Count].Id, Title = "واجب الحاسوب: مشروع تصميم صفحة ويب بسيطة", LastDate = DateTime.UtcNow.AddDays(7), UploadDate = DateTime.UtcNow }
                };
                await context.Assignments.AddRangeAsync(assignmentsList);
                await context.SaveChangesAsync();
            }
            var assignmentsInDb = await context.Assignments.ToListAsync();

            if (!await context.SubmittedAssignments.AnyAsync() && assignmentsInDb.Any() && studentsInDb.Any())
            {
                var submittedList = new List<SubmittedAssignment>();
                for (int i = 0; i < Math.Min(studentsInDb.Count, assignmentsInDb.Count); i++)
                {
                    submittedList.Add(new SubmittedAssignment
                    {
                        StudentId = studentsInDb[i].Id,
                        AssignmentId = assignmentsInDb[i].Id,
                        FilePath = $"uploads/assignments/student_{studentsInDb[i].Id}_sol.pdf",
                        Mark = 10,
                        SubmittedDate = DateTime.UtcNow,
                        ChekeState = "مكتمل",
                        Notes = "عمل متقن ومتميز"
                    });
                }
                await context.SubmittedAssignments.AddRangeAsync(submittedList);
                await context.SaveChangesAsync();
            }

            // 16. Seed Weekly TimeTables
            if (!await context.TimeTables.AnyAsync() && groups.Any() && subjectDetails.Any() && teachers.Any())
            {
                var days = new[] { "الأحد", "الإثنين", "الثلاثاء", "الأربعاء", "الخميس" };
                var timeTablesList = new List<TimeTable>();
                var sampleGroup = groups.First();

                for (int d = 0; d < days.Length; d++)
                {
                    for (int p = 0; p < 3; p++)
                    {
                        var startHour = 8 + p;
                        var startTime = DateTime.Today.AddHours(startHour);
                        var endTime = startTime.AddMinutes(45);

                        timeTablesList.Add(new TimeTable
                        {
                            GroupId = sampleGroup.Id,
                            SubjectDetailId = subjectDetails[(d + p) % subjectDetails.Count].Id,
                            TeacherId = teachers[(d + p) % teachers.Count].Id,
                            DayOfWeek = days[d],
                            StartTime = startTime,
                            EndTime = endTime
                        });
                    }
                }
                await context.TimeTables.AddRangeAsync(timeTablesList);
                await context.SaveChangesAsync();
            }

            // 17. Seed Student Attendances
            if (!await context.StudentAttendances.AnyAsync() && studentsInDb.Any() && teachers.Any())
            {
                var attList = new List<StudentAttendance>();
                var primaryTeacher = teachers.First();
                var today = DateTime.Today.AddHours(8);
                var statuses = new[] { "حاضر", "حاضر", "متأخر", "غائب", "حاضر" };

                for (int i = 0; i < studentsInDb.Count; i++)
                {
                    attList.Add(new StudentAttendance
                    {
                        StudentId = studentsInDb[i].Id,
                        TeacherId = primaryTeacher.Id,
                        AttendanceDate = today,
                        Status = statuses[i % statuses.Length]
                    });
                }
                await context.StudentAttendances.AddRangeAsync(attList);
                await context.SaveChangesAsync();
            }
        }
    }
}
