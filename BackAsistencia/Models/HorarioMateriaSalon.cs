using System;
using System.Collections.Generic;

namespace BackAsistencia.Models;

public partial class HorarioMateriaSalon
{
    public int IdHorarioMateriaSalon { get; set; }

    public int IdMateriaSalon { get; set; }

    public int IdHorario { get; set; }

    public string? HlunJuv { get; set; }

    public string? Hviernes { get; set; }

    public string? Hsabados { get; set; }

    public virtual Horario IdHorarioNavigation { get; set; } = null!;

    public virtual MateriaSalon IdMateriaSalonNavigation { get; set; } = null!;
}
