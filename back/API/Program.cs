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


            builder.Services.AddControllers();
            builder.Services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("LocalDB"));
            });


            

            builder.Services.AddSwaggerGen();

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
