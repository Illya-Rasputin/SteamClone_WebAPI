using BLL.Mapping;
using DAL;
using DAL.Initializer;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using BLL.Services; 
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


            

            builder.Services.AddSwaggerGen();

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

            app.UseAuthorization();

            await app.SeedAsync();
                
            app.MapControllers();

            app.Run();
        }
    }
}
