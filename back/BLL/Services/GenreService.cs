using BLL.DTO.Developer;
using BLL.DTO.Game;
using BLL.DTO.Genre;
using DAL.Entities;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class GenreService
    {
        private readonly GenreRepository _genreRepository;

        public GenreService(GenreRepository genreRepository)
        {
            _genreRepository = genreRepository;
        }

        public async Task<ServiceResponse> GetAllAsync()
        {
            var entities = await _genreRepository.GetAll().ToListAsync();
            var dtos = entities.Select(e => new GenreDTO
            {
                Name = e.Name,
                Id = e.Id
            });
            return ServiceResponse.Success("Genres retrieved successfully", dtos);
        }
        public async Task<ServiceResponse> GetByIdAsync(int id)
        {
            var entity = await _genreRepository.GetByIdAsync(id);

            if (entity == null)
            {
                return ServiceResponse.Error($"Жанр з id '{id}' не знайдений");
            }

            var dto = new GenreDTO
            {
                Id = entity.Id,
                Name = entity.Name
            };

            return ServiceResponse.Success("Жанр отримано", dto);
        }
        public async Task<ServiceResponse> CreateAsync(CreateGenreDTO dto)
        {
            if (await _genreRepository.IsExistsAsync(dto.Name))
            {
                return ServiceResponse.Error($"Genre with name '{dto.Name}' already exists");
            }
            var entity = new GenreEntity
            {
                Name = dto.Name
            };
            bool res = await _genreRepository.CreateAsync(entity);
            if (!res)
            {
                return ServiceResponse.Error("Failed to create genre");
            }
            return ServiceResponse.Success("Genre created successfully", new GenreDTO
            {
                Id = entity.Id,
                Name = entity.Name
            });
        }
        public async Task<ServiceResponse> UpdateAsync(UpdateGenreDTO dto)
        {
            var entity = await _genreRepository.GetByIdAsync(dto.Id);
            if (entity == null)
            {
                return ServiceResponse.Error($"Жанр з id '{dto.Id}' не знайдений");
            }
            entity.Name = dto.Name;
         
            bool res = await _genreRepository.UpdateAsync(entity);
            if (!res)
            {
                return ServiceResponse.Error($"Не вдалося змінити дані жанру");
            }
            var responseDto = new GenreDTO
            {
                Id = entity.Id,
                Name = entity.Name,
                
            };
            return ServiceResponse.Success($"Дані жанру '{dto.Name}' успішно змінено", responseDto);
        }
        public async Task<ServiceResponse> DeleteAsync(int id)
        {
            var entity = await _genreRepository.GetByIdAsync(id);
            if (entity == null)
            {
                return ServiceResponse.Error($"Жанр з id '{id}' не знайдений");
            }
            bool res = await _genreRepository.DeleteAsync(entity);
            if (!res)
            {
                return ServiceResponse.Error("Failed to delete genre");
            }
            return ServiceResponse.Success("Genre deleted successfully");
        }
    }
}
