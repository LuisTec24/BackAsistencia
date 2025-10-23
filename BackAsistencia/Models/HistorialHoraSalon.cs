using System;
using System.Collections.Generic;

namespace BackAsistencia.Models;

public partial class HistorialHoraSalon
{
    public int IdHorarioMateriaSalon { get; set; }

    public int IdMateria { get; set; }

    public string? HlunJuv { get; set; }

    public string? Hviernes { get; set; }

    public string? Hsabados { get; set; }
}
