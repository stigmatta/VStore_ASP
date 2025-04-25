using Business_Logic.Mappers.UserProfile;
using Business_Logic.Services;
using Data_Access.Context;
using Data_Access.Interfaces;
using Data_Access.Models;
using Data_Access.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// ��������� �������
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization();
builder.Services.AddControllers();

// ��������� ��
string? connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<StoreContext>(options =>
    options.UseSqlServer(connection, b => b.MigrationsAssembly("Data-Access")));

// ��������� CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReact",
        policy => policy.WithOrigins("http://localhost:3000")
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});
builder.Services.AddAutoMapper(typeof(RegistrationProfile).Assembly);

// ����������� ������������
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IListRepository<Game>, GameRepository>();
builder.Services.AddScoped<IListRepository<RecommendedRequirement>, RecommendedRequirementRepository>();
builder.Services.AddScoped<IListRepository<MinimumRequirement>, MinimumRequirementRepository>();
builder.Services.AddScoped<IRepository<GameGallery>, GameGalleryRepository>();

//�������
builder.Services.AddTransient<UserService>();

var app = builder.Build();

// ������������ middleware
app.UseCors("AllowReact");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// �����: StaticFiles ������ ���� ����� UseAuthorization � MapControllers
app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.WebRootPath, "uploads")),
    RequestPath = "/uploads"
});

app.UseAuthorization();
app.MapControllers();

// ������� ����� uploads ��� �������
var uploadsPath = Path.Combine(builder.Environment.WebRootPath, "uploads");
if (!Directory.Exists(uploadsPath))
{
    Directory.CreateDirectory(uploadsPath);
}

app.Run();