using GestorDeCitas.Application.DTOs;
using GestorDeCitas.Application.Interfaces;
using GestorDeCitas.Domain.Models;
using GestorDeCitas.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DiabetesNoteBook.Application.Services
{
    //Hemos creado una interfaz para que el componente sea reutilizable por eso esta clase se ha
    //vinculado a una interfaz
    public class ChangePassEnlace : IChangePassMail
    {
        //Se llaman a los servicios necesarios para que este servicio cumpla su funcion

        private readonly HashService _hashService;
        private readonly AgendaCitaContext _context;
    
		//Creamos el constructor

		public ChangePassEnlace(HashService hashService, AgendaCitaContext context)
        {

            _hashService = hashService;
            _context = context;
           
            
        }
        //Ponemos el metodo de tipo task que esta en la interfaz el cual recibe un DTOUsuarioChangePasswordMailConEnlace
        //este dto gestiona los datos necesarios para que el servicio funcione.
        public async Task ChangePassEnlaceMail(DTOUsuarioChangePasswordMailConEnlace userData)
        {
            
		   //Vemos si en base de datos existe el token que genera al cambiar la contraseña via email
		   var usuarioDB = await _context.Usuarios.AsTracking().FirstOrDefaultAsync(x => x.EnlaceCambioPass == userData.Token);
            //Llamamos al servicio _hashService el cual tiene un metodo hash y ha este metodo le pasamos la
            //contraseña nueva que ponga el usuario este servicio se encarga de cifrar esta contraseña
            var resultadoHash = _hashService.Hash(userData.NewPass);
            //A la contraseña que se crea le asignamos un hash y un salt
            usuarioDB.Password = resultadoHash.Hash;
            usuarioDB.Salt = resultadoHash.Salt;
            _context.Usuarios.Update(usuarioDB);
            //Guardamos los cambios en base de datos

            await _context.SaveChangesAsync();
           
        }
    }
}
