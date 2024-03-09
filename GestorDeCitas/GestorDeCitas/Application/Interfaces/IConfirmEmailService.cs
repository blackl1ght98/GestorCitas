using GestorDeCitas.Application.DTOs;

namespace GestorDeCitas.Application.Interfaces
{
    public interface IConfirmEmailService
    {
        Task ConfirmEmail(DTOConfirmRegistrtion confirm);
    }
}
