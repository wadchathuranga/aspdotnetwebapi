using authentication.Configurations;
using authentication.DbConnections;
using authentication.Models;
using authentication.Services;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// JwtConfig
builder.Services.Configure<JwtSecret>(builder.Configuration.GetSection("JwtConfig"));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Define repository interfaces
builder.Services.AddScoped<IUserService, UserService>();

// Add Athentication
builder.Services.AddAuthentication().AddJwtBearer();

// Add Authorization
//builder.Services.AddAuthorizationBuilder();

// Register database
builder.Services.AddDbContext<AppDbContext>();



var app = builder.Build();




//Enable CORS
//app.UseCors(c => c.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
