using Azure;
using DiabetesNoteBook.Application.Services;
using GestorDeCitas.Application.DTOs;
using GestorDeCitas.Application.Interfaces;
using GestorDeCitas.Application.Services.Genereics;
using GestorDeCitas.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestorDeCitas.Infrastructure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChangePasswordController : ControllerBase
    {
        private readonly AgendaCitaContext _context;
        private readonly HashService _hashService;
        private readonly TokenService _tokenService;
        private readonly IEmailService _emailService;
        private readonly ExistUsersService _existUsersService;
        private readonly ILogger<UsersController> _logger;
        private readonly IChangePassService _changePassService;
        private readonly IChangePassMail _changePassMail;
        public ChangePasswordController(AgendaCitaContext context, HashService hashService, TokenService tokenService, IEmailService emailService,
            ExistUsersService existUsersService, ILogger<UsersController> logger, IChangePassService changePassService, IChangePassMail changePassMail)
        {
            _context = context;
            _hashService = hashService;
            _tokenService = tokenService;
            _emailService = emailService;
            _existUsersService = existUsersService;
            _logger = logger;
            _changePassService = changePassService;
            _changePassMail = changePassMail;
        }
        [Authorize]
        [HttpPut("changePassword")]
        public async Task<ActionResult> ChangePassword([FromBody] DTOCambioPassPorId userData)
        {
            try
            {
                var usuarioDB = await _existUsersService.UserExistById(userData.Id);

               

                if (usuarioDB == null)
                {
                    return Unauthorized("Operación no autorizada");
                }
               
                var resultadoHash = _hashService.Hash(userData.NewPass, usuarioDB.Salt);
             
                if (usuarioDB.Password == resultadoHash.Hash)
                {
                    return Unauthorized("La nueva contraseña no puede ser la misma.");
                }
              
                await _changePassService.ChangePassId(new DTOCambioPassPorId
                {
                    Id = userData.Id,
                    NewPass = userData.NewPass
                });
                //Agregamos la operacion


                //Si todo va bien se devuelve un ok

                return Ok("Password cambiado con exito");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al procesar al cambiar la contraseña");
                return BadRequest("En estos momentos no se ha podido realizar el cambio de contraseña, por favor, intentelo más tarde.");
            }

        }
        [AllowAnonymous]
        [HttpPost("changePasswordMail")]
        public async Task<ActionResult<Response>> SendInstruction(DTOUsuarioChangePasswordMail usuario)
        {
            try
            {
                var usuarioDB = await _existUsersService.EmailExist(usuario.Email);

                if (usuarioDB is false)
                {
                    return Unauthorized("Este email no se encuentra registrado.");
                }

                var usuarioDBEmail = await _existUsersService.UserExistByEmail(usuario.Email);

                if (usuarioDBEmail is false)
                {
                    return Unauthorized("Usuario dado de baja con anterioridad");
                }

                if (usuarioDB != null)
                {

                    await _emailService.SendEmailAsyncChangePassword(new DTOEmail
                    {
                        ToEmail = usuario.Email
                    });

                    return Ok("Se enviaron las instrucciones a tu correo para restablecer la contraseña. Recuerda revisar la bandeja de Spam.");

                }

                return BadRequest("Email no encontrado.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al procesar el envio de instrucciones");
                return BadRequest("En este momento no puedo enviar las intrucciones, intentalo más tarde por favor.");
            }
        }
        [AllowAnonymous]
        [HttpPost("changePasswordMailConEnlace")]
        public async Task<ActionResult<Response>> Reset([FromBody] DTOUsuarioChangePasswordMailConEnlace cambiopass)
        {
            try
            {
                var userTokenExiste = await _existUsersService.UserTokenExist(cambiopass);

                
                var resultadoHash = _hashService.Hash(cambiopass.NewPass, userTokenExiste.Salt);
              
                if (userTokenExiste.Password == resultadoHash.Hash)
                {
                    return Unauthorized("La nueva contraseña no puede ser la misma.");
                }
                //Se genera una fecha

                DateTime fecha = DateTime.Now;
                //Si el token generado en el endpoint anterior existe y el token que se ha generado es mayoor o
                //igual a la fecha actual se genera
                if (userTokenExiste.EnlaceCambioPass != null && userTokenExiste.FechaEnlaceCambioPass >= fecha)
                {
                    //Se llama al servicio _changePassMail este servicio tiene un metodo ChangePassEnlaceMail
                    //el cual tiene un DTOUsuarioChangePasswordMailConEnlace que contiene los datos necesarios
                    //para cambiar el email
                    await _changePassMail.ChangePassEnlaceMail(new DTOUsuarioChangePasswordMailConEnlace
                    {
                        NewPass = cambiopass.NewPass,
                        Token = cambiopass.Token

                    });
                    //Si todo ha ido bien devuelve un ok

                    return Ok("Password cambiado con exito");
                }
                //Se agrega la operacion


                //Si el token a caducado o no existe muestra este mensaje

                return Ok("El token no existe o ha caducado.");

            }
            catch (Exception ex)
            {
                //Este mensaje se muestra si ocurreo algun otro error con el servidor
                _logger.LogError(ex + "Error al procesar el reset del email.");
                return BadRequest("En este momento no se puede actualizar tu contraseña, intentelo más tarde por favor.");
            }
        }
    }
}
