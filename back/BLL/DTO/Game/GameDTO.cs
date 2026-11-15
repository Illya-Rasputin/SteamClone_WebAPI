using BLL.DTO.Developer;
using BLL.DTO.Genre;

namespace BLL.DTO.Game
{
    public class GameDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public DateTime Release { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public int? DeveloperId { get; set; }
        public DeveloperDTO? Developer { get; set; }
        public List<GenreDTO> Genres { get; set; } = [];
        public List<GameImageDTO> Images { get; set; } = [];
    }
}
