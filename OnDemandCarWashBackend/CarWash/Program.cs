using CarWash.Data;
using CarWash.Interface;
using CarWash.Mapping;
using CarWash.Repositories;
using CarWashBookingSystem.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<CarWashContext>(options=>options.UseSqlServer(builder.Configuration.GetConnectionString("DBConnection")));
builder.Services.AddDbContext<CarWashAuthDbContext>(options=>options.UseSqlServer(builder.Configuration.GetConnectionString("DBConnectionAuthConnectionstring")));

builder.Services.AddScoped<IUser,UserRepository>();
builder.Services.AddScoped<ICar,CarRepository>();
builder.Services.AddScoped<IPackage,PackageRepository>();
builder.Services.AddScoped<IBooking,BookingRepository>();
builder.Services.AddScoped<IAddress,AddressRepository>();
builder.Services.AddScoped<IPayment,PaymentRepository>();
builder.Services.AddScoped<ITokenService,TokenService>();
builder.Services.AddScoped<IReview, ReviewRepository>();
builder.Services.AddScoped<IEmailService,EmailService>();

builder.Services.AddAutoMapper(typeof(AutoMaperProfile));

builder.Services.AddIdentityCore<IdentityUser>()
.AddRoles<IdentityRole>().AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("CarWash")
.AddEntityFrameworkStores<CarWashAuthDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});



builder.Services.Configure<IdentityOptions>(options=>{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars=1;

});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
       .AddJwtBearer(options =>
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
            }
       );
 
builder.Services.AddAuthorization();
builder.Services.AddSwaggerGen(c =>
{
 
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "CarWash", Version = "v1" });
 
 
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "JWT Authorization header using the Bearer scheme",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "bearer",
        BearerFormat = "JWT"
    };
 
 
    c.AddSecurityDefinition("Bearer", securityScheme);
 
 
    var securityRequirement = new OpenApiSecurityRequirement
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
            new List<string>()
        }
    };
 
 
    c.AddSecurityRequirement(securityRequirement);
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

app.UseStaticFiles(new StaticFileOptions
{
    // FileProvider = new PhysicalFileProvider(
    //     Path.Combine(Directory.GetCurrentDirectory(), "UploadedImages")),
    FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "UploadedImages")),
    RequestPath = "/UploadedImages"
});
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseCors("AllowAll");
app.UseAuthorization();

app.MapControllers();
app.Run();

