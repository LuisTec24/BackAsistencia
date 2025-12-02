using System;
using System.Collections.Generic;

namespace BackAsistencia.Models;

public partial class ProfesorMateria
{
    public int IdProfesorMateria { get; set; }

    public int IdProfesor { get; set; }

    public int IdMateriaSalon { get; set; }

    //recibo de MateriaSalon
    public virtual MateriaSalon IdMateriaSalonNavigation { get; set; } = null!;//donde entra la propiedad de la fk

    //recibo de Profesor
    public virtual Profesor IdProfesorNavigation { get; set; } = null!;
}
