using Microsoft.AspNetCore.DataProtection;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDataProtection();
builder.Services.AddHttpContextAccessor();


var app = builder.Build();

app.Use((ctx, next) =>
{
    var idp = ctx.RequestServices.GetRequiredService<IDataProtectionProvider>();
    var protector = idp.CreateProtector("auth");
    var cookie = ctx.Request.Headers.Cookie.FirstOrDefault(x => x.StartsWith("username"));

    if (cookie is null) return next();

    var protectedPayload = cookie.Split("=").Last();
    var username = protector.Unprotect(protectedPayload);

    List<Claim> claims = new();
    claims.Add(new Claim(ClaimTypes.Name, username));
    var identity = new ClaimsIdentity(claims, "Cookie");

    ctx.User = new ClaimsPrincipal(identity);

    return next();
});

app.MapGet("/username", (IHttpContextAccessor ctx, IDataProtectionProvider idp) =>
{

    var username = ctx.HttpContext.User.Identity.Name;

    return username;
});

app.MapGet("/login", (HttpContext ctx, IDataProtectionProvider idp) =>
{
    var protectedCookie = idp.CreateProtector("auth").Protect("mustafa");
    ctx.Response.Headers["Set-Cookie"] = $"username={protectedCookie}";
    return "OK";
});

app.Run();
