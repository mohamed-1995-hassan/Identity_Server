

using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder
    .Services
    .AddAuthentication(config =>
    {
        config.DefaultScheme = "Cookie";
        config.DefaultChallengeScheme = "oidc";
    })
    .AddCookie("Cookie")
    .AddOpenIdConnect("oidc", config =>
    {
        config.ClientId = "client_id_mvc";
        config.ClientSecret = "client_secret_mvc";
        config.SaveTokens = true;
        config.Authority = "https://localhost:7077/";
        config.ResponseType = "code";
        config.Scope.Clear();
        config.Scope.Add("openid");
        config.Scope.Add("rc.scope");
        config.Scope.Add("ApiOne");

        //configure cookie claims mapping
        config.ClaimActions.DeleteClaim("amr");
        config.ClaimActions.DeleteClaim("s_hash");
        config.ClaimActions.MapUniqueJsonKey("Geno", "gramma");
        //two trips to load claims into cookie
        //but id token is smaller

        config.GetClaimsFromUserInfoEndpoint = true;
    });
    
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();
app.UseCookiePolicy(new CookiePolicyOptions
{
    Secure = CookieSecurePolicy.Always
});
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
