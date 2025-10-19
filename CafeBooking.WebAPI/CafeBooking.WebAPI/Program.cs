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

// ���������� ������������
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ��������� ����������� � ��
builder.Services.AddDbContext<CafeDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// ����������� ������������
builder.Services.AddScoped<ITableRepository, TableRepository>();
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();

// ����������� ������-��������
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddScoped<ITableService, TableService>();

// ��������� AutoMapper
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<ReservationProfile>();
});

// ����������� �����������
builder.Services.AddValidatorsFromAssemblyContaining<ReservationCreateDtoValidator>();

// ��������� �������������� ���������
builder.Services.AddFluentValidationAutoValidation();

// ��������� ���������� ��������� (���� �����)
builder.Services.AddFluentValidationClientsideAdapters();

// ����������� �������� �������
builder.Services.AddHostedService<ExpiredReservationsCleanupService>();

// �������������� ���������
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

// ��������� middleware
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

// ������������� �� � ���������� �������� ������
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<CafeDbContext>();
    context.Database.Migrate();

    // �������� � ���������� �������� ������
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

        Console.WriteLine("��������� 10 �������� ������ � ���� ������");
    }
}

app.Run();
