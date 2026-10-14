
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Mappings;
using NZWalks.API.Repositories;
using NZWalks.API.Repositries;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi;
using System.Security.Cryptography.Xml;
using Microsoft.Extensions.FileProviders;
using Serilog;
using Microsoft.AspNetCore.Diagnostics;
using NZWalks.API.MiddleWares;
var builder = WebApplication.CreateBuilder(args);

var connectionString =
    builder.Configuration.GetConnectionString("NZWalksConnectionString");

Console.WriteLine("DATABASE CONNECTION STRING:");
Console.WriteLine(connectionString);

//Add services to the controller

var logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/NzWalks_Log.txt",rollingInterval: RollingInterval.Minute)
    .MinimumLevel.Warning()
    .CreateLogger();


builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);


builder.Services.AddControllers();

builder.Services.AddHttpContextAccessor();

builder.Services.AddEndpointsApiExplorer();

//some options should be added for the Addswaggergen so that we can add the authorization header when we are making a call


builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "NZ Walks API",
        Version = "v1"
    });

    options.AddSecurityDefinition(
        JwtBearerDefaults.AuthenticationScheme,
        new OpenApiSecurityScheme
        {
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT"
        });

    options.AddSecurityRequirement(document =>
        new OpenApiSecurityRequirement
        {
            [new OpenApiSecuritySchemeReference(
                JwtBearerDefaults.AuthenticationScheme,
                document)] = []
        });
});

builder.Services.AddDbContext<NZWalksDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("NZWalksConnectionString")));


builder.Services.AddDbContext<NZWalksAuthDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("NZWalksAuthConnectionString")));
//here the repository to be injected
//this line will inject the inerface IRegionRepository and with the concerete implementation of SQLRegionrepository
builder.Services.AddScoped<IRegionRepository, SQLRegionRepository>();

//here there is another class that implements IRegionRespository
//that can be injected as

//builder.Services.AddScoped<IRegionRepository, InMemoryRegionRepository>();

builder.Services.AddScoped<IWalkRepository, SQLWalkRepository>();

builder.Services.AddScoped<ITokenRepository,TokenRepository>();

builder.Services.AddScoped<IImageRepository, LocalImageRepository>();

//Automapper should be injected 
builder.Services.AddAutoMapper(cfg => { }, typeof(AutoMapperProfiles).Assembly);

//Identity solution should be added

builder.Services.AddIdentityCore<IdentityUser>()
    .AddRoles<IdentityRole>()
    .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("NZWalks")
    .AddEntityFrameworkStores<NZWalksAuthDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
});

//Add authentication to the services

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    });
var app = builder.Build();

//Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseMiddleware<NZWalks.API.MiddleWares.ExceptionHandlerMiddleware>();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

//A middleware is added here for ASP.Net to serve the static files

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider=new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Images")),
    RequestPath="/Images"
    
});
app.MapControllers();
app.Run();