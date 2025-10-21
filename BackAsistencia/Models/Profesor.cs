using System;
using System.Collections.Generic;

namespace BackAsistencia.Models;

public partial class Profesor
{
    public int IdProfesor { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Departamento { get; set; }

    public string Contrasena { get; set; } = null!;

    public virtual ICollection<ProfesorMateria> ProfesorMateria { get; set; } = new List<ProfesorMateria>();
}
