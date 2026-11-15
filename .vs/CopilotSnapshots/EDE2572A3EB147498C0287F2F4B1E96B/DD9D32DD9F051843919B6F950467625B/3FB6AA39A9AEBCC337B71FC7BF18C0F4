using BLL.DTO.Developer;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BLL.DTO.Game;
using DAL.Entities;

namespace BLL.Services
{
    public class GameService
    {
        private readonly GameRepository _gameRepository;
        private readonly DeveloperRepository _developerRepository;
        private readonly GameImageRepository _gameImageRepository;

        public GameService(GameRepository gameRepository, DeveloperRepository developerRepository, GameImageRepository gameImageRepository)
        {
            _gameRepository = gameRepository;
            _developerRepository = developerRepository;
            _gameImageRepository = gameImageRepository;
        }

        public async Task<ServiceResponse> GetAllAsync()
        {
            var entities = await _gameRepository.Games
                .Include(g => g.Developer)
                .ToListAsync();

            var dtos = entities.Select(e => new GameDTO
            {
                Images = e.Images.Select(i => new GameImageDTO
                {
                    Name = i.Name,
                    IsPreview = i.IsPreview,
                    GameId = i.GameId,
                    Game = i.Game
                }).ToList(),
                Name = e.Name,
                Id = e.Id
            });

            return ServiceResponse.Success("Ігор отримано", dtos);
        }

        public async Task<ServiceResponse> GetByIdAsync(int id)
        {
            var entity = await _gameRepository.GetByIdAsync(id);

            if (entity == null)
            {
                return ServiceResponse.Error($"Гра з id '{id}' не знайдена");
            }

            var dto = new GameDTO
            {
                Id = entity.Id,
                Name = entity.Name,
                Price = entity.Price,
                DeveloperId = entity.DeveloperId
            };

            return ServiceResponse.Success("Гру отримано", dto);
        }
        public async Task<ServiceResponse> CreateAsync(CreateGameDTO dto)
        {
            if (await _gameRepository.IsExistsAsync(dto.Name))
            {
                return ServiceResponse.Error($"Гра з назвою '{dto.Name}' вже існує");
            }
            var entity = new GameEntity
            {
                Name = dto.Name,
                Price = dto.Price,
                DeveloperId = dto.DeveloperId
            };
            bool res = await _gameRepository.CreateAsync(entity);
            if (!res)
            {
                return ServiceResponse.Error($"Не вдалося додати гру");
            }
            var responseDto = new GameDTO
            {
                Id = entity.Id,
                Name = entity.Name,
                Price = entity.Price,
                DeveloperId = entity.DeveloperId
            };
            return ServiceResponse.Success($"Гра '{dto.Name}' успішно додана", responseDto);
        }

        public async Task<ServiceResponse> UpdateAsync(UpdateGameDTO dto)
        {
            var entity = await _gameRepository.GetByIdAsync(dto.Id);
            if (entity == null)
            {
                return ServiceResponse.Error($"Гра з id '{dto.Id}' не знайдена");
            }
            entity.Name = dto.Name;
            entity.Price = dto.Price;
            entity.DeveloperId = dto.DeveloperId;
            bool res = await _gameRepository.UpdateAsync(entity);
            if (!res)
            {
                return ServiceResponse.Error($"Не вдалося змінити дані гри");
            }
            var responseDto = new GameDTO
            {
                Id = entity.Id,
                Name = entity.Name,
                Price = entity.Price,
                DeveloperId = entity.DeveloperId
            };
            return ServiceResponse.Success($"Дані гри '{dto.Name}' успішно змінено", responseDto);
        }

        public async Task<ServiceResponse> DeleteAsync(int id)
        {
            var entity = await _gameRepository.GetByIdAsync(id);
            if (entity == null)
            {
                return ServiceResponse.Error($"Гра з id '{id}' не знайдена");
            }
            bool res = await _gameRepository.DeleteAsync(entity);
            if (!res)
            {
                return ServiceResponse.Error($"Не вдалося видалити гру");
            }
            return ServiceResponse.Success($"Гра '{entity.Name}' успішно видалена");
        }   
    }
}
