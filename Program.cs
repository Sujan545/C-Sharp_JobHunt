using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MyRazorApp.Data;
using MyRazorApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Razor + API
builder.Services.AddRazorPages();
builder.Services.AddControllers();

// DB
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var path = Path.Combine(Directory.GetCurrentDirectory(), "app.db");
    options.UseSqlite($"Data Source={path}");
});

// JWT Service
builder.Services.AddScoped<TokenService>();

// JWT for API only
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new()
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["JwtKey"]!)
        )
    };
});

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();
app.MapRazorPages();

app.Run();
