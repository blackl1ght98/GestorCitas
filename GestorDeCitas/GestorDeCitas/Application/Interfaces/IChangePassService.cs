using GestorDeCitas.Application.DTOs;

namespace GestorDeCitas.Application.Interfaces
{
    public interface IChangePassService
    {
        Task ChangePassId(DTOCambioPassPorId userData);
    }
}
