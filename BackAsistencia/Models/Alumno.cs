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
    //icolleccion envia //virtual recibe
    public virtual ICollection<Horario> Horarios { get; set; } = new List<Horario>(); //relacion Para Fk//pasa propiedad
}