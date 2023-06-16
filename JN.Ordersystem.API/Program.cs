using JN.Ordersystem.BL;
using JN.Ordersystem.DAL;
using JN.Ordersystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("JN.Ordersystem")));

builder.Services.AddScoped<IService<OrderDetail>, OrderDetailService>();
builder.Services.AddScoped<IService<Order>, OrderService>();
builder.Services.AddTransient<AbstractProductService>();
builder.Services.AddTransient<AbstractCustomerService>();
builder.Services.AddTransient<AbstractSupplierService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
