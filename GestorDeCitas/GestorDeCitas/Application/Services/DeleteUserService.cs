using GestorDeCitas.Application.DTOs;
using GestorDeCitas.Application.Interfaces;
using GestorDeCitas.Domain.Models;
using GestorDeCitas.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GestorDeCitas.Application.Services
{
    //Hemos creado una interfaz para que el componente sea reutilizable por eso esta clase se ha
    //vinculado a una interfaz.
    public class DeleteUserService : IDeleteUserService
    {
        //Llamamos a base de datos y al servicio que va a usar

        private readonly AgendaCitaContext _context;

		public DeleteUserService(AgendaCitaContext context)
        {
            _context = context;
        }
     
        public async Task DeleteUser(DTODeleteUser delete)
        {
           
            var usuarioDB = await _context.Usuarios.Include(x => x.Cita).FirstOrDefaultAsync(x => x.Id == delete.Id);
            //llamamos al servicio encargado de eliminar este servicio borra recursivamente
           
            _context.Cita.RemoveRange(usuarioDB.Cita);
       
            _context.Usuarios.Remove(usuarioDB);
            //Guardamos los cambios

            await _context.SaveChangesAsync();
            
        }

    }
}
