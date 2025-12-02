using System;
using System.Collections.Generic;

namespace BackAsistencia.Models;

public partial class MateriaSalon
{
    public int IdMateriaSalon { get; set; }

    public int IdMateria { get; set; }

    public int IdSalon { get; set; }

    public string? HlunJuv { get; set; }

    public string? Hviernes { get; set; }

    public string? Hsabados { get; set; }

    //envio a HorarioMateriaSalon
    public virtual ICollection<HorarioMateriaSalon> HorarioMateriaSalons { get; set; } = new List<HorarioMateriaSalon>();

    //envio a ProfesorMateria
    public virtual ICollection<ProfesorMateria> ProfesorMateria { get; set; } = new List<ProfesorMateria>();

    //recibo de Materias
    public virtual Materia IdMateriaNavigation { get; set; } = null!;

    //recibo de Salon
    public virtual Salon IdSalonNavigation { get; set; } = null!;
}
