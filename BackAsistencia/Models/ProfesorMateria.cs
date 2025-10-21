using System;
using System.Collections.Generic;

namespace BackAsistencia.Models;

public partial class ProfesorMateria
{
    public int IdProfesorMateria { get; set; }

    public int IdProfesor { get; set; }

    public int IdMateria { get; set; }

    public virtual Materium IdMateriaNavigation { get; set; } = null!;

    public virtual Profesor IdProfesorNavigation { get; set; } = null!;
}
