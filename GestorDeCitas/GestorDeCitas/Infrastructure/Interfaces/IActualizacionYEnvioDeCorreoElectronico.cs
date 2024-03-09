namespace GestorDeCitas.Infrastructure.Interfaces
{
	public interface IActualizacionYEnvioDeCorreoElectronico
	{
		Task<bool> ActualizarEmailUsuario(int userId, string nuevoEmail);
		Task<bool> EnviarCorreoElectronico(string nuevoEmail);
	}
}
