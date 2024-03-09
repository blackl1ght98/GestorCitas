using Microsoft.EntityFrameworkCore;
using GestorDeCitas.Domain.Models;
using GestorDeCitas.Infrastructure.Interfaces;
using GestorDeCitas.Application.Interfaces;
using GestorDeCitas.Application.DTOs;

namespace DiabetesNoteBook.Application.Services
{
    //Hemos creado una interfaz para que el componente sea reutilizable por eso esta clase se ha
    //vinculado a una interfaz.
    public class NewRegisterService : INewRegister
    {
        //Llamamos a los servicios necesarios y base de datos para hacer uso de ellos 

        private readonly AgendaCitaContext _context;
        private readonly HashService _hashService;
        

		//Creamos el contructor

		public NewRegisterService(AgendaCitaContext context, HashService hashService
			 )
        {
            _context = context;
            _hashService = hashService;
           

        }
        //Agregamos el metodo que esta en la interfaz al cual se le pasa un DTORegister
        //que tiene los datos necesarios para registrar un usuario
        public async Task NewRegister(DTORegister userData)
        {
            try
            {
                var resultadoHash = _hashService.Hash(userData.Password);

                var newUsuario = new Usuario
                {
                   
               
                    Email = userData.Email,
                    Password = resultadoHash.Hash,
                    Salt = resultadoHash.Salt,
                    Rol = "user",
                    NombreCompleto = userData.NombreCompleto,
                    FechaNacimiento=userData.FechaNacimiento,
                    Telefono = userData.Telefono,
                    Direccion=userData.Direccion,
                    FechaRegistro=DateTime.Now
                   
                };

              
                await _context.Usuarios.AddAsync(newUsuario);
             

                await _context.SaveChangesAsync();
               
               

			}
            catch
            {
                
            }
        }


    }
}