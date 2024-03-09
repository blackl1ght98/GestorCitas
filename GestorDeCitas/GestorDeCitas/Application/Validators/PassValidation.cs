using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace GestorDeCitas.Application.Validators
{
    public class PassValidation : ValidationAttribute
    {
        //Establece un patron a la contraseña

        private readonly string nuevoPatron = @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";
        //Se crea el constructor

        public PassValidation()
        {

        }
        //Metodo interno de ValidationAttribute

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            //vemos si la contraseña es de tipo string

            string password = value as string;
            //si el usuario deja vacio el campo de contraseña muestra este mensaje

            if (password == null)
            {
                return new ValidationResult($"La contraseña debe estar presente");
            }
            else
            {
                //Si el campo de contraseña esta relleno y cumple el patron sigue para adelante 

                Regex regex = new Regex(nuevoPatron);

                if (regex.IsMatch(password))
                {
                    return ValidationResult.Success;
                }
                else
                {
                    //si no cumple devuelve este mensaje

                    return new ValidationResult($"El formato de la contraseña no es válido. Debe cumplir con el nuevo patrón.");
                }
            }
        }
    }
}
