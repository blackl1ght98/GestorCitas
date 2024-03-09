using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GestorDeCitas.Domain.Models;
using GestorDeCitas.Application.DTOs;
using Microsoft.EntityFrameworkCore;

namespace DiabetesNoteBook.Application.Services
{
    public class TokenService
    {
        private readonly IConfiguration _configuration;
        private readonly AgendaCitaContext _context;

        public TokenService(IConfiguration configuration, AgendaCitaContext context)
        {
            _configuration = configuration;
            _context = context;
        }
        public async Task<DTOLoginResponse> GenerarToken(Usuario credencialesUsuario)
        {

            var usuarioDB = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id == credencialesUsuario.Id);


            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Role, credencialesUsuario.Rol),
                new Claim(ClaimTypes.NameIdentifier, credencialesUsuario.Id.ToString()),
                new Claim(ClaimTypes.Email, credencialesUsuario.Email),

            };

            var clave = _configuration["ClaveJWT"];
            var claveKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(clave));
            var signinCredentials = new SigningCredentials(claveKey, SecurityAlgorithms.HmacSha256);
            var securityToken = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: signinCredentials);
            var tokenString = new JwtSecurityTokenHandler().WriteToken(securityToken);

            return new DTOLoginResponse()
            {
                Id = credencialesUsuario.Id,
                Token = tokenString,
                Rol = credencialesUsuario.Rol,
                NombreCompleto = credencialesUsuario.NombreCompleto,
                Email = credencialesUsuario.Email,
                Direccion= credencialesUsuario.Direccion,
                Telefono= credencialesUsuario.Telefono,
                FechaNacimiento= credencialesUsuario.FechaNacimiento,
                FechaRegistro=credencialesUsuario.FechaRegistro
              
            };
        }
    }
}
