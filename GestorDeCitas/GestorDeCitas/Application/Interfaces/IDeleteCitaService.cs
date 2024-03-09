using GestorDeCitas.Application.DTOs;

namespace GestorDeCitas.Application.Interfaces
{
    public interface IDeleteCitaService
    {
        Task DeleteCita(DTOById delete);
    }
}
