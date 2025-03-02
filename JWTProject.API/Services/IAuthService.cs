using JWTProject.Data.Models;

namespace JWTProject.API.Services
{
    public interface IAuthService
    {
        Task<(int, string)> Registeration(RegistrationModel model, string role);
        Task<(int, string)> Login(LoginModel model);
        Task<(int, string)> CreateRole(string role);
    }
}
