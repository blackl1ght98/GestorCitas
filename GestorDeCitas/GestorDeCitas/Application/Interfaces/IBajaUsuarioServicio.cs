using GestorDeCitas.Application.DTOs;

namespace GestorDeCitas.Application.Interfaces
{
    public interface IBajaUsuarioServicio
    {
        Task UserDeregistration(DTOUserDeregistration delete);
    }
}
