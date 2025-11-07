namespace BackAsistencia.Models
{


    public class CrearAsistenciaDTO
    {
        public int ID_HorarioMateriaSalon { get; set; }
        public string NumeroControl { get; set; }
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
}