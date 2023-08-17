using Identity_Server;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MvcClient;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services
    .AddDbContext<ApplicationContext>(config =>
    {
        config.UseSqlServer("server=.;database=IdentityServer;Trusted_Connection=SSPI;Encrypt=false;TrustServerCertificate=true");
    });

builder
    .Services
    .AddIdentity<IdentityUser, IdentityRole>(config =>
    {
        config.Password.RequiredLength = 4;
        config.Password.RequireDigit = false;
        config.Password.RequireNonAlphanumeric = false;
        config.Password.RequireLowercase = false;
    })
    .AddEntityFrameworkStores<ApplicationContext>()
    .AddDefaultTokenProviders();

builder
    .Services
    .ConfigureApplicationCookie(config =>
    {
        config.Cookie.Name = "Identity.Cookie";
        config.LoginPath = "/account/login";
    });


// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddIdentityServer()
                
                .AddAspNetIdentity<IdentityUser>()
                .AddInMemoryApiResources(Configuration.GetApis())
                .AddInMemoryIdentityResources(Configuration.GetIdentityResource())
                .AddInMemoryClients(Configuration.GetClients())
                .AddDeveloperSigningCredential();
                


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
app.UseIdentityServer();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


//using (var service = app.Services.CreateScope())
//{
//    var userManager = service
//        .ServiceProvider
//        .GetRequiredService<UserManager<IdentityUser>>();

//    var user = new IdentityUser("mohamed");
//    userManager
//        .CreateAsync(user, "password")
//        .GetAwaiter()
//        .GetResult();
//}

app.Run();
