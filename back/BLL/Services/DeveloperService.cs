using AutoMapper;
using BLL.DTO.Developer;
using BLL.Settings;
using DAL.Entities;
using DAL.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BLL.Services
{
    public class DeveloperService
    {
        private readonly DeveloperRepository _developerRepository;
        private readonly FileService _fileService;
        private readonly IMapper _mapper;

        public DeveloperService(DeveloperRepository developerRepository, FileService fileService, IMapper mapper)
        {
            _developerRepository = developerRepository;
            _fileService = fileService;
            _mapper = mapper;
        }

        private async Task<ServiceResponse> SaveImageAsync(IFormFile image)
        {
            string guid = Guid.NewGuid().ToString();

            string folderPath = Path.Combine(
                StaticFilesSettings.Developers,
                guid);

            var result = await _fileService.SaveImageAsync(
                image,
                folderPath);

            if (!result.IsSuccess)
            {
                return result;
            }

            string? fileName = result.Payload?.ToString();

            if (string.IsNullOrWhiteSpace(fileName))
            {
                return ServiceResponse.Error(
                    "Не вдалося отримати ім'я зображення");
            }

            return ServiceResponse.Success(
                "Зображення розробника збережено",
                $"{guid}/{fileName}");
        }

        public async Task<ServiceResponse> GetAllAsync()
        {
            var entities = await _developerRepository.Developers
                .Include(developer => developer.Games)
                .ToListAsync();

            var dtos = entities
                .Select(entity => _mapper.Map<DeveloperDTO>(entity))
                .ToList();

            return ServiceResponse.Success(
                "Розробників отримано",
                dtos);
        }

        public async Task<ServiceResponse> GetByIdAsync(int id)
        {
            var entity = await _developerRepository.GetByIdAsync(id);

            if (entity == null)
            {
                return ServiceResponse.Error(
                    $"Розробник з id '{id}' не знайдений");
            }

            return ServiceResponse.Success(
                "Розробника отримано",
                _mapper.Map<DeveloperDTO>(entity));
        }

        public async Task<ServiceResponse> GetByNameAsync(string name)
        {
            var entity = await _developerRepository.GetByNameAsync(name);

            if (entity == null)
            {
                return ServiceResponse.Error(
                    $"Розробник з ім'ям '{name}' не знайдений");
            }

            return ServiceResponse.Success(
                "Розробника отримано",
                _mapper.Map<DeveloperDTO>(entity));
        }

        public async Task<ServiceResponse> CreateAsync(
            CreateDeveloperDTO dto)
        {
            if (await _developerRepository.IsExistsAsync(dto.Name))
            {
                return ServiceResponse.Error(
                    $"Ім'я '{dto.Name}' вже використовується");
            }

            var entity = _mapper.Map<DeveloperEntity>(dto);

            if (dto.Image != null)
            {
                var imageResult = await SaveImageAsync(dto.Image);

                if (!imageResult.IsSuccess)
                {
                    return imageResult;
                }

                entity.Image = imageResult.Payload?.ToString();
            }

            bool result = await _developerRepository.CreateAsync(entity);

            if (!result)
            {
                return ServiceResponse.Error(
                    "Не вдалося додати розробника");
            }

            return ServiceResponse.Success(
                $"Розробник '{entity.Name}' успішно доданий",
                _mapper.Map<DeveloperDTO>(entity));
        }


        public async Task<ServiceResponse> UpdateAsync(
    UpdateDeveloperDTO dto)
        {
            var entity = await _developerRepository.GetByIdAsync(dto.Id);

            if (entity == null)
            {
                return ServiceResponse.Error(
                    $"Розробник з id '{dto.Id}' не знайдений");
            }

            if (await _developerRepository.IsExistsAsync(
                    dto.Name,
                    dto.Id))
            {
                return ServiceResponse.Error(
                    $"Ім'я '{dto.Name}' вже використовується");
            }

            string oldName = entity.Name;

            _mapper.Map(dto, entity);

            if (dto.Image != null)
            {
                if (!string.IsNullOrWhiteSpace(entity.Image))
                {
                    await _fileService.DeleteImageAsync(
                        Path.Combine(StaticFilesSettings.Developers, entity.Image));
                }

                var imageResult = await SaveImageAsync(dto.Image);

                if (!imageResult.IsSuccess)
                {
                    return imageResult;
                }

                entity.Image = imageResult.Payload?.ToString();
            }

            bool result = await _developerRepository.UpdateAsync(entity);

            if (!result)
            {
                return ServiceResponse.Error(
                    "Не вдалося змінити дані розробника");
            }

            var responseDto = _mapper.Map<DeveloperDTO>(entity);

            return ServiceResponse.Success(
                $"Розробник '{oldName}' успішно змінений",
                responseDto);
        }

        public async Task<ServiceResponse> DeleteAsync(int id)
        {
            var entity = await _developerRepository.GetByIdAsync(id);

            if (entity == null)
            {
                return ServiceResponse.Error($"Розробник з id '{id}' не знайдений");
            }

            if (!string.IsNullOrWhiteSpace(entity.Image))
            {
                await _fileService.DeleteImageAsync(
                    Path.Combine(StaticFilesSettings.Developers, entity.Image));
            }

            bool res = await _developerRepository.DeleteAsync(entity);

            if (!res)
            {
                return ServiceResponse.Error($"Не вдалося видалити розробника");
            }

            return ServiceResponse.Success($"Розробник '{entity.Name}' успішно видалений");
        }
    }
}
