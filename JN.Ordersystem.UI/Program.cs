using JN.Ordersystem.BL;
using JN.Ordersystem.DAL;
using JN.Ordersystem.DAL.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddTransient<DataContext>();
builder.Services.AddTransient<CustomerService>();
builder.Services.AddTransient<OrderDetailService>();
builder.Services.AddTransient<OrderService>();
builder.Services.AddTransient<AbstractProductService>();
builder.Services.AddTransient<AbstractCustomerService>();
builder.Services.AddTransient<SupplierService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
