using System.ComponentModel.DataAnnotations;

namespace WebAPI_DOTNET8.DTOs
{
    public class UserLoginDTO
    {
        [Required]
        public required string UserName { get; set; }
        [Required]
        public required string Password { get; set; }
    }
}
