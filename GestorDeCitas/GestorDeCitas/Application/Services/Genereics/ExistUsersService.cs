using GestorDeCitas.Application.DTOs;
using GestorDeCitas.Domain.Models;
using GestorDeCitas.Infrastructure.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestorDeCitas.Application.Services.Genereics
{
    public class ExistUsersService
    {
        private readonly AgendaCitaContext _context;
        private readonly ILogger<UsersController> _logger;

        public ExistUsersService(AgendaCitaContext context, ILogger<UsersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        

        public async Task<bool> EmailExist(string email)
        {
            try
            {
                var usuarioDB = await _context.Usuarios.AsTracking().FirstOrDefaultAsync(x => x.Email == email);

                return usuarioDB != null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al procesar la consulta de email existente");
                throw new Exception("Error al procesar la solicitud");
            }
        }

        public async Task<Usuario> UserExistById(int id)
        {
            try
            {
                var usuarioDB = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id == id);

                return usuarioDB;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al procesar la consulta de id de usuario existente");
                throw new Exception("Error al procesar la solicitud");
            }
        }

       

        public async Task<bool> UserExistByEmail(string email)
        {
            try
            {
                var usuarioDB = await _context.Usuarios.FirstOrDefaultAsync(x => x.ConfirmacionEmail == true);

                return usuarioDB != null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al procesar la consulta de email de usuario existente");
                throw new Exception("Error al procesar la solicitud");
            }
        }

        public async Task<Usuario> UserTokenExist(DTOUsuarioChangePasswordMailConEnlace userEmail)
        {
            try
            {
                var usuarioDB = await _context.Usuarios.AsTracking().FirstOrDefaultAsync(x => x.EnlaceCambioPass == userEmail.Token);

                return usuarioDB;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al procesar la consulta de id de usuario existente");
                throw new Exception("Error al procesar la solicitud");
            }
        }
    }
}
