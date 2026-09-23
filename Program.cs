var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

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
builder.Services.AddScoped<Buoi2_WebAPI.Services.ITrainerService, Buoi2_WebAPI.Services.TrainerService>();
builder.Services.AddScoped<Buoi2_WebAPI.Services.IStaffService, Buoi2_WebAPI.Services.StaffService>();

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

// ================= CẤU HÌNH MIDDLEWARE PIPELINE (BUỔI 2) =================
// 1. Request Logging Middleware (Bài tập Buổi 2: [LOG] Request: {Method} {Path})
app.UseMiddleware<Buoi2_WebAPI.Middleware.RequestLoggingMiddleware>();

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

