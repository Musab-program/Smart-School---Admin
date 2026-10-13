using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RepositoryPatternWithUOW.EF;
using SmartSchool.Core;
using SmartSchool.Core.Interfaces;
using SmartSchool.Core.Models;
using SmartSchool.EF;
using SmartSchool.EF.ImplementedClasses;
using SmartSchool.Main.InterFaces;
using SmartSchool.Api;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// ─────────────────────────────────────────────────────────────
// Controllers + Swagger
// ─────────────────────────────────────────────────────────────
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Smart School Api",
        Description = "General API for Smart School project",
        Contact = new OpenApiContact { Name = "Smart School Team" },
    });

    // Allow sending JWT from Swagger UI
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter: Bearer {your JWT token}",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" },
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

// ─────────────────────────────────────────────────────────────
// Database
// ─────────────────────────────────────────────────────────────
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

// ─────────────────────────────────────────────────────────────
// ASP.NET Core Identity
// Provides: UserManager, RoleManager, password hashing,
//           lockout, security stamp validation, etc.
// ─────────────────────────────────────────────────────────────
builder.Services.AddIdentity<User, Role>(options =>
{
    // Password policy
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;

    // Lockout on repeated failures
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);

    // Unique email per user
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// ─────────────────────────────────────────────────────────────
// JWT Authentication
// ─────────────────────────────────────────────────────────────
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme    = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
        ValidateIssuer   = true,
        ValidIssuer      = builder.Configuration["Jwt:Issuer"],
        ValidateAudience = true,
        ValidAudience    = builder.Configuration["Jwt:Audience"],
        ValidateLifetime = true,
        ClockSkew        = TimeSpan.Zero  // no grace period on token expiry
    };
});

// ─────────────────────────────────────────────────────────────
// Repository Pattern + Unit of Work
// ─────────────────────────────────────────────────────────────
builder.Services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

// ─────────────────────────────────────────────────────────────
// Application Services
// ─────────────────────────────────────────────────────────────
builder.Services.AddTransient<IAuthService,               AuthService>();
builder.Services.AddTransient<IUserService,               UserService>();
builder.Services.AddTransient<IRelationTypeService,       RelationTypeService>();
builder.Services.AddTransient<ISpecialtyService,          SpecialtyService>();
builder.Services.AddTransient<IStudentService,            StudentService>();
builder.Services.AddTransient<IStudentAttendanceService,  StudentAttendanceService>();
builder.Services.AddTransient<ITeacherService,            TeacherService>();
builder.Services.AddTransient<ISubjectService,            SubjectService>();
builder.Services.AddTransient<ISubjectDetailService,      SubjectDetailService>();
builder.Services.AddTransient<ISubmittedAssignmentService,SubmittedAssignmentService>();
builder.Services.AddTransient<ITeachingSubjectService,    TeachingSubjectService>();
builder.Services.AddTransient<ITimeTableService,          TimeTableService>();
builder.Services.AddTransient<IRoleService,               RoleService>();
builder.Services.AddTransient<IAssignmentService,         AssignmentService>();
builder.Services.AddTransient<IContentService,            ContentService>();
builder.Services.AddTransient<IExamService,               ExamService>();
builder.Services.AddTransient<IExamTypeService,           ExamTypeService>();
builder.Services.AddTransient<IGradeService,              GradeService>();
builder.Services.AddTransient<IGroupService,              GroupService>();
builder.Services.AddTransient<IGuardianService,           GuardianService>();
builder.Services.AddTransient<IResulteService,            ResulteService>();
builder.Services.AddTransient<ITeacherHolidayService,     TeacherHolidayService>();

// ─────────────────────────────────────────────────────────────
// CORS
// ─────────────────────────────────────────────────────────────
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowNextJs", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://localhost:3001")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// ─────────────────────────────────────────────────────────────
// Build + Pipeline
// ─────────────────────────────────────────────────────────────
var app = builder.Build();

// ─────────────────────────────────────────────────────────────
// Database Seeder (Delegated to DataSeeder.cs)
// ─────────────────────────────────────────────────────────────
await DataSeeder.SeedAsync(app.Services);

app.UseCors("AllowNextJs");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();   // ← Must come BEFORE UseAuthorization
app.UseAuthorization();

app.MapControllers();

app.Run();
