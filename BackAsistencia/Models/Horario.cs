using System;
using System.Collections.Generic;

namespace BackAsistencia.Models;

public partial class Horario
{
    public int IdHorario { get; set; }

    public DateOnly FechaInicioSemestre { get; set; }

    public DateOnly FechaFinSemestre { get; set; }

    public int? NumeroControl { get; set; }

    public virtual ICollection<HorarioMateriaSalon> HorarioMateriaSalons { get; set; } = new List<HorarioMateriaSalon>();

    public virtual Alumno? NumeroControlNavigation { get; set; }
}
