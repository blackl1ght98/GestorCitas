using GestorDeCitas.Application.DTOs;
using GestorDeCitas.Application.Interfaces;
using GestorDeCitas.Domain.Models;
using GestorDeCitas.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DiabetesNoteBook.Application.Services
{
    //Hemos creado una interfaz para que el componente sea reutilizable por eso esta clase se ha
    //vinculado a una interfaz
    public class ChangeUserDataService : IChangeUserDataService
    {
        private readonly AgendaCitaContext _context;
       private readonly IHttpContextAccessor _httpContextAccessor;
        
		//Creamos el constructor

		public ChangeUserDataService(AgendaCitaContext context, IHttpContextAccessor httpContextAccessor
            )
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            
          
          
        }
        //Ponemos el metodo que se encuentra en la interfaz el cual tiene un DTOChangeUserData que contiene
        //datos para poder hacer que este metodo cumpla su funcion
        
        public async Task ChangeUserData(DTOChangeUserData changeUserData)
        {
            try
            {
                //Como hemos implementado la separacion de responsabilidades ahora la logica de este get
                //se encuentra en la carpeta repositories-->GetOperations-->UserMedicationRetrieval.cs
                //var usuarioUpdate = await _userMedicationRetrieval.GetUserWithMedications(changeUserData.Id);
                // Buscar el usuario en la base de datos por su ID
                var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                int id;
                if (int.TryParse(userId, out id))
                {
                    var usuarioUpdate = await _context.Usuarios.AsTracking().FirstOrDefaultAsync(x => x.Id == id);
                    if (usuarioUpdate != null)
                    {

                        usuarioUpdate.Id = id;
                        usuarioUpdate.NombreCompleto = changeUserData.NombreCompleto;
                        usuarioUpdate.Email = changeUserData.Email;
                        usuarioUpdate.FechaNacimiento = changeUserData.FechaNacimiento;
                        usuarioUpdate.Telefono = changeUserData.Telefono;
                        usuarioUpdate.Direccion = changeUserData.Direccion;
                        _context.Usuarios.Update(usuarioUpdate);
                        await _context.SaveChangesAsync();

                    }
                }
                  
            }
            catch
            {
               
            }
        }
    }
}
