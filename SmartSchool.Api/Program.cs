using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SmartSchool.Core.Interfaces;
using SmartSchool.EF;
using SmartSchool.EF.ImplementedClasses;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{   
    /////////
    ///This code to identify the Description for API
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Smart School Api",
        Description = "Genral Api for Smart School project",
        Contact = new OpenApiContact
        {
            Name = "Do it later"
        },

    });

    //////
    ///JWT =>json word token. it allow just for authrized users to access to the api 
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter Your JWT key",
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
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id= "Bearer",
                },
                Name ="Bearer",
                In= ParameterLocation.Header

            },
            new List<string>()
        }
    });


    });





// Connect ApplicationDbContext With Database
//"ApplicationDbContext" is A Class That Initialize The Domain Classes
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration
        .GetConnectionString("DefaultConnection"),
            b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));



/////////
///                         Explanation
///This tell the compiler(ASP.NET Core) You want any type of object from IBaseReporstert<> 
///It will Create an object from type of BaseReporstert<> temporary
///That means everytime you request this service it'll create a new copy of it
/////////
builder.Services.AddTransient(typeof(IBaseReporstery<>), typeof(BaseReporstery<>));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
