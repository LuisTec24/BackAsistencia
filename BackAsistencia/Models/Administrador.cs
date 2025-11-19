using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BackAsistencia.Models;

namespace BackAsistencia.Models;

public partial class Administrador
{   
   
        [MaxLength(100)]
        public required string Nombre { get; set; }
        [Key]
        [MaxLength(150)]
        public required string Correo { get; set; }

        [MaxLength(256)]
        public required string Contraseña { get; set; }
   
}

