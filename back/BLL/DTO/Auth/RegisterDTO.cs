using System.ComponentModel.DataAnnotations;

namespace BLL.DTO.Auth
{
    public class RegisterDTO
    {
        [Required]
        [StringLength(256, MinimumLength = 3)]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(256)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;
    }
}