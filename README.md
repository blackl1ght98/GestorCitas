Frontend

El frontend de la aplicación está desarrollado con el popular framework Angular. Este proporciona una interfaz de usuario rica y dinámica, permitiendo a los usuarios interactuar con la aplicación de manera eficiente. Las características clave del frontend incluyen:

<p><strong>Registro de usuario:</strong> Los nuevos usuarios pueden crear una cuenta proporcionando los detalles necesarios.</p>
<p><strong>Inicio de sesión:</strong> Los usuarios registrados pueden iniciar sesión en la aplicación utilizando sus credenciales.</p>
<p><strong>Recuperación de contraseña:</strong> Si un usuario olvida su contraseña, puede solicitar un correo electrónico de recuperación para restablecerla.</p>
<p><strong>Cambio de contraseña:</strong> Los usuarios pueden cambiar su contraseña después de iniciar sesión en la aplicación.</p>
<p><strong>Gestión de citas:</strong> Los usuarios pueden agregar y eliminar citas según sus necesidades.</p>
Backend

El backend de la aplicación está construido con .NET 7.0, un marco de trabajo robusto y escalable que permite el desarrollo de aplicaciones de alto rendimiento. Las funcionalidades principales del backend incluyen:

   <p> Autenticación de usuario: El backend maneja el inicio de sesión y el registro de los usuarios, asegurando que solo los usuarios autorizados puedan acceder a la aplicación.</p>
    <p><strong>Gestión de usuarios:</strong> Los usuarios pueden ser eliminados y sus datos pueden ser recuperados según sea necesario.</p>
   <p><strong>Generación de tokens:</strong> Para mantener las sesiones de los usuarios y permitir la autenticación, el backend genera tokens.</p>
   <p> <strong>Hashing de contraseñas:</strong> Para garantizar la seguridad de las contraseñas de los usuarios, estas se cifran utilizando un algoritmo de hashing.</p>
Como poner en funcionamiento el bakend:
1º Ejecutar el comando scaffold: Scaffold-DbContext "Data Source=GUILLERMO\SQLEXPRESS;Initial Catalog=AgendaCita;Integrated Security=True;TrustServerCertificate=True" -Provider Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -force -project GestorDeCitas
2º Poner en el archivo de secretos: ClaveJWT y CONNECTION_STRING, 
ClaveJWT-> tendra un valor largo
CONNECTION_STRING-> cadena de conexion de BD
