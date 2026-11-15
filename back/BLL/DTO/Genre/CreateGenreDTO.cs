using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO.Genre
{
    public class CreateGenreDTO
    {
        [Required(ErrorMessage = "Поле 'Name' є обов'язковим")]
        public string Name { get; set; } = string.Empty;

    }
}
