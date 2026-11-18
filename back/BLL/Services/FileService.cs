using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Settings;

namespace BLL.Services
{
    public class FileService
    {
        private readonly string _storagePath;

        public FileService()
        {
            var root = Directory.GetCurrentDirectory();
            _storagePath = Path.Combine(root, StaticFilesSettings.Storage);
        }

        public async Task<ServiceResponse> SaveImageAsync(IFormFile image, string destPath)
        {
            var types = image.ContentType.Split("/");

            if (types.Length != 2 || types[0] != "image")
            {
                return ServiceResponse.Error("Файл не є зображенням");
            }

            string folderPath = Path.Combine(_storagePath, destPath);

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string ext = Path.GetExtension(image.FileName);
            string imageName = Guid.NewGuid().ToString() + ext;
            string imagePath = Path.Combine(folderPath, imageName);

            using var stream = File.Create(imagePath);
            await image.CopyToAsync(stream);

            return ServiceResponse.Success("Зображення успішно збережене", imageName);
        }

        public async Task<ServiceResponse[]> SaveImagesAsync(IEnumerable<IFormFile> images, string destPath)
        {
            var tasks = new List<Task<ServiceResponse>>();

            foreach (var image in images)
            {
                tasks.Add(SaveImageAsync(image, destPath));
            }

            var res = await Task.WhenAll(tasks);

            return res;
        }
        public Task<ServiceResponse> DeleteImageAsync(string relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
            {
                return Task.FromResult(
                    ServiceResponse.Error("Шлях до зображення порожній"));
            }

            string storageRoot = Path.GetFullPath(_storagePath);
            string imagePath = Path.GetFullPath(
                Path.Combine(_storagePath, relativePath));

            // Prevent deleting files outside the storage directory.
            string rootWithSeparator = storageRoot
                .TrimEnd(Path.DirectorySeparatorChar)
                + Path.DirectorySeparatorChar;

            if (!imagePath.StartsWith(
                    rootWithSeparator,
                    StringComparison.OrdinalIgnoreCase))
            {
                return Task.FromResult(
                    ServiceResponse.Error("Некоректний шлях до зображення"));
            }

            if (File.Exists(imagePath))
            {
                File.Delete(imagePath);

                string? directoryPath = Path.GetDirectoryName(imagePath);

                if (directoryPath != null
                    && Directory.Exists(directoryPath)
                    && !Directory.EnumerateFileSystemEntries(directoryPath).Any())
                {
                    Directory.Delete(directoryPath);
                }
            }

            return Task.FromResult(
                ServiceResponse.Success("Зображення видалено"));
        }
    }
}
