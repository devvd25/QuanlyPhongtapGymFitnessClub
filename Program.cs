using Microsoft.EntityFrameworkCore;
using QuanlyPhongtapGymFitnessClub.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ================= 1. CAU HINH DATABASE & DEPENDENCY INJECTION =================
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// ================= CAU HINH JWT AUTHENTICATION =================
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

// ================= 2. DANG KY CAC DICH VU KHAC =================
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Gym Fitness Club Management API",
        Version = "v1",
        Description = "API Quan ly Phong tap Gym Fitness Club - Lap Trinh Web 2 (LTW2)"
    });

    var xmlFilename = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = System.IO.Path.Combine(AppContext.BaseDirectory, xmlFilename);
    if (System.IO.File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }

    // Cau hinh o khoa JWT Bearer cho Swagger UI
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Nhap JWT Token (VD: Bearer eyJhbGciOi...)",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(_ => new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecuritySchemeReference("Bearer"),
            new List<string>()
        }
    });
});

// ================= DANG KY DEPENDENCY INJECTION (BUOI 2 & 3-TIER REPOSITORY) =================
builder.Services.AddScoped<QuanlyPhongtapGymFitnessClub.Repositories.IUserRepository, QuanlyPhongtapGymFitnessClub.Repositories.UserRepository>();
builder.Services.AddScoped<QuanlyPhongtapGymFitnessClub.Services.IUserService, QuanlyPhongtapGymFitnessClub.Services.UserService>();
builder.Services.AddScoped<QuanlyPhongtapGymFitnessClub.Services.ITrainerService, QuanlyPhongtapGymFitnessClub.Services.TrainerService>();
builder.Services.AddScoped<QuanlyPhongtapGymFitnessClub.Services.IStaffService, QuanlyPhongtapGymFitnessClub.Services.StaffService>();

// Cau hinh CORS
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

// ================= CAU HINH MIDDLEWARE PIPELINE =================
app.UseMiddleware<QuanlyPhongtapGymFitnessClub.Middleware.RequestLoggingMiddleware>();

// Luon bat Swagger (ke ca Production hay Development) de test do an va cham bai tien loi
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Gym Fitness Club API v1");
});

app.UseCors("AllowAll");
app.UseHttpsRedirection();

// Bat buoc khai bao Authentication (Kiem tra ve) TRUOC Authorization (Kiem tra quyen)
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Tu dong chuyen huong trang chu sang Swagger
app.MapGet("/", () => Results.Redirect("/swagger"));

// Hien thi link Swagger va huong dan ro rang trong Terminal khi server khoi dong
app.Lifetime.ApplicationStarted.Register(() =>
{
    Console.WriteLine();
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("==================================================================");
    Console.WriteLine("  GYM FITNESS CLUB MANAGEMENT API DA KHOI DONG THANH CONG!");
    Console.ForegroundColor = ConsoleColor.Cyan;
    var urls = app.Urls;
    if (urls.Any())
    {
        foreach (var url in urls)
        {
            Console.WriteLine($"  >> Truy cap Swagger UI tai: {url.TrimEnd('/')}/swagger");
        }
    }
    else
    {
        Console.WriteLine("  >> Truy cap Swagger UI tai: http://localhost:5000/swagger");
    }
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("  [Tai khoan Admin]:  admin      | Mat khau: Admin@123");
    Console.WriteLine("  [Tai khoan Member]: nguyenvana | Mat khau: password123");
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("==================================================================");
    Console.ResetColor();
    Console.WriteLine();
});

app.Run();






