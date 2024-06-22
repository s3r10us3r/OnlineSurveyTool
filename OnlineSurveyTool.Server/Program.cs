using Microsoft.EntityFrameworkCore;
using OnlineSurveyTool.Server.DAL.Extensions;
using OnlineSurveyTool.Server.DAL.Models;
using OnlineSurveyTool.Server.Services.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("LocalConnectionString");

builder.Services.AddDbContext<OstDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddJWTAuthentication(builder.Configuration);

//repos
builder.Services.AddRepos();

//services
builder.Services.AddServices();

builder.Services.AddAutoMapper();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowAngular",
        policy =>
        {
            policy.WithOrigins("https://127.0.0.1:4200").AllowAnyHeader().AllowAnyMethod();
        }
    );
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseCors("AllowAngular");
app.UseDefaultFiles();
app.UseStaticFiles();

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
