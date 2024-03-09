using GestorDeCitas.Application.DTOs;
using GestorDeCitas.Application.Interfaces;
using GestorDeCitas.Domain.Models;
using GestorDeCitas.Infrastructure.Interfaces;
using System.Security.Claims;

namespace DiabetesNoteBook.Application.Services
{
    //Hemos creado una interfaz para que el componente sea reutilizable por eso esta clase se ha
    //vinculado a una interfaz.
    public class NuevaCitaService:INuevaCitaService
    {
        //Se llama a base de datos y al servicio.

        private readonly AgendaCitaContext _context;
		private readonly IHttpContextAccessor _httpContextAccessor;
		//Se crea el constructor

		public NuevaCitaService(AgendaCitaContext context, IHttpContextAccessor accessor)
        {
            _context = context;
            _httpContextAccessor = accessor;
        }
        //Agregamos el metodo que se encuentra en la interfaz junto con el DTOMediciones

        public async Task NuevaCita(DTOCita cita)
        {
            //Creamos una nueva medicion
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            int id;
            if (int.TryParse(userId, out id))
            {
                var nuevaCita = new Citum()
                {
                   FechaYhora=cita.FechaYHora,
                   MotivoCita=cita.MotivoCita,
                   UbicacionCita=cita.UbicacionCita,
                   DuracionEstimada=cita.DuracionEstimada,
                   NombreDelProfesional=cita.NombreDelProfesional,
                   NotasAdicionales=cita.NotasAdicionales,
                   EstadoCita=cita.EstadoCita,
                    IdUsuario = id

                };
                //Llamamos al servicio para que la guarde
                await _context.Cita.AddAsync(nuevaCita);
                //Guardamos los cambios

                await _context.SaveChangesAsync();
            }
        

        }

    }
}
