using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using News_Platform.Data;
using News_Platform.Utilities;
using News_Platform.Repositories.Interfaces;
using News_Platform.Repositories.Implementations;
using News_Platform.Services.Implementations;
using News_Platform.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Amazon.Runtime;
using Amazon;
using Amazon.S3;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
builder.Services.AddScoped<IArticleService, ArticleService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<ILikeRepository, LikeRepository>();
builder.Services.AddScoped<ILikeService, LikeService>();
builder.Services.AddScoped<IUserTokenRepository, UserTokenRepository>();
builder.Services.AddScoped<IUserTokenService, UserTokenService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IEmailTemplateRepository, EmailTemplateRepository>();
builder.Services.AddScoped<IArticleViewRepository, ArticleViewRepository>();
builder.Services.AddScoped<IArticleViewService, ArticleViewService>();
builder.Services.AddScoped<IForgotPasswordService, ForgotPasswordService>();




builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));


var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key),
            RoleClaimType = "role",
            NameClaimType = ClaimTypes.NameIdentifier
        };
    });


builder.Services.AddAuthorization();
builder.Services.AddSingleton<JWTUtility>(provider =>
{
    var config = provider.GetRequiredService<IConfiguration>();
    return new JWTUtility(config);
});

builder.Services.AddSingleton<IS3Service, S3Service>();

var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(allowedOrigins ?? Array.Empty<string>())
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});



var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
