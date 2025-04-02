using ControleGastosPessoais.Shared.Models.AuthenticationModels;

namespace ControleGastosPessoais.Client.Services.Interfaces;

public interface IAuthService
{
    Task<LoginResult> Login(LoginModel loginModel);
    Task Logout();
    Task<RegisterResult> Register(RegisterModel registerModel);
}
