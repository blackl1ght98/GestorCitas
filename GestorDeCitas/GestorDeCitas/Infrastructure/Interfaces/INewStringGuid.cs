using GestorDeCitas.Domain.Models;

namespace GestorDeCitas.Infrastructure.Interfaces
{
    public interface INewStringGuid
    {
        Task SaveNewStringGuid(Usuario operation);
    }
}
