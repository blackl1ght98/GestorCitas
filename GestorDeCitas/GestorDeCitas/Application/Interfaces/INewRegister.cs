using GestorDeCitas.Application.DTOs;

namespace GestorDeCitas.Application.Interfaces
{
    public interface INewRegister
    {
        Task NewRegister(DTORegister userData);
    }
}
