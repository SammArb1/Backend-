using ApiProyectoWeb.Interface;
using ApiProyectoWeb.Models;
using ApiProyectoWeb.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;

namespace ApiProyectoWeb.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<string> RegisterAsync(RegisterDto model)
        {
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName,
                Major = model.Major
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                return "User created successfully!";
            }

            var translatedErrors = result.Errors.Select(e => TranslateIdentityError(e.Code)).ToList();
            var errors = string.Join(", ", translatedErrors);
            return $"Error al crear usuario: {errors}";
        }

        private string TranslateIdentityError(string code)
        {
            return code switch
            {
                "DuplicateUserName" => "El correo ingresado ya está registrado.",
                "DuplicateEmail" => "El correo ingresado ya está registrado.",
                "PasswordTooShort" => "La contraseña debe tener al menos 8 caracteres.",
                "PasswordRequiresNonAlphanumeric" => "La contraseña debe tener al menos un carácter especial.",
                "PasswordRequiresDigit" => "La contraseña debe tener al menos un número.",
                "PasswordRequiresLower" => "La contraseña debe tener al menos una letra minúscula.",
                "PasswordRequiresUpper" => "La contraseña debe tener al menos una letra mayúscula.",
                "InvalidEmail" => "El correo ingresado no es válido.",
                "InvalidUserName" => "El nombre de usuario no es válido.",
                _ => "Error desconocido al procesar la solicitud."
            };
        }

        public async Task<string> LoginAsync(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.UserName!),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? "SecretKeyVeryLongStringRequiredForJWT123456789!"));

                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            return "Unauthorized";
        }

        public async Task<List<object>> GetUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            return users.Select(u => new {
                id = u.Id,
                fullName = u.FullName ?? u.UserName,
                email = u.Email,
                major = u.Major ?? "",
                avatarUrl = ""
            }).Cast<object>().ToList();
        }
    }
}
