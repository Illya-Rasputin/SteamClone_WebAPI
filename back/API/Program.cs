using BLL.Mapping;
using BLL.Services;
using BLL.Settings;
using DAL;
using DAL.Entities;
using DAL.Initializer;
using DAL.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddScoped<GenreRepository>();
            builder.Services.AddScoped<DeveloperRepository>();
            builder.Services.AddScoped<GameRepository>();
            builder.Services.AddScoped<GameImageRepository>();

            builder.Services.AddScoped<GenreService>();
            builder.Services.AddScoped<DeveloperService>();
            builder.Services.AddScoped<GameService>();
            builder.Services.AddScoped<FileService>();
            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<JwtService>();
            builder.Services.AddScoped<EmailService>();


            builder.Services.AddControllers();
            builder.Services.AddAutoMapper(cfg =>
            {
                cfg.LicenseKey = 
                "eyJhbGciOiJSUzI1NiIsImtpZCI6Ikx1Y2t5UGVubnlTb2Z0d2FyZUxpY2Vuc2VLZXkvYmJiMTNhY2I1OTkwNGQ4OWI0Y2IxYzg1ZjA4OGNjZjkiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL2x1Y2t5cGVubnlzb2Z0d2FyZS5jb20iLCJhdWQiOiJMdWNreVBlbm55U29mdHdhcmUiLCJleHAiOiIxODI5MDg4MDAwIiwiaWF0IjoiMTc5NTAxNDQ0NiIsImFjY291bnRfaWQiOiIwMWExZWYzMWE1YmY3Y2FjYjg2N2NkMTExYzUwYmZmOCIsImN1c3RvbWVyX2lkIjoiMDFhMWVmMzFhNWJmN2NhY2I4NjdjZDExMWM1MGJmZjgiLCJzdWJfaWQiOiItIiwiZWRpdGlvbiI6IjAiLCJ0eXBlIjoiMiIsInRlcm1fZW5kIjoiMTgyNjQ5NjAwMCJ9.fvLAHdkwWQB0DNEPAac9ngNa00wtPGz28ARdkaONkIvmAt_zVR9HnVCryFc-k7l2a2I-KALae3_0H8Lr7qaj_4MBuFVtahg4BV21apZvfpTE-YBnoEdmZxtAMPbo30QE-e7SyL3O6XHvncIuAA9F0yHYMCCTPvNapoF2wRVsFL-mdZGq9PCMepXinHyLCEUuuPPj9TX0DZ6PHq2x0O_y4yrfrLLNwgUIznd8BrpHlGQ9hIUj5CvGyhORArNFM8w75YrCRgqOpdPxADU_f1mzA0WM432zejWfhOKzlbK5YIBVbnZwi19ru97-4iK3Ibf8p2ZhbsYaO0ykVAad7IpWMQ";
            }, AppDomain.CurrentDomain.GetAssemblies());
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("LocalDB"));
            });

            builder.Services.AddIdentity<UserEntity, RoleEntity>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;

                options.Password.RequiredLength = 6;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;

            })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

            builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
            builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));


            string? secretKey = builder.Configuration["JwtSettings:SecretKey"];

            if (string.IsNullOrEmpty(secretKey))
            {
                throw new ArgumentNullException("Jwt secret key is null");
            }

            byte[] secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
            string issuer = builder.Configuration["JwtSettings:Issuer"] ?? string.Empty;
            string audience = builder.Configuration["JwtSettings:Audience"] ?? string.Empty;

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    RequireExpirationTime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidateLifetime = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes)
                };
            });

            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "SPR411", Version = "v1" });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                        Array.Empty<string>()
                    }
                });
            });

            string corsPolicy = "allowAll";
            builder.Services.AddCors(cfg =>
            {
                cfg.AddPolicy(corsPolicy, cfg =>
                {
                    cfg.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                });
            });

            var app = builder.Build();


            
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors("allowAll");

            var root = builder.Environment.ContentRootPath;
            var storagePath = Path.Combine(root, StaticFilesSettings.Storage);
            var sharePath = Path.Combine(storagePath, StaticFilesSettings.Share);
            var developersPath = Path.Combine(storagePath, StaticFilesSettings.Developers);
            var gamesPath = Path.Combine(storagePath, StaticFilesSettings.Games);

            if (!Directory.Exists(storagePath))
            {
                Directory.CreateDirectory(storagePath);
            }

            if (!Directory.Exists(sharePath))
            {
                Directory.CreateDirectory(sharePath);
            }

            if (!Directory.Exists(developersPath))
            {
                Directory.CreateDirectory(developersPath);
            }

            if (!Directory.Exists(gamesPath))
            {
                Directory.CreateDirectory(gamesPath);
            }

            // Share
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(sharePath),
                RequestPath = "/share"
            });

            // Developers
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(developersPath),
                RequestPath = "/developer"
            });

            // Games
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(gamesPath),
                RequestPath = "/game"
            });

            app.UseAuthentication();
            app.UseAuthorization();

            await app.SeedAsync();
                
            app.MapControllers();

            app.Run();
        }
    }
}
