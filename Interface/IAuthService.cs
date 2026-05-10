namespace ApiProyectoWeb.Interface
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(Models.DTOs.RegisterDto model);
        Task<string> LoginAsync(Models.DTOs.LoginDto model);
    }
}
