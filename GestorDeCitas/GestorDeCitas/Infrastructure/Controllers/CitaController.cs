using GestorDeCitas.Application.DTOs;
using GestorDeCitas.Application.Interfaces;
using GestorDeCitas.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GestorDeCitas.Infrastructure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CitaController : ControllerBase
    {
        private readonly AgendaCitaContext _context;
        private readonly INuevaCitaService _cita;
        private readonly ILogger<UsersController> _logger;
        private readonly IDeleteCitaService _deleteCita;

        public CitaController(AgendaCitaContext context, INuevaCitaService nuevaCitaService, ILogger<UsersController> logger, IDeleteCitaService deleteCita)
        {
            _context = context;
            _cita = nuevaCitaService;
            _logger = logger;
            _deleteCita = deleteCita;
        }
        [HttpPost]
        public async Task<ActionResult> PostCita(DTOCita cita)
        {
            try
            {
                //var existeUsuario = await _usuarioPorId.ObtenerUsuarioPorId(id);
                //var existeUsuario = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id == mediciones.Id_Usuario);
                //Si la persona no existe devolvemos el mensaje contenido en NotFound.
                //var existeUsuario = await _existUsersService.UserExistById(mediciones.Id_Usuario);
                var existeUsuario = User.FindFirstValue(ClaimTypes.NameIdentifier);
                int usuarioId;
                if (int.TryParse(existeUsuario, out usuarioId))
                {
                    if (existeUsuario == null)
                    {
                        return NotFound("La persona a la que intenta poner la cita no existe");
                    }
                    //Llamamos al servicio medicion que contiene el metodo NuevaMedicion este metodo necesita un 
                    //DTOMediciones que contiene los datos necesarios para agregar la medicion a esa persona
                    await _cita.NuevaCita(new DTOCita
                    {
                       FechaYHora=cita.FechaYHora,
                       MotivoCita=cita.MotivoCita,
                       UbicacionCita=cita.UbicacionCita,
                       DuracionEstimada=cita.DuracionEstimada,
                       NombreDelProfesional=cita.NombreDelProfesional,
                       NotasAdicionales=cita.NotasAdicionales,
                       EstadoCita=cita.EstadoCita,




                    });







                }

                return Ok("Cita guardada con exito ");


            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Error al procesar la nueva cita");
                return BadRequest("En estos momentos no se ha podido realizar la insercción de la cita, por favor, intentelo más tarde.");
            }

        }
        [HttpDelete("eliminarcita")]
        public async Task<ActionResult> DeleteMedicion(DTOById Id)
        {
            //Buscamos la medicion por id en base de datos
            try
            {

               

                //var medicionExist =await _getMedicionExiste.GetMedicionAsync(Id.Id);
                var citaExist = await _context.Cita.FirstOrDefaultAsync(x => x.Id == Id.Id);
                //Si la medicion no existe devolvemos el mensaje contenido en BadRequest



                if (citaExist == null)
                {
                    return BadRequest("La cita que intenta eliminar no se encuentra");
                }


                //Llamamos al servicio _deleteMedicion que tiene un metodo DeleteMedicion el cual
                //necesita un DTOEliminarMedicion que  contiene los datos necesarios para eliminar la medicion
                await _deleteCita.DeleteCita(new DTOById
                {
                    Id = Id.Id
                });
                //Agregamos la operacion  llamando  al servicio _operationsService el cual tiene un
                //metodo AddOperacion este metodo necesita un DTOOperation el cual tiene los datos necesarios 
                //para agregar la operacion



                //Devolvemos un ok si todo va bien

                return Ok("Eliminacion realizada con exito");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al procesar el borrado de la cita");
                return BadRequest("En estos momentos no se ha podido realizar la ieliminación de la cita, por favor, intentelo más tarde.");

            }

        }
        [HttpGet("getcitaporidusuario/{Id}")]
        public async Task<ActionResult<IEnumerable<Citum>>> GetCitaPorIdUsuario([FromRoute] DTOById userData)
        {

            try
            {
                

                //Buscamos en base de datos la id del usuario el cual tiene asociadas mediciones
                //var mediciones = await _medicionesRepository.ObtenerMedicionesPorUsuarioId(userData.Id);
                var cita = await _context.Cita.Where(m => m.IdUsuarioNavigation.Id == userData.Id).ToListAsync();
                //Si la id del usuario que se le pasa no existe no encuentra las mediciones asociadas

                if (cita == null)
                {
                    return NotFound("Datos de cita no encontrados");
                }
                //Agregamos la operacion usando el servicio _operationsService que tiene un metodo
                //AddOperacion dicho metodo necesita un DTOOperation que contiene los datos necesarios
                //para realizar la operacion.


                //Si todo va bien se devuelve un ok

                return Ok(cita);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al procesar la solicitud de las  citas");
                return BadRequest("En estos momentos no se ha podido consultar los datos de la cita, por favor, intentelo más tarde.");
            }

        }

    }
}
