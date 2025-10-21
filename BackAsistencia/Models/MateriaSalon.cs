using System;
using System.Collections.Generic;

namespace BackAsistencia.Models;

public partial class MateriaSalon
{
    public int IdMateriaSalon { get; set; }

    public int IdMateria { get; set; }

    public int IdSalon { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<Asistencia> Asistencia { get; set; } = new List<Asistencia>();

    public virtual ICollection<HorarioMateriaSalon> HorarioMateriaSalons { get; set; } = new List<HorarioMateriaSalon>();

    public virtual Materium IdMateriaNavigation { get; set; } = null!;

    public virtual Salon IdSalonNavigation { get; set; } = null!;
}
