using GestorDeCitas.Application.DTOs;

namespace GestorDeCitas.Application.Interfaces
{
    public interface INuevaCitaService
    {
        Task NuevaCita(DTOCita cita);
    }
}
