using Microsoft.EntityFrameworkCore;
using QuanlyPhongtapGymFitnessClub.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ================= 1. CẤU HÌNH DATABASE & DEPENDENCY INJECTION =================
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// ================= CẤU HÌNH JWT AUTHENTICATION =================
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"] ?? "MySuperSecretKeyForGymApp_1234567890!!!";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"] ?? "GymApp",
        ValidAudience = jwtSettings["Audience"] ?? "GymAppClient",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
});

// ================= 2. ĐĂNG KÝ CÁC DỊCH VỤ KHÁC =================
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = System.IO.Path.Combine(AppContext.BaseDirectory, xmlFilename);
    if (System.IO.File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});

// ================= ĐĂNG KÝ DEPENDENCY INJECTION (BUỔI 2) =================
builder.Services.AddScoped<QuanlyPhongtapGymFitnessClub.Services.ITrainerService, QuanlyPhongtapGymFitnessClub.Services.TrainerService>();
builder.Services.AddScoped<QuanlyPhongtapGymFitnessClub.Services.IStaffService, QuanlyPhongtapGymFitnessClub.Services.StaffService>();

// Cấu hình CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// ================= CẤU HÌNH MIDDLEWARE PIPELINE =================
app.UseMiddleware<QuanlyPhongtapGymFitnessClub.Middleware.RequestLoggingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseHttpsRedirection();

// Bắt buộc khai báo Authentication (Kiểm tra vé) TRƯỚC Authorization (Kiểm tra quyền)
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
