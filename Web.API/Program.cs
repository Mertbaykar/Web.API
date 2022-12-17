using API.Core.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Web.API.Bases.Repos;
using Web.API.DbContexts;
using Web.API.MapperConfigurations;
using Web.API.Repositories;
using Web.API.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAutoMapper(typeof(MapperConfigurations));
// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(config =>
{
    config.JsonSerializerOptions.WriteIndented = true;
    config.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});
builder.Services.AddDbContext<BusinessContext>(opts =>
{
    opts.UseSqlServer(builder.Configuration.GetConnectionString("sqlConnection"));
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped(typeof(IEmployeeRepository), typeof(EmployeeRepository));
builder.Services.AddScoped(typeof(IProductRepository), typeof(ProductRepository));
builder.Services.AddScoped(typeof(ICategoryRepository), typeof(CategoryRepository));
builder.Services.AddScoped(typeof(ICompanyRepository), typeof(CompanyRepository));
builder.Services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
    };
});
// Baþka yerde set edilmemesi ihtimaline karþý ApiUrl set edildi
ConstantHelper.ApiUrl = builder.Configuration["Jwt:Issuer"] + "/api";

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();