using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Papri.Data;
using Papri.Services;

var builder = WebApplication.CreateBuilder(args);

var conn = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(opt =>
    opt.UseMySql(conn, ServerVersion.AutoDetect(conn)));

builder.Services
    .AddDefaultIdentity<IdentityUser>(o =>
    {
        o.SignIn.RequireConfirmedAccount = false;
        o.Password.RequiredLength = 8;
        o.Lockout.MaxFailedAccessAttempts = 5;
        o.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
    })
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.ConfigureApplicationCookie(o =>
{
    o.LoginPath = "/admin";
    o.LogoutPath = "/admin/logout";
    o.AccessDeniedPath = "/admin";
    o.ExpireTimeSpan = TimeSpan.FromHours(8);
    o.SlidingExpiration = true;
});

builder.Services.AddRazorPages(o =>
{
    o.Conventions.AuthorizeFolder("/Admin");
    o.Conventions.AllowAnonymousToPage("/Admin/Login");
    o.Conventions.AuthorizeAreaFolder("Identity", "/Account");
});

builder.Services.Configure<FormOptions>(o =>
{
    o.MultipartBodyLengthLimit = AllowedFiles.DocumentMaxBytes;
});

builder.Services.AddScoped<FileUploadService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
    var seedEmail = builder.Configuration["AdminSeed:Email"];
    var seedPwd = builder.Configuration["AdminSeed:Password"];

    if (!string.IsNullOrEmpty(seedEmail) && !string.IsNullOrEmpty(seedPwd)
        && await userMgr.FindByEmailAsync(seedEmail) is null)
    {
        var admin = new IdentityUser { UserName = seedEmail, Email = seedEmail, EmailConfirmed = true };
        var result = await userMgr.CreateAsync(admin, seedPwd);
        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(e => e.Description));
            app.Logger.LogError("Admin seed failed: {Errors}", errors);
        }
        else
        {
            app.Logger.LogInformation("Seeded admin user {Email}", seedEmail);
        }
    }

}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.Run();
