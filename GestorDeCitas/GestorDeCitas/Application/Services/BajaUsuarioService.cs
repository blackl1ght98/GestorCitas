using GestorDeCitas.Application.DTOs;
using GestorDeCitas.Application.Interfaces;
using GestorDeCitas.Domain.Models;
using GestorDeCitas.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GestorDeCitas.Application.Services
{
    //Hemos creado una interfaz para que el componente sea reutilizable por eso esta clase se ha
    //vinculado a una interfaz.
    public class BajaUsuarioService : IBajaUsuarioServicio
    {
        //Llamamos a la base de datos y servicios necesarios

        private readonly AgendaCitaContext _context;
       
        //Creamos el constructor

        public BajaUsuarioService(AgendaCitaContext context)
        {
            _context = context;
            
        }
        //Agregamos el metodo que esta en la interfaz junto a su DTOUserDeregistration

        public async Task UserDeregistration(DTOUserDeregistration delete)
        {
            //Buscamos al usuario por su id  y que persona tiene asociada

            var usuarioDB = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id == delete.Id);
            usuarioDB.BajaUsuario = true;
            //Eliminamos de manera recursiva esa persona junto al usuario
            _context.Usuarios.Update(usuarioDB);
            //Guardamos cambios

            await _context.SaveChangesAsync();
        }
    }
}
