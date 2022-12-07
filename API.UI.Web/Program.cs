using API.Core.Helpers;
using API.Core.HTTPClients;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Primitives;

var builder = WebApplication.CreateBuilder(args);

// HttpClient'lar için ApiUrl gerekiyor
ConstantHelper.ApiUrl = builder.Configuration["ApiUrl"];

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<AuthorizationHandler>();


builder.Services.AddHttpClient<LoginClient>().AddHttpMessageHandler<AuthorizationHandler>();
builder.Services.AddHttpClient<CategoryClient>().AddHttpMessageHandler<AuthorizationHandler>();
builder.Services.AddHttpClient<CompanyClient>().AddHttpMessageHandler<AuthorizationHandler>();
builder.Services.AddHttpClient<ProductClient>().AddHttpMessageHandler<AuthorizationHandler>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = new PathString("/Account/Login");
    });

// Add services to the container.
builder.Services.AddControllersWithViews();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
