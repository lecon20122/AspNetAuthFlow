using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddDataProtection();
//builder.Services.AddHttpContextAccessor();
//builder.Services.AddScoped<AuthService>();

builder.Services.AddAuthentication()
    .AddCookie("cookie");


var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthentication();

//app.Use((ctx, next) =>
//{
//    var idp = ctx.RequestServices.GetRequiredService<IDataProtectionProvider>();
//    var protector = idp.CreateProtector("auth");
//    var cookie = ctx.Request.Headers.Cookie.FirstOrDefault(x => x.StartsWith("username"));

//    if (cookie is null) return next();

//    var protectedPayload = cookie.Split("=").Last();
//    var username = protector.Unprotect(protectedPayload);

//    List<Claim> claims = new();
//    claims.Add(new Claim(ClaimTypes.Name, username));
//    var identity = new ClaimsIdentity(claims, "Cookie");

//    ctx.User = new ClaimsPrincipal(identity);

//    return next();
//});

app.MapGet("/username", (HttpContext ctx) =>
{
    var username = ctx.User.FindFirst(ClaimTypes.Name).Value;

    return username;
});

app.MapGet("/login", async (HttpContext ctx) =>
{
    //auth.SignIn();

    var claims = new List<Claim>();
    claims.Add(new Claim(ClaimTypes.Name, "Mustafa"));
    var identity = new ClaimsIdentity(claims, "Cookie");

    ctx.User = new ClaimsPrincipal(identity);

    await ctx.SignInAsync("cookie", ctx.User);

    return "OK";
});

app.Run();
