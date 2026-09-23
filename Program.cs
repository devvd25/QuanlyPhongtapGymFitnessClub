using Microsoft.EntityFrameworkCore;
using QuanlyPhongtapGymFitnessClub.Data;

var builder = WebApplication.CreateBuilder(args);

// ================= 1. Cáº¤U HĂŒNH DATABASE & DEPENDENCY INJECTION =================
// 1. Äá»c chuá»—i káº¿t ná»‘i tá»« appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// 2. ÄÄƒng kĂ½ AppDbContext vĂ o há»‡ thá»‘ng Dependency Injection
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// ================= 2. ÄÄ‚NG KĂ CĂC Dá»CH Vá»¤ KHĂC =================

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

// ================= ÄÄ‚NG KĂ DEPENDENCY INJECTION (BUá»”I 2) =================
builder.Services.AddScoped<QuanlyPhongtapGymFitnessClub.Services.ITrainerService, QuanlyPhongtapGymFitnessClub.Services.TrainerService>();
builder.Services.AddScoped<QuanlyPhongtapGymFitnessClub.Services.IStaffService, QuanlyPhongtapGymFitnessClub.Services.StaffService>();

// Cáº¥u hĂ¬nh CORS
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

// ================= Cáº¤U HĂŒNH MIDDLEWARE PIPELINE (BUá»”I 2) =================
// 1. Request Logging Middleware (BĂ i táº­p Buá»•i 2: [LOG] Request: {Method} {Path})
app.UseMiddleware<QuanlyPhongtapGymFitnessClub.Middleware.RequestLoggingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

