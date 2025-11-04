using System;
using System.Collections.Generic;
using BackAsistencia.Models;

namespace BackAsistencia.Models;

public partial class Alumno
{
    public string NumeroControl { get; set; }

    public string Nombre { get; set; } = null!;

    public string Carrera { get; set; } = null!;

    public string Semestre { get; set; } = null!;

    public string Contrasena { get; set; } = null!;

    public virtual ICollection<Asistencia> Asistencia { get; set; } = new List<Asistencia>();

    public virtual ICollection<Horario> Horarios { get; set; } = new List<Horario>();
}