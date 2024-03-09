using GestorDeCitas.Domain.Models;
using GestorDeCitas.Infrastructure.Interfaces;

namespace GestorDeCitas.Infrastructure.Repositories.UpdateOperations
{
    //Hemos creado una interfaz para que el componente sea reutilizable por eso esta clase se ha
    //vinculado a una interfaz
    public class NewStringGuid : INewStringGuid
    {
        //Llamamos a la base de datos para poder usarla

        private readonly AgendaCitaContext _context;
        //Creamos el contructor

        public NewStringGuid(AgendaCitaContext context)
        {
            _context = context;
        }
        //Agregamos el metodo que esta en la interfaz junto a  la tabla usuarios

        public async Task SaveNewStringGuid(Usuario operation)
        {
            //Actualizamos los datos directamente en la tabla usuarios

            _context.Usuarios.Update(operation);
            //Guardamos los cambios

            await _context.SaveChangesAsync();
        }
    }
}
