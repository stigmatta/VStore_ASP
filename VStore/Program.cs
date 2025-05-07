using Business_Logic.Mappers;
using Business_Logic.Mappers.UserProfile;
using Business_Logic.Services;
using Data_Access.Context;
using Data_Access.Interfaces;
using Data_Access.Models;
using Data_Access.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Логирование пути к wwwroot для проверки
Console.WriteLine($"WebRootPath: {builder.Environment.WebRootPath}");

// Добавление сервисов
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Настройка аутентификации
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Events.OnRedirectToAccessDenied = context =>
        {
            context.Response.StatusCode = 403;
            return Task.CompletedTask;
        };
        options.Events.OnRedirectToLogin = context =>
        {
            context.Response.StatusCode = 401;
            return Task.CompletedTask;
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CookieAdminPolicy", policy =>
    {
        policy.RequireAssertion(context =>
        {
            var httpContext = context.Resource as HttpContext;
            return httpContext?.Request.Cookies["isAdmin"] == "True";
        });
    });
    options.AddPolicy("CookieUserPolicy", policy =>
    {
        policy.RequireAssertion(context =>
        {
            var httpContext = context.Resource as HttpContext;
            return !string.IsNullOrEmpty(httpContext?.Request.Cookies["userId"]);
        });
    });

});

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();

// Настройка базы данных
string? connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<StoreContext>(options =>
    options.UseSqlServer(connection, b => b.MigrationsAssembly("Data-Access")));

// Настройка CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReact",
        policy => policy.WithOrigins("http://localhost:3000")
                        .AllowCredentials()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

// Настройка AutoMapper
builder.Services.AddAutoMapper(typeof(GameProfile).Assembly);

// Регистрация сервисов и репозиториев
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<UserService>();
builder.Services.AddTransient<GameService>();
builder.Services.AddTransient<WishlistService>();
builder.Services.AddTransient<GameGalleryService>();
builder.Services.AddTransient<NewsService>();
builder.Services.AddScoped<IListRepository<MinimumRequirement>, MinimumRequirementRepository>();
builder.Services.AddScoped<IListRepository<RecommendedRequirement>, RecommendedRequirementRepository>();
builder.Services.AddScoped<IRequirementsService, RequirementsService>();

var app = builder.Build();

// Создание необходимых директорий для загрузок
var uploadsPath = Path.Combine(builder.Environment.WebRootPath, "uploads");
var logosPath = Path.Combine(uploadsPath, "logos");

if (!Directory.Exists(uploadsPath)) Directory.CreateDirectory(uploadsPath);
if (!Directory.Exists(logosPath)) Directory.CreateDirectory(logosPath);

// Настройка middleware
app.UseCors("AllowReact");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Настройка статических файлов
var staticFileOptions = new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.WebRootPath, "uploads")),
    RequestPath = "/uploads",
    ContentTypeProvider = new FileExtensionContentTypeProvider(),
    OnPrepareResponse = ctx =>
    {
        ctx.Context.Response.Headers.Append("Access-Control-Allow-Origin", "http://localhost:3000");
        ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=604800");
    }
};

app.UseStaticFiles();
app.UseStaticFiles(staticFileOptions);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Middleware для логгирования запросов
app.Use(async (context, next) =>
{
    Console.WriteLine($"Incoming request: {context.Request.Path}");
    await next();
    Console.WriteLine($"Response status: {context.Response.StatusCode}");
});

app.Run();