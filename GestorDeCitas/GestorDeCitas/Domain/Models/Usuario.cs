using System;
using System.Collections.Generic;

namespace GestorDeCitas.Domain.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public byte[] Salt { get; set; } = null!;

    public string Rol { get; set; } = null!;

    public bool ConfirmacionEmail { get; set; }

    public bool BajaUsuario { get; set; }

    public string? EnlaceCambioPass { get; set; }

    public DateTime? FechaEnlaceCambioPass { get; set; }

    public string NombreCompleto { get; set; } = null!;

    public DateTime? FechaNacimiento { get; set; }

    public string? Telefono { get; set; }

    public string Direccion { get; set; } = null!;

    public DateTime? FechaRegistro { get; set; }

    public virtual ICollection<Citum> Cita { get; set; } = new List<Citum>();
}
