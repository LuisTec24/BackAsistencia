using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BackAsistencia.Models;

public partial class Asistencia
{
    [Key]
    public int IdAsistencia { get; set; }

    public int ID_HorarioMateriaSalon { get; set; }
    public string Estatus { get; set; } = null!;

    public DateOnly Fecha { get; set; }

    public TimeOnly Hora { get; set; }


    public virtual HorarioMateriaSalon ID_HorarioMateriaSalonNavigation { get; set; } = null!;// vendria a ser la referencia de la fk

}
