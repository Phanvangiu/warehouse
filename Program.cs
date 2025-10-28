using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using warehouse.Data;
using warehouse.Interfaces;
using warehouse.Services;
using warehouse.Setting;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddOpenApi();

// Add services to the container.

builder.Services.AddDbContext<DataContext>(option =>
{
  option.UseSqlServer(builder.Configuration.GetConnectionString("MyConnection"));
});
builder.Services.Configure<MailSetting>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddTransient<IMailService, MyMailService>();
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddControllers().AddJsonOptions(x =>
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles
);
var allowOrigin = builder.Configuration.GetSection("AllowOrigin").Get<string[]>();

builder.Services.AddCors(options =>
{
  options.AddPolicy("myAppCors", policy =>
  {
    policy.WithOrigins(allowOrigin).AllowAnyHeader().AllowAnyMethod().AllowCredentials();
  });
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
  options.RequireHttpsMetadata = false;
  options.SaveToken = true;

  options.TokenValidationParameters = new TokenValidationParameters
  {
    ValidateIssuer = true,
    ValidateAudience = true,
    ValidateLifetime = true,
    ValidateIssuerSigningKey = true,
    ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
    ValidAudience = builder.Configuration["JwtSettings:Audience"],
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
          builder.Configuration["JwtSettings:Key"]!
          )),
    ClockSkew = TimeSpan.Zero
  };
});
builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSwaggerGen(c =>
{
  c.SwaggerDoc("v1", new OpenApiInfo
  {
    Title = "Warehouse API",
    Version = "v1"
  });

  // 🔑 Cấu hình Swagger tự thêm "Bearer "
  c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
  {
    Description = "Dán token trực tiếp vào đây (không cần thêm chữ 'Bearer')",
    Name = "Authorization",
    In = ParameterLocation.Header,
    Type = SecuritySchemeType.Http,
    Scheme = "bearer",
    BearerFormat = "JWT"
  });

  c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.MapOpenApi();
  app.UseSwagger();
  app.UseSwaggerUI();
}
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseCors("myAppCors");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

