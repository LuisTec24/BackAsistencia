using System;
using System.Collections.Generic;

namespace BackAsistencia.Models;

public partial class HistorialHorarioSemestre
{
    public DateOnly FechaFinSemestre { get; set; }

    public DateOnly FechaInicioSemestre { get; set; }

    public int NumeroDeControl { get; set; }

    public int IdHorario { get; set; }
}
