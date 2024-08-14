using Microsoft.EntityFrameworkCore;
using OnlineSurveyTool.Server.DAL.Extensions;
using OnlineSurveyTool.Server.DAL.Models;
using OnlineSurveyTool.Server.Services.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDirectoryBrowser();
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
            policy.WithOrigins("https://localhost:4200", "https://127.0.0.1:4200")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapFallbackToFile("index.html");
});

app.Run();
