using MemberApp.Data;          // AppDbContext
using MemberApp.Interface;
using MemberApp.Services;     // IMemberService + MemberService
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Ū���]�w�ɡ]appsettings.json�^
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// 2. �b DI �e�����U DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 3. ���UService�� DI �e��
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/User/Login";
        options.LogoutPath = "/User/Logout";
    });
builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICouponService, CouponService>();
builder.Services.AddScoped<IConsumptionService, ConsumptionService>();


// 4. ���U AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

// 5. �`�U MVC�]Controllers + Views�^
builder.Services.AddControllersWithViews();

// 6. �إ����ε{��
var app = builder.Build();

// 7. HTTP Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// HTTPS & Static Files
app.UseHttpsRedirection();
app.UseStaticFiles();

// Routing & Authorization
app.UseRouting();

app.UseAuthorization();

// 8. ���I�]�w
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// 9. ����
app.Run();
