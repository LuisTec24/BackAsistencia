using System;
using System.Collections.Generic;

namespace BackAsistencia.Models;

public partial class Asistencium
{
    public int IdAsistencia { get; set; }

    public int IdMateriaSalon { get; set; }

    public int NumeroControl { get; set; }

    public DateOnly Fecha { get; set; }

    public TimeOnly Hora { get; set; }

    public virtual MateriaSalon IdMateriaSalonNavigation { get; set; } = null!;

    public virtual Alumno NumeroControlNavigation { get; set; } = null!;
}
