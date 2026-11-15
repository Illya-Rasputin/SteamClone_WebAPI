using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO.Game
{
    public class GameImageDTO
    {

        public required string Name { get; set; }
        public bool IsPreview { get; set; } = false;

        public int GameId { get; set; }
        public GameEntity? Game { get; set; }
    }
}
