using System.ComponentModel.DataAnnotations;

namespace WebAPI_DOTNET8.DTOs
{
    public class UserCreateDTO
    {
        [Required(ErrorMessage = "Nome de usuário é obrigatório")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Nome deve ter entre 3 e 20 caracteres")]
        public required string UserName { get; set; }

        [Required(ErrorMessage = "Senha é obrigatória")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Senha deve ter no mínimo 8 caracteres")]
        [DataType(DataType.Password)]
        public required string Password { get; set; }

        [Compare("Password", ErrorMessage = "Senhas não conferem")]
        [DataType(DataType.Password)]
        public required string ConfirmPassword { get; set; }
    }
}
