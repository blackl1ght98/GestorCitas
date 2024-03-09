using GestorDeCitas.Application.DTOs;

namespace GestorDeCitas.Application.Interfaces
{
    public interface IChangeUserDataService
    {
        Task ChangeUserData(DTOChangeUserData changeUserData);
    }
}
