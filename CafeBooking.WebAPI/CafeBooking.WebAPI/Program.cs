using Microsoft.EntityFrameworkCore;
using CafeBooking.DataAccess.Context;
using CafeBooking.DataAccess.Entities;
using CafeBooking.DataAccess.Repositories;
using CafeBooking.BusinessLogic.Mappings;
using CafeBooking.BusinessLogic.Services;
using CafeBooking.BusinessLogic.Validation;
using CafeBooking.WebAPI.Services;
using CafeBooking.WebAPI.Middleware;
using FluentValidation.AspNetCore;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

// Добавление конфигурации
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Настройка подключения к БД
builder.Services.AddDbContext<CafeDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Регистрация репозиториев
builder.Services.AddScoped<ITableRepository, TableRepository>();
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();

// Регистрация бизнес-сервисов
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddScoped<ITableService, TableService>();

// Настройка AutoMapper
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<ReservationProfile>();
});

// Регистрация валидаторов
builder.Services.AddValidatorsFromAssemblyContaining<ReservationCreateDtoValidator>();

// Включение автоматической валидации
builder.Services.AddFluentValidationAutoValidation();

// Включение клиентских адаптеров (если нужно)
builder.Services.AddFluentValidationClientsideAdapters();

// Регистрация фонового сервиса
builder.Services.AddHostedService<ExpiredReservationsCleanupService>();

// Дополнительные настройки
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Настройка middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Инициализация БД и добавление тестовых данных
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<CafeDbContext>();
    context.Database.Migrate();

    // Проверка и добавление тестовых данных
    if (!context.Tables.Any())
    {
        var tables = new List<Table>();
        for (int i = 1; i <= 10; i++)
        {
            tables.Add(new Table
            {
                Number = i,
                Capacity = (i % 3) + 2 // 3, 4, 2, 3, 4, 2...
            });
        }
        context.Tables.AddRange(tables);
        context.SaveChanges();

        Console.WriteLine("Добавлено 10 тестовых столов в базу данных");
    }
}

app.Run();
