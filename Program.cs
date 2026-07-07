using ASM.Helpers;
using ASM.Models;
using ASM.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString;
    });

builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("defaultConnection")));

builder.Services.AddTransient<IGiaySvc, GiaySvc>();
builder.Services.AddTransient<INguoidungSvc, NguoidungSvc>();
builder.Services.AddTransient<IKhachhangSvc, KhachhangSvc>();
builder.Services.AddTransient<IDonHangSvc, DonHangSvc>();
builder.Services.AddTransient<IDonhangChitietSvc, DonhangChitietSvc>();
builder.Services.AddTransient<ICartSvc, CartSvc>();
builder.Services.AddTransient<IMahoaHelper, MahoaHelper>();
builder.Services.AddTransient<IUploadHelper, UploadHelper>();

builder.Services.AddSession(option => { 
    option.IdleTimeout = TimeSpan.FromHours(24); // Lýu 24 gi?
    option.Cookie.HttpOnly = true;
    option.Cookie.IsEssential = true;
});

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

app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
