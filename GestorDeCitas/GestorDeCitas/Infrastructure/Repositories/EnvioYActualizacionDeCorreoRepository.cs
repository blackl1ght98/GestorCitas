using GestorDeCitas.Application.DTOs;
using GestorDeCitas.Application.Interfaces;
using GestorDeCitas.Domain.Models;
using GestorDeCitas.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GestorDeCitas.Infrastructure.Repositories
{
	public class EnvioYActualizacionDeCorreoRepository: IActualizacionYEnvioDeCorreoElectronico
	{
		private readonly AgendaCitaContext _context;
		private readonly IEmailService _emailService;

		public EnvioYActualizacionDeCorreoRepository(AgendaCitaContext context, IEmailService emailService)
		{
			_context = context;
			_emailService = emailService;
		}
       
        public async Task<bool> ActualizarEmailUsuario(int userId, string nuevoEmail)
        {
            var usuarioActualizado = await _context.Usuarios.AsTracking().FirstOrDefaultAsync(x => x.Id == userId);

            if (usuarioActualizado != null)
            {
                if (usuarioActualizado.Email != nuevoEmail)
                {
                    usuarioActualizado.ConfirmacionEmail = false;
                    usuarioActualizado.Email = nuevoEmail;
                    _context.Usuarios.Update(usuarioActualizado);
                    await _context.SaveChangesAsync();
                    	

                    await _emailService.SendEmailAsyncRegister(new DTOEmail
                    {
                        ToEmail = nuevoEmail
                    });
                }
                else
                {
                    // No hay cambios en el email, pero aún así se envía el correo electrónico de confirmación
                    //await _emailService.SendEmailAsyncRegister(new DTOEmail
                    //{
                    //    ToEmail = nuevoEmail
                    //});
                }
                return true;
            }

            return false;
        }


        public async Task<bool> EnviarCorreoElectronico(string nuevoEmail)
		{
			await _emailService.SendEmailAsyncRegister(new DTOEmail
			{
				ToEmail = nuevoEmail
			});
			return true;
		}
	}
}
