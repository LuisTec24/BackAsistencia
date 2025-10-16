using System;
using System.Collections.Generic;

namespace BackAsistencia.Models;

public partial class Horario
{
    public int IdHorario { get; set; }

    public DateOnly FechaInicioSemestre { get; set; }

    public DateOnly FechaFinSemestre { get; set; }

    public virtual ICollection<Alumno> Alumnos { get; set; } = new List<Alumno>();

    public virtual ICollection<HorarioMateriaSalon> HorarioMateriaSalons { get; set; } = new List<HorarioMateriaSalon>();
}
