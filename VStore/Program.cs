using Business_Logic.Mappers.UserProfile;
using Business_Logic.Services;
using Data_Access.Context;
using Data_Access.Interfaces;
using Data_Access.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<UserService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
string? connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<StoreContext>(options =>
    options.UseSqlServer(connection, b => b.MigrationsAssembly("Data-Access")));
builder.Services.AddCors(builder =>
{
    builder.AddPolicy("AllowReact",
        options => options.WithOrigins("http://localhost:3000")
            .AllowAnyMethod()
            .AllowAnyHeader());
});
builder.Services.AddAutoMapper(typeof(RegistrationProfile).Assembly);

var app = builder.Build();
app.UseCors("AllowReact");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
