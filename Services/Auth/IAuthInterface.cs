using WebAPI_DOTNET8.DTOs;
using WebAPI_DOTNET8.Models;

namespace WebAPI_DOTNET8.Services.Auth
{
    public interface IAuthInterface
    {
        Task<ResponseModel<string>> Login(UserLoginDTO userLoginDTO);
    }
}
