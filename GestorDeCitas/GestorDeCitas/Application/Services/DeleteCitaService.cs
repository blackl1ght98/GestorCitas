using GestorDeCitas.Application.DTOs;
using GestorDeCitas.Application.Interfaces;
using GestorDeCitas.Domain.Models;
using GestorDeCitas.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GestorDeCitas.Application.Services
{
    //Hemos creado una interfaz para que el componente sea reutilizable por eso esta clase se ha
    //vinculado a una interfaz.
    public class DeleteCitaService:IDeleteCitaService
    {
        //Llamamos a base de datos y servicio para poder hacer uso de ellos

        private readonly AgendaCitaContext _context;
     
		//Creamos el constructor

		public DeleteCitaService(AgendaCitaContext context)
        {
            _context = context;
           
        }
        //Agregamos el metodo que esta en la interfaz el cual tiene un DTOEliminarMedicion que contiene
        //los datos necesarios para gestionar la eliminacion
        public async Task DeleteCita(DTOById delete)
        {
            //Buscamos la medicion por id
            
			var deleteCita = await _context.Cita.FirstOrDefaultAsync(x => x.Id == delete.Id);
            //Llamamos al servicio encargado de eliminar y le pasamos lo que se va ha eliminar
            _context.Cita.Remove(deleteCita);
            await _context.SaveChangesAsync();
         
        }
    }
}
