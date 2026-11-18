using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class GameEntity: BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public DateTime Release { get; set; } = DateTime.Now;
        public decimal Price { get; set; } = 0;
        public string? Description { get; set; }

        public int? DeveloperId { get; set; }
        public DeveloperEntity? Developer { get; set; }
        public List<GenreEntity> Genres { get; set; } = [];
        public List<GameImageEntity> Images { get; set; } = [];
    }
}
