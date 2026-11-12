using DAL.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Initializer
{
    public static class Seeder
    {
        public static async Task SeedAsync(this IApplicationBuilder app)
        {
        using var scope = app.ApplicationServices.CreateScope();
        using var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        await dbContext.Database.MigrateAsync();

        // Genres
        var genres = new List<GenreEntity>();
        if (!dbContext.Genres.Any())
        {
            genres = new List<GenreEntity>
                {
                    new GenreEntity {Name = "Спорт"},
                    new GenreEntity {Name = "Стратегії"},
                    new GenreEntity {Name = "Жахи"},
                    new GenreEntity {Name = "Казуальні"},
                    new GenreEntity {Name = "Перегони"},
                    new GenreEntity {Name = "Симулятори"},
                    new GenreEntity {Name = "Виживання"},
                    new GenreEntity {Name = "Космос"},
                    new GenreEntity {Name = "Аніме"},
                    new GenreEntity {Name = "Шутери"}
                };

            await dbContext.Genres.AddRangeAsync(genres);
            await dbContext.SaveChangesAsync();
        }
        else 
        {
            genres = await dbContext.Genres
                .AsNoTracking()
                .ToListAsync();
        }
        if (!dbContext.Developers.Any())
        {
            var developers = new List<DeveloperEntity>
            {
                new DeveloperEntity
                {
                    Name = "Rockstar Games",
                    Games = new List<GameEntity>
                    {
                        new GameEntity { Name = "Grand Theft Auto V", Price = 599, Release = new DateTime(2013, 9, 17), Genres = new List<GenreEntity> { genres[9], genres[4] } },
                        new GameEntity { Name = "Red Dead Redemption 2", Price = 899, Release = new DateTime(2018, 10, 26), Genres = new List<GenreEntity> { genres[9], genres[5] } },
                        new GameEntity { Name = "Max Payne 3", Price = 299, Release = new DateTime(2012, 5, 15), Genres = new List<GenreEntity> { genres[9] } },
                        new GameEntity { Name = "Bully", Price = 149, Release = new DateTime(2006, 10, 17), Genres = new List<GenreEntity> { genres[3] } },
                        new GameEntity { Name = "L.A. Noire", Price = 399, Release = new DateTime(2011, 5, 17), Genres = new List<GenreEntity> { genres[5] } }
                    }
                },

                new DeveloperEntity
                {
                    Name = "Electronic Arts",
                    Games = new List<GameEntity>
                    {
                        new GameEntity { Name = "EA Sports FC 24", Price = 699, Release = new DateTime(2023, 9, 29), Genres = new List<GenreEntity> { genres[0] } },
                        new GameEntity { Name = "The Sims 4", Price = 399, Release = new DateTime(2014, 9, 2), Genres = new List<GenreEntity> { genres[5], genres[3] } },
                        new GameEntity { Name = "Need for Speed Heat", Price = 599, Release = new DateTime(2019, 11, 8), Genres = new List<GenreEntity> { genres[4] } },
                        new GameEntity { Name = "Battlefield 1", Price = 399, Release = new DateTime(2016, 10, 21), Genres = new List<GenreEntity> { genres[9] } },
                        new GameEntity { Name = "Dead Space", Price = 599, Release = new DateTime(2023, 1, 27), Genres = new List<GenreEntity> { genres[2], genres[9] } }
                    }
                },

                new DeveloperEntity
                {
                    Name = "Ubisoft",
                    Games = new List<GameEntity>
                    {
                        new GameEntity { Name = "Assassin's Creed Valhalla", Price = 599, Release = new DateTime(2020, 11, 10), Genres = new List<GenreEntity> { genres[6], genres[5] } },
                        new GameEntity { Name = "Far Cry 6", Price = 599, Release = new DateTime(2021, 10, 7), Genres = new List<GenreEntity> { genres[9], genres[6] } },
                        new GameEntity { Name = "Watch Dogs Legion", Price = 499, Release = new DateTime(2020, 10, 29), Genres = new List<GenreEntity> { genres[9] } },
                        new GameEntity { Name = "The Crew 2", Price = 499, Release = new DateTime(2018, 6, 29), Genres = new List<GenreEntity> { genres[4] } },
                        new GameEntity { Name = "Rainbow Six Siege", Price = 199, Release = new DateTime(2015, 12, 1), Genres = new List<GenreEntity> { genres[9], genres[1] } }
                    }
                },

                new DeveloperEntity
                {
                    Name = "Microsoft Xbox Game Studios",
                    Games = new List<GameEntity>
                    {
                        new GameEntity { Name = "Forza Horizon 5", Price = 599, Release = new DateTime(2021, 11, 9), Genres = new List<GenreEntity> { genres[4] } },
                        new GameEntity { Name = "Halo Infinite", Price = 599, Release = new DateTime(2021, 12, 8), Genres = new List<GenreEntity> { genres[9], genres[7] } },
                        new GameEntity { Name = "Microsoft Flight Simulator", Price = 599, Release = new DateTime(2020, 8, 18), Genres = new List<GenreEntity> { genres[5] } },
                        new GameEntity { Name = "Age of Empires IV", Price = 599, Release = new DateTime(2021, 10, 28), Genres = new List<GenreEntity> { genres[1] } },
                        new GameEntity { Name = "Gears 5", Price = 399, Release = new DateTime(2019, 9, 10), Genres = new List<GenreEntity> { genres[9] } }
                    }
                }
                };
                await dbContext.Developers.AddRangeAsync(developers);
                await dbContext.SaveChangesAsync();
            }
            ;

            
        }
    }
}
