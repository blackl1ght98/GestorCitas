using System;
using System.Collections.Generic;

namespace GestorDeCitas.Domain.Models;

public partial class Citum
{
    public int Id { get; set; }

    public DateTime FechaYhora { get; set; }

    public string MotivoCita { get; set; } = null!;

    public string UbicacionCita { get; set; } = null!;

    public string DuracionEstimada { get; set; } = null!;

    public string NombreDelProfesional { get; set; } = null!;

    public string? NotasAdicionales { get; set; }

    public string? EstadoCita { get; set; }

    public int IdUsuario { get; set; }

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
