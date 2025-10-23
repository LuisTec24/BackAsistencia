using System;
using System.Collections.Generic;

namespace BackAsistencia.Models;

public partial class Materia
{
    public int IdMateria { get; set; }

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<MateriaSalon> MateriaSalons { get; set; } = new List<MateriaSalon>();

    public virtual ICollection<ProfesorMateria> ProfesorMateria { get; set; } = new List<ProfesorMateria>();
}
