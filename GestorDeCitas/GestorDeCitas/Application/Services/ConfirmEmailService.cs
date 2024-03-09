using Microsoft.EntityFrameworkCore;
using GestorDeCitas.Domain.Models;
using GestorDeCitas.Infrastructure.Interfaces;
using GestorDeCitas.Application.DTOs;
using GestorDeCitas.Application.Interfaces;

namespace GestorDeCitas.Application.Services
{
    //Hemos creado una interfaz para que el componente sea reutilizable por eso esta clase se ha
    //vinculado a una interfaz
    public class ConfirmEmailService : IConfirmEmailService
    {
        //Llamamos a base de datos y servicio que se encarga de guardar
        private readonly AgendaCitaContext _context;
		//Creamos el constructor

		public ConfirmEmailService(AgendaCitaContext context)
        {
            _context = context;
           
        }
        //Agregamos el metodo que esta en la interfaz junto a su DTOConfirmRegistrtion

        public async Task ConfirmEmail(DTOConfirmRegistrtion confirm)
        {
            //var usuarioUpdate = await _confimremail.ChangePassword(confirm.UserId);
		   //Buscamos en base de datos si existe el usuario en base a su id
		   var usuarioUpdate = _context.Usuarios.AsTracking().FirstOrDefault(x => x.Id == confirm.UserId);
            //ConfirmacionEmail esto lo establecemos a true una vez que el usuario haya confirmado su email

            usuarioUpdate.ConfirmacionEmail = true;
            //Llamamos al servicio _confirmationRegisterRepository encargado de gurdar y actualizar los datos
            _context.Usuarios.Update(usuarioUpdate);
            //Guardamos los cambios

            await _context.SaveChangesAsync();
        }
    }
}