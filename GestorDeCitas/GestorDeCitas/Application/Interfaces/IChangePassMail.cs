using GestorDeCitas.Application.DTOs;

namespace GestorDeCitas.Application.Interfaces
{
    public interface IChangePassMail
    {
        Task ChangePassEnlaceMail(DTOUsuarioChangePasswordMailConEnlace userData);
    }
}
