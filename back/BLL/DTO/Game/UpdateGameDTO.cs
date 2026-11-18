using BLL.DTO.Genre;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO.Game
{
    public class UpdateGameDTO
    {
        [Required(ErrorMessage = "Поле 'Id' є обов'язковим")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Поле 'Name' є обов'язковим")]
        public string Name { get; set; } = string.Empty;
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero")]
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public DateTime Release { get; set; } = DateTime.Now;
        public int? DeveloperId { get; set; }
        public List<GenreDTO> Genres { get; set; } = [];
        public IFormFile? PreviewImage { get; set; }
        public List<IFormFile> Images { get; set; } = [];
    }
}
