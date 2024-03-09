using GestorDeCitas.Application.Validators;

namespace GestorDeCitas.Application.DTOs
{
    public class DTOChangeUserData
    {
        public int Id { get; set; }
  
        public string NombreCompleto { get; set; }
      
        public string Email { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string Telefono { get; set; }
        public string Direccion {  get; set; }
	

    }
}
