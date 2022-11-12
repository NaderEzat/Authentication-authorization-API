using Authentication.DATA;
using Authentication.DATA.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
string allowAllPolicy = "AllowAll";


// Add services to the container.
#region Defultservices
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#endregion

#region Allo Cors

builder.Services.AddCors(options =>
{
    options.AddPolicy(allowAllPolicy, policy =>
    {
        policy.AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyOrigin();
    });
});
#endregion

#region connection Database
var ConnectionString = builder.Configuration.GetConnectionString("co1");
builder.Services.AddDbContext<SchoolUserContext>(optins => optins.UseSqlServer(ConnectionString));
#endregion

#region Manager Configuration
builder.Services.AddIdentity<USERS, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Lockout.MaxFailedAccessAttempts = 2;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
}).AddEntityFrameworkStores<SchoolUserContext>();
#endregion

#region Authentication

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "Default";
    options.DefaultChallengeScheme = "Default";
})
    .AddJwtBearer("Default", options =>
    {
        var keyString = builder.Configuration.GetValue<string>("SecretKey");
        var keyInBytes = Encoding.ASCII.GetBytes(keyString);
        var key = new SymmetricSecurityKey(keyInBytes);
        options.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = key,
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

#endregion

#region Authorization && Policy
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Student", p => p.RequireClaim(claimType: ClaimTypes.Role, "Student"));
    options.AddPolicy("Teacher", p => p.RequireClaim(claimType: ClaimTypes.Role, "Teacher"));


});
#endregion

var app = builder.Build();

#region Middeleware

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(allowAllPolicy);
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

#endregion
