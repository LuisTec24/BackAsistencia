using System;
using System.Collections.Generic;

namespace BackAsistencia.Models;

public partial class Salon
{
    public int IdSalon { get; set; }

    public int? IdEscaner { get; set; }

    public string? Descripcion { get; set; }

    public virtual ICollection<MateriaSalon> MateriaSalons { get; set; } = new List<MateriaSalon>();
}
