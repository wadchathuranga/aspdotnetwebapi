using authentication.Configurations;
using authentication.DbConnections;
using authentication.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// --------------- JwtConfig ---------------
builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// --------------- Update Swagger to add authorization token ---------------
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Athentication Web API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Please enter a valid token",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id=JwtBearerDefaults.AuthenticationScheme
                }
            },
            new string[]{}
        }
    });
});

// --------------- Define repository interfaces ---------------
builder.Services.AddScoped<IEmployeeService, EmployeeService>();

// --------------- Add identity ---------------
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// --------------- Config Identity ---------------
//builder.Services.Configure<IdentityOptions>(options =>
//{
//    options.Password.RequiredLength = 3;
//    options.Password.RequireDigit = false;
//    options.Password.RequireLowercase = false;
//    options.Password.RequireUppercase = false;
//    options.Password.RequireNonAlphanumeric = false;
//    options.SignIn.RequireConfirmedEmail = false;
//});

// --------------- Add Athentication ---------------
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(jwt => {
        var key = Encoding.ASCII.GetBytes(builder.Configuration.GetSection("JwtConfig:Secret").Value!);
        jwt.SaveToken = true;
        jwt.RequireHttpsMetadata = false;
        jwt.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            //ValidIssuer = builder.Configuration.GetSection("JwtConfig:ValidIssuer").Value!,
            //ValidAudience = builder.Configuration.GetSection("JwtConfig:ValidAudience").Value!,
            RequireExpirationTime = false,
            ValidateLifetime = false,
        };
    });

// --------------- Register database ---------------
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

// --------------- Add Athentication ---------------
app.UseAuthentication();

app.UseAuthorization();

// Middleware define bellow

app.MapControllers();

app.Run();
