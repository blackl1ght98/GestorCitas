using GestorDeCitas.Application.DTOs;

namespace GestorDeCitas.Application.Interfaces
{
    public interface IDeleteUserService
    {
        Task DeleteUser(DTODeleteUser delete);
    }
}
