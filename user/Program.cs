using App.Data;
using App.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("SQLite");
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSqlite<AppDb>(connectionString);
builder.Services.AddIdentity<UserProfile, IdentityRole>()
              .AddEntityFrameworkStores<AppDb>()
              .AddDefaultTokenProviders();

var issuer = builder.Configuration["Jwt:Issuer"];
var key = builder.Configuration["Jwt:Key"];
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
{
    opt.RequireHttpsMetadata = false;//false for dev
    opt.Authority = issuer;
    opt.TokenValidationParameters = new()
    {
        ValidateIssuer = true,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = issuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
    };
});
builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
          {
              new OpenApiSecurityScheme
              {
                  Reference = new OpenApiReference
                  {
                      Type=ReferenceType.SecurityScheme,
                      Id="Bearer"
                  }
              },
              new string[]{}
          }
    });
});

builder.Services.AddHealthChecks();
var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
app.UseDefaultFiles();
app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapHealthChecks("/health");
app.MapGet("/hello", () => "Hello");
app.MapGet("/auth", (HttpContext context) =>
{
    var id = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    var n = context.User.FindFirst(ClaimTypes.Name)?.Value;
    var g = context.User.FindFirst(ClaimTypes.GivenName)?.Value;
    var sn = context.User.FindFirst(ClaimTypes.Surname)?.Value;
    var p = context.User.FindFirst("preferred_username")?.Value;
    return $"{id}:{n}:{sn}:{p}";
}).RequireAuthorization();

app.MapGet("/admin", [Authorize(Roles = "admin")] () => "Admin ");
app.MapGet("/admin_dev", [Authorize(Roles = "admin,dev")] () => "Dev or Admin");
app.MapGet("/dev", [Authorize(Roles = "dev")] () => "Dev ");
app.Run();

