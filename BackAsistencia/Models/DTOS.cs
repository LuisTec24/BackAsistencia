namespace BackAsistencia.Models
{


    public class CrearAsistenciaDTO
    {
        public int ID_HorarioMateriaSalon { get; set; }
        public required string NumeroControl { get; set; }
        public DateOnly Fecha { get; set; }
        public TimeOnly Hora { get; set; }
        public required string Estatus { get; set; }

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
                public string? HlunJuv { get; set; }
                public string? Hviernes { get; set; }
                public string? Hsabados { get; set; }
            }

            // ProfesorMateria
            public class ProfesorMateriaDto
            {
                public int IdProfesorMateria { get; set; }
                public int IdProfesor { get; set; }
                public int IdMateria { get; set; }
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

    }




    public class ScannerHorarioMateriaSalon
    {
        public int IdHorarioMateriaSalon { get; set; }

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
    public class AsistenciaItemDTO
    {
        public string Materia { get; set; }
        public DateOnly Fecha { get; set; }
        public TimeOnly Hora { get; set; }
    }
    public class HorarioItemDTO
    {
        public string NombreMateria { get; set; }
        public string Profesor { get; set; }
        public string Salon { get; set; }
        public string HorarioTexto1 { get; set; }
        public string HorarioDias1 { get; set; }
        public string HorarioTexto2 { get; set; }
        public string HorarioDias2 { get; set; }
        public string HorarioTexto3 { get; set; }
        public string HorarioDias3 { get; set; }
    }
    // DTO para el reporte de asistencia que verá el profesor
    public class ClaseConHorarioDTO
    {
        public int IdGrupo { get; set; } // Es el ID_HorarioMateriaSalon
        public string DescripcionCompleta { get; set; } // "Matemáticas - Aula 1 (08:00 - 09:00)"
    }

    // Este se queda igual para el reporte
    public class AsistenciaReporteItemDTO
    {
        public int IdAsistencia { get; set; }
        public string NumeroControl { get; set; }
        public string NombreAlumno { get; set; }
        public string Estatus { get; set; }
    }
}