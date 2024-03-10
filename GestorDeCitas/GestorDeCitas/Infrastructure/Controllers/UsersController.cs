using DiabetesNoteBook.Application.Services;
using GestorDeCitas.Application.DTOs;
using GestorDeCitas.Application.Interfaces;
using GestorDeCitas.Application.Services.Genereics;
using GestorDeCitas.Domain.Models;
using GestorDeCitas.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace GestorDeCitas.Infrastructure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AgendaCitaContext _context;
        private readonly HashService _hashService;
        private readonly TokenService _tokenService;
        private readonly IEmailService _emailService;
        private readonly ExistUsersService _existUsersService;
        private readonly INewRegister _newRegisterService;
        private readonly IConfirmEmailService _confirmEmailService;
        private readonly ILogger<UsersController> _logger;
        private readonly IBajaUsuarioServicio _bajaUsuarioServicio;
        private readonly IDeleteUserService _deleteUserService;
        private readonly IChangeUserDataService _changeUserDataService;
        private readonly IConfiguration _config;

        private readonly IActualizacionYEnvioDeCorreoElectronico _actualizacionYEnvioDeCorreoElectronico;
        public UsersController(AgendaCitaContext context, HashService hashService, TokenService tokenService, IEmailService emailService,
            ExistUsersService existUsersService, INewRegister newRegister, IConfirmEmailService confirmEmailService, ILogger<UsersController> logger, IBajaUsuarioServicio bajaUsuarioServicio, IDeleteUserService deleteUserService, IChangeUserDataService changeUserDataService,
            IActualizacionYEnvioDeCorreoElectronico actualizacionYEnvioDeCorreoElectronico, IConfiguration config)
        {
            _context = context;
            _hashService = hashService;
            _tokenService = tokenService;
            _emailService = emailService;
            _existUsersService = existUsersService;
            _newRegisterService = newRegister;
            _confirmEmailService = confirmEmailService;
            _logger = logger;
            _bajaUsuarioServicio = bajaUsuarioServicio;
            _deleteUserService = deleteUserService;
            _changeUserDataService = changeUserDataService;
            _actualizacionYEnvioDeCorreoElectronico = actualizacionYEnvioDeCorreoElectronico;
            _config = config;
        }
        [AllowAnonymous]
        [HttpPost("registro")]
        public async Task<ActionResult> UserRegistration([FromBody] DTORegister userData)
        {

            try
            {
             

               
                var usuarioDBEmail = await _existUsersService.EmailExist(userData.Email);
                if (usuarioDBEmail is true)
                {
                    return BadRequest("El email ya se encuentra registrado");
                }
               
                await _newRegisterService.NewRegister(new DTORegister
                {
                   
                   
                    Email = userData.Email,
                    Password = userData.Password,
                    NombreCompleto = userData.NombreCompleto,
                    FechaNacimiento=userData.FechaNacimiento,
                    Telefono= userData.Telefono,
                    Direccion=userData.Direccion,
                    FechaRegistro=DateTime.Now
                  
                });
                //cuando el usuario se registra hay un servicio que manda un email para que confirme la
                //creacion de la cuenta si el usuario no confirma su cuenta no puede hacer login hasta que
                //no confirme su email esto se hace para evitar accesos no deseados.
                //Este servicio de enviar el email tiene un metodo SendEmailAsyncRegister que precisa de un
                //DTOEmail que contiene el email.
                await _emailService.SendEmailAsyncRegister(new DTOEmail
                {
                    ToEmail = userData.Email
                });



                return Ok();
            }
            catch (Exception ex)
            {
               _logger.LogError(ex, "Error al procesar el registro");
                return BadRequest("En estos momentos no se ha podido realizar le registro, por favor, intentelo más tarde.");
            }

        }
        [AllowAnonymous]
        [HttpGet("validarRegistro/{UserId}/{Token}")]
        public async Task<ActionResult> ConfirmRegistration([FromRoute] DTOConfirmRegistrtion confirmacion)
        {

            try
            {
                var usuarioDB = await _existUsersService.UserExistById(confirmacion.UserId);

                //var usuarioDB = _usuarioPorId.ObtenerUsuarioPorId(confirmacion.UserId);
                //var usuarioDB = _context.Usuarios.FirstOrDefault(x => x.Id == confirmacion.UserId);

                if (usuarioDB.ConfirmacionEmail != false)
                {
                    return BadRequest("Usuario ya validado con anterioridad");
                }

                if (usuarioDB.EnlaceCambioPass != confirmacion.Token)
                {
                    return BadRequest("Token no valido");
                }

                await _confirmEmailService.ConfirmEmail(new DTOConfirmRegistrtion
                {
                    UserId = confirmacion.UserId
                });

                string loginUrl = _config.GetValue<string>("RedirectUrls:Login");
                //return Ok();
                return Redirect(loginUrl);
                // return Redirect(loginUrl);

            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "Error al procesar de confirmación");
                return BadRequest("En estos momentos no se ha podido validar el registro, por favor, intentelo de nuevo más tarde.");
            }
           
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult> UserLogin([FromBody] DTOLoginUsuario usuario)
        {

            try
            {

                var usuarioDB = await _context.Usuarios.FirstOrDefaultAsync(x => x.Email == usuario.Email);

                //Si el usuario existe pero no ha confirmado su email devolvemos el mensaje contenido en
                //Unauthorized
                if (usuarioDB.ConfirmacionEmail != true)
                {
                    return Unauthorized("Usuario no confirmado, por favor acceda a su correo y valida su registro.");
                }
                //Si el usuario ha solicidado darse de baja de la aplicacion he intenta loguearse se le
                //avisara al usuario con el mensaje contenido en Unauthorized.
                if (usuarioDB.BajaUsuario == true)
                {
                    return Unauthorized("El usuario se encuentra dado de baja.");
                }
                //Esta variable almacena la llamada al servicio _hashService este servicio tiene un metodo
                //llamado hash al cual se le pasa la contraseña de usuario para que la cifre y se le asigna un
                //salt que corresponde a esa contraseña
                var resultadoHash = _hashService.Hash(usuario.Password, usuarioDB.Salt);
                //Se comprueba si la contraseña que intruce el usuario corresponde con el hash que tiene asociado
                //esa contraseña en base de datos.
                if (usuarioDB.Password == resultadoHash.Hash)
                {
                    //Si la contraseña es correcta se le devuelve el token al usuario

                    var response = await _tokenService.GenerarToken(usuarioDB);

                    //Si todo ha ido bien se le devuelve el token

                    return Ok(response);
                }
                else
                {
                    //Si el usuario se equivoca en la contraseña se le devuelve este error

                    return Unauthorized("Contraseña incorrecta.");
                }

            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "Error al procesar de logado");
                return BadRequest("En estos momentos no se ha podido realizar el login, por favor, intentelo más tarde." + ex);
            }

        }
        [HttpPut("bajaUsuario")]
        public async Task<ActionResult> UserDeregistration([FromBody] DTOUserDeregistration Id)
        {

            try
            {
                //Buscamos si el usuario existe en base de datos esta busqueda se realiza en base a su id
                // var userExist = _usuarioPorId.ObtenerUsuarioPorId(Id.Id);
                //var userExist = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id == Id.Id);
                //Si se intenta dar de baja a un usuario que no existe sale el mensaje contenido en
                //Unauthorized
                var userExist = await _existUsersService.UserExistById(Id.Id);

                if (userExist == null)
                {
                    return Unauthorized("Usuario no encontrado");
                }
                //Si el usuario se intenta dar de baja nuevamente sale el mensaje contenido en Unauthorized

                if (userExist.BajaUsuario == true)
                {
                    return Unauthorized("Usuario dado de baja con anterioridad");
                }
                //Si todo va bien el usuario se da de baja correctamente, para ello se llama al servicio
                //_userDeregistrationService que tiene un metodo UserDeregistration dicho metodo necesita
                //un DTOUserDeregistration que contiene los datos necesarios para procesar la baja de usuario
                await _bajaUsuarioServicio.UserDeregistration(new DTOUserDeregistration
                {
                    Id = Id.Id
                });

                //Si todo ha ido bien se devuelve un ok.

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al procesar de baja");
                return BadRequest("En estos momentos no se ha podido dar de baja el usuario, por favor, intentelo más tarde.");
            }


        }
        [AllowAnonymous]
        [HttpDelete("elimnarUsuario")]
        public async Task<ActionResult> DeleteUser([FromBody] DTODeleteUser Id)
        {

            try
            {
                var userExist = await _existUsersService.UserExistById(Id.Id);

                //var userExist = _usuarioPorId.ObtenerUsuarioPorId(Id.Id);
                //Buscamos en base de datos si existe el usuario en base de datos en base a su id.
                //var userExist = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id == Id.Id);
                //Si el usuario no existe al administrador del sitio le sale el mensaje contenido en
                //Unauthorized.
                if (userExist == null)
                {
                    return Unauthorized("Usuario no encontrado");
                }
                //await _operationsService.AddOperacion(new DTOOperation
                //{
                //	Operacion = "Borrar usuario",
                //	UserId = userExist.Id
                //});
                //Si el usuario no se ha dado de baja no se puede eliminar por lo tanto al administrador se le
                //comunica que el usuario no se ha dado de baja por lo tanto necesita darse de baja.
                if (userExist.BajaUsuario == false)
                {
                    return Unauthorized("El usuario no se encuentra dado de baja, por favor, solicita la baja primero.");
                }
                //Para eliminar al usuario llamamos al servicio _deleteUserService el cual tiene un metodo
                //DeleteUser este metodo necesita un DTODeleteUser que contiene los datos necesarios para
                //eliminar a un usuario.

                await _deleteUserService.DeleteUser(new DTODeleteUser
                {
                    Id = Id.Id
                });
                //Una vez que se ha eliminado el usuario se agrega una operacion llamando al servicio
                //_operationsService que contiene un metodo AddOperacion y contiene un DTOOperation el
                //cual tiene los datos necesarios para agregar la operacion.


                //Si todo ha ido bien devolvemos un ok.

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al procesar de eliminación de usuario");
                return BadRequest("En estos momentos no se ha podido eliminar el usuario, por favor, intentelo más tarde.");
            }

        }
        [HttpGet("usuarioPorId/{Id}")]
        public async Task<ActionResult> UserById([FromRoute] DTOById userData)
        {

            try
            {
                var userExist = await _existUsersService.UserExistById(userData.Id);

                //Buscamos en base de datos si el usuario existe en base a su id
                // var userExist = _usuarioPorId.ObtenerUsuarioPorId(userData.Id);
                //var userExist = await _context.Usuarios.FindAsync(userData.Id);
                //Si el usuario no existe mostramos el mensaje contenido en NotFound.

                if (userExist == null)
                {
                    return NotFound("Usuario no encontrado");
                }
                // Consultar todas las citas del usuario
                var citasUsuario = await _context.Cita
                    .Include(um => um.IdUsuarioNavigation)
                    .Where(um => um.IdUsuario == userExist.Id)
                    .ToListAsync();

                // Asignar las citas al usuario
                userExist.Cita = citasUsuario;


                // Asignar las medicaciones al usuario
                //Si todo ha ido bien devolvemos el usuario.

                return Ok(userExist);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener informacion del usuario");
                return BadRequest("En estos momentos no se ha podido consultar el usuario, por favor, intentelo más tarde.");
            }

        }
        [HttpPatch("cambiardatosusuario")]
        public async Task<ActionResult> UserPUT([FromBody] DTOChangeUserData userData)
        {

            try
            {
                //Se busca en base de datos si el usuario existe en base a su id
                //var usuarioUpdate = await _context.Usuarios.AsTracking().FirstOrDefaultAsync(x => x.Id == userData.Id);
                //var usuarioUpdate = _usuarioPorId.ObtenerUsuarioPorId(userData.Id);
                var userExist = await _existUsersService.UserExistById(userData.Id);

            
                await _changeUserDataService.ChangeUserData(new DTOChangeUserData
                {
                    Id = userData.Id,
                    Email = userData.Email,
                    FechaNacimiento=userData.FechaNacimiento,
                    NombreCompleto = userData.NombreCompleto,
                    Telefono= userData.Telefono,
                    Direccion=userData.Direccion,

                });

                //// Actualizar email del usuario
                //var usuarioActualizado = await _context.Usuarios.AsTracking().FirstOrDefaultAsync(x => x.Id == userData.Id);

                //var usuarioActualizado = _usuarioPorId.ObtenerUsuarioPorId(userData.Id);
                //if (usuarioActualizado != null && usuarioActualizado.Email != userData.Email)
                //{

                //	usuarioActualizado.ConfirmacionEmail = false;
                //	usuarioActualizado.Email = userData.Email;
                //	_context.Usuarios.Update(usuarioActualizado);
                //	await _context.SaveChangesAsync();
                //	await _emailService.SendEmailAsyncRegister(new DTOEmail
                //                {
                //                    ToEmail = userData.Email
                //                });
                //            }
                //            else
                //{
                //                usuarioUpdate.Email = userData.Email;
                //            }
                var emailActualizado = await _actualizacionYEnvioDeCorreoElectronico.ActualizarEmailUsuario(userData.Id, userData.Email);
                if (emailActualizado)
                {
                    await _actualizacionYEnvioDeCorreoElectronico.EnviarCorreoElectronico(userData.Email);
                }
                else
                {
                    return BadRequest("El email no puede ser el mismo si lo va a cambiar");
                }



                return Ok("Datos actualizados con exito");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al procesar de eliminación actualización de usuario");
                return BadRequest("En estos momentos no se ha podido actualizar el usuario, por favor, intentelo más tarde.");
            }


        }
    }
}
