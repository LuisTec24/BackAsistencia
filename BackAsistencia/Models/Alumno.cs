using System;
using System.Collections.Generic;

namespace BackAsistencia.Models;

public partial class Alumno
{
    public int NumeroControl { get; set; }

    public string Nombre { get; set; } = null!;

    public string Carrera { get; set; } = null!;

    public string Semestre { get; set; } = null!;

    public int IdHorario { get; set; }

    public string Contrasena { get; set; } = null!;

    public virtual ICollection<Asistencium> Asistencia { get; set; } = new List<Asistencium>();

    public virtual Horario IdHorarioNavigation { get; set; } = null!;
}
