using Bookworms_Online.Model;
using Microsoft.AspNetCore.Identity;
using Bookworms_Online.Services;
using GoogleReCaptcha.V3;
using GoogleReCaptcha.V3.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using System;
using Ganss.Xss;



var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<AuthDbContext>();

//Sanitaizer for XSS attacks
builder.Services.AddSingleton<HtmlSanitizer>();

// Add Identity configuration and chain the AddEntityFrameworkStores method.
builder.Services.AddIdentity<IdentityUserStaff, IdentityRole>(options =>
{
	// Lockout settings
	options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(10); // Lockout time
	options.Lockout.MaxFailedAccessAttempts = 3; // Max failed access attempts
	options.Lockout.AllowedForNewUsers = true;
})
.AddEntityFrameworkStores<AuthDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddScoped<UserManager<IdentityUserStaff>>();

builder.Services.ConfigureApplicationCookie(Config =>
{
	Config.LoginPath = "/Login";
});


//session management
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromSeconds(10); // Set session timeout
	options.Cookie.HttpOnly = true;
	options.Cookie.IsEssential = true;
	options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
	options.Cookie.HttpOnly = true;
});

builder.Services.AddSingleton<EncryptionService>();
builder.Services.AddHttpClient<ICaptchaValidator, GoogleReCaptchaValidator>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
	app.UseStatusCodePagesWithRedirects("/Error/{0}");
	app.UseHsts();

	app.Use(async (context, next) =>
	{
		var statusCode = context.Response.StatusCode;
		if (statusCode == 500)
		{
			context.Request.Path = "/Error500";
			await next();
		}
	});
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSession();

app.UseStatusCodePagesWithRedirects("/errors/{0}");
app.UseRouting();


app.UseAuthentication();

app.UseAuthorization(); app.Use(async (context, next) =>
{
    context.Response.Headers.Add("Content-Security-Policy",
                                 "default-src 'self'; " +
                                 "script-src 'self' 'unsafe-inline' 'unsafe-eval' https://www.google.com https://www.gstatic.com; " +
                                 "style-src 'self' 'unsafe-inline' https://fonts.googleapis.com; " +
                                 "font-src 'self' https://fonts.gstatic.com; " +
                                 "img-src 'self' data:; " +
                                 "frame-src 'self' https://www.google.com; " +
                                 "connect-src 'self' wss://localhost:44331 wss://localhost:44332 wss://localhost:44356 wss://localhost:44392 wss://localhost:44347");
    await next();
});


app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
    // Add other endpoints as needed
});

app.MapRazorPages();

app.Run();
