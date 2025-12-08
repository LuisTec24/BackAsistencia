namespace BackAsistencia.Models
{


    public class CrearAsistenciaDTO
    {
        public int ID_HorarioMateriaSalon { get; set; }
        public DateOnly Fecha { get; set; }
        public string Horario { get; set; }
        public TimeOnly Hora { get; set; }
        public required string Estatus { get; set; }

    }
    //Update
    public class UpdateAsistenciaDTO
    {
        public string ? Estatus { get; set; }
        public DateOnly Fecha { get; set; }
        public TimeOnly Hora { get; set; }
    }


    // Materia
    public class MateriaDto
            {
                public int IdMateria { get; set; }
                public string Descripcion { get; set; } = null!;
            }

            // Horario
            public class HorarioDto
            {
                public int IdHorario { get; set; }
                public DateOnly FechaInicioSemestre { get; set; }
                public DateOnly FechaFinSemestre { get; set; }
                public string NumeroControl { get; set; } = null!;
            }

            // HorarioMateriaSalon
            public class HorarioMateriaSalonDto
            {
                public int IdHorarioMateriaSalon { get; set; }
                public int IdMateriaSalon { get; set; }
                public int IdHorario { get; set; }
          
            }

            // ProfesorMateria
            public class ProfesorMateriaDto
            {
                public int IdProfesorMateria { get; set; }
                public int IdProfesor { get; set; }
                public int IdMateriaSalon { get; set; }
            }
        
    
    public class SalonDTO
    {
        public int IdSalon { get; set; }

        public int? IdEscaner { get; set; }

        public string? Descripcion { get; set; }

    }


    public class MateriaSalonDTO
    {
        public int IdMateriaSalon { get; set; }

        public int IdMateria { get; set; }

        public int Idsalon { get; set; }

        public string? HlunJuv { get; set; }

        public string? Hviernes { get; set; }

        public string? Hsabados { get; set; }


    }




    public class ScannerMateriaSalon
    {
        public int IdMateriaSalon { get; set; }

        public string HorarioDia { get; set; } = null;
    
    }

    public class AlumnoDTO

    {
    public string NumeroControl { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string Carrera { get; set; } = null!;

    public string Semestre { get; set; } = null!;

    public string Contrasena { get; set; } = null!;

    }

    public class LoginDTO
    {
        public string NC { get; set; }
        public string PasswordHash { get; set; }
    }

    public class LoginDTOA
    {
        public string Correo { get; set; }
        public string PasswordHash { get; set; }
    }



    public class LoginDTOM
    {
        public string Correo { get; set; }
        public string PasswordHash { get; set; }
    }

    public class ProfesorDTO
    {
        public int IdProfesor { get; set; }

        public string Nombre { get; set; } = null!;

        public string? Departamento { get; set; }

        public string Contrasena { get; set; } = null!;

        public string Correo { get; set; } = null!;
    }

    // > NUEVOS DTO PARA ASISTENCIA Y HORARIO (El back hace el pastel pa que el front solo muestre el pastel WUOOO) < //

    public class HorarioAlumnoDTO
    {
        public string Materia { get; set; }
        public string Salon { get; set; }
        public string Profesor { get; set; }
        public string LunesJueves { get; set; } // HLunJuv
        public string Viernes { get; set; }     // HViernes
        public string Sabado { get; set; }      // HSabados
    }
    public class AsistenciaAlumnoDTO
    {
        public string Materia { get; set; }
        public string Hora { get; set; }    // Ej: "08:30:00"
        public string Estatus { get; set; } // "Asistencia", "Falta", "Retardo"
        public DateTime Fecha { get; set; } // Para saber si es de hoy
    }
    public class GrupoDocenteDTO
    {
        public int IdMateriaSalon { get; set; } // La llave para buscar asistencias
        public string Materia { get; set; }
        public string Salon { get; set; }
        public string Horario { get; set; } // Texto tipo "L-J 7-8"
    }
    public class AsistenciaDocenteItemDTO
    {
        public string NumeroControl { get; set; }
        public string NombreAlumno { get; set; }
        public string Estatus { get; set; } // "Asistencia", "Falta"
        public string Hora { get; set; }
    }



}