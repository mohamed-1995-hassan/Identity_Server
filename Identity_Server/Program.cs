using DemoIDP;
using Identity_Server;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MvcClient;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
var cs = builder.Configuration.GetConnectionString("MainConnection");
string migrationAssembly = Assembly.GetExecutingAssembly().GetName().Name;

builder
    .Services
    .AddDbContext<ApplicationContext>(config =>
    {
        config.UseSqlServer(cs);
    });

builder
    .Services
    .AddIdentity<IdentityUser, IdentityRole>(config =>
    {
        config.SignIn.RequireConfirmedAccount = false;
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
    .AddConfigurationStore(option =>
    {
        option.ConfigureDbContext = builder => builder.UseSqlServer(cs, sqlServerOptionsAction: sql => sql.MigrationsAssembly(migrationAssembly));
    })
    .AddOperationalStore(option =>
    {
        option.ConfigureDbContext = builder => builder.UseSqlServer(cs, sqlServerOptionsAction: sql => sql.MigrationsAssembly(migrationAssembly));
    })
    //.AddInMemoryApiResources(Configuration.GetApis())
    //.AddInMemoryIdentityResources(Configuration.GetIdentityResource())
    //.AddInMemoryClients(Configuration.GetClients())
    .AddDeveloperSigningCredential()
    .AddProfileService<IdentityProfileService>();

if (builder.Configuration.GetValue<bool>("SeedData"))
{
    SeedData.EnsureSeedData(cs);
}

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    //app.UseHsts();
}

app.UseHttpsRedirection();
app.UseCors("AllowAllOrigins");

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
