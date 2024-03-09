using Microsoft.EntityFrameworkCore;
using GestorDeCitas.Domain.Models;
using GestorDeCitas.Application.DTOs;
using GestorDeCitas.Application.Interfaces;
using GestorDeCitas.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using DiabetesNoteBook.Application.Services;

namespace GestorDeCitas.Application.Services
{
    //Hemos creado una interfaz para que el componente sea reutilizable por eso esta clase se ha
    //vinculado a una interfaz
    //Este es el que cambia la contraseña una vez que has iniciado sesion permite cambiar la contraseña
    public class ChangePassService : IChangePassService
    {
        //Llamamos a los servicios necesarios para que este servicio cumpla su funcion
        private readonly HashService _hashService;
        private readonly AgendaCitaContext _context;
        
       
		//Creamos el contructor
		public ChangePassService(HashService hashService, AgendaCitaContext context)
        {
            _hashService = hashService;
            _context = context;
          
           
        }
        //Agregamos el metodo task que esta en la interfaz con el DTOCambioPassPorId para poder cambiar la contraseña
        //una vez logueados, este dto dispone de los datos que se necesitan junto a la base de datos
        public async Task ChangePassId(DTOCambioPassPorId userData)
        {
            
		   //Buscamos en base de datos al usuario por su id
		   var usuarioDB = await _context.Usuarios.AsTracking().FirstOrDefaultAsync(x => x.Id == userData.Id);
		   //Llamamos al servicio _hashService este servicio tiene un metodo Hash  al cual se le pasa la 
		   //nueva contraseña que cree el usuario para que la cifre
			var resultadoHash = _hashService.Hash(userData.NewPass);
            //A la contraseña que cree el usuario se cifra con el hash y se le asigna un salt
            usuarioDB.Password = resultadoHash.Hash;
            usuarioDB.Salt = resultadoHash.Salt;
            _context.Usuarios.Update(usuarioDB);
            //Guardamos los cambios en base de datos

            await _context.SaveChangesAsync();
            
        }

        

    }
}
