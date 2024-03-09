using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace GestorDeCitas.Application.Validators
{
    public class EmailValidation : ValidationAttribute
    {
        //Patron que tiene que tener el email para que sea valido

        private readonly string pattern = @"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$";
        //Constructor
        public EmailValidation()
        {

        }
        //Metodo interno de ValidationAttribute

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            //Vemos si el email su valor es de tipo string

            string email = value as string;
            //Si el usuario no rellena el email le sale este mensaje

            if (email == null)
            {
                return new ValidationResult($"El email debe estar presente");
            }
            else
            {
                //Si esta rellenado comprueba que cumpla con el patron

                Regex regex = new Regex(pattern);

                if (regex.IsMatch(email))
                {
                    return ValidationResult.Success;
                }
                else
                {
                    //Si no cumple muestra este mensaje

                    return new ValidationResult($"El formato del email no es válido");
                }
            }
        }
    }
}
