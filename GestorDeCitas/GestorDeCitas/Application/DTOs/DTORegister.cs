using GestorDeCitas.Application.Validators;

namespace GestorDeCitas.Application.DTOs
{
    public class DTORegister
    {
       
       
        [EmailValidation]
        public string Email { get; set; }
        [PassValidation]
        public string Password { get; set; }
        public string NombreCompleto { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string Telefono {  get; set; }
        public string Direccion { get; set; }
        public DateTime? FechaRegistro { get; set; }
    
       
      
        
       
    }
}
