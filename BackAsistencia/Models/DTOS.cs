namespace BackAsistencia.Models
{


    public class CrearAsistenciaDTO
    {
        public int IdMateriaSalon { get; set; }
        public int NumeroControl { get; set; }
        public DateOnly Fecha { get; set; }
        public TimeOnly Hora { get; set; }
    }

    //public class AsistenciaDTO
    //{
    //    public int IdAsistencia { get; set; }
    //    public DateOnly Fecha { get; set; }
    //    public TimeOnly Hora { get; set; }

    //    public MateriaSalonDTO MateriaSalon { get; set; } = null!;
    //    public AlumnoDTO Alumno { get; set; } = null!;
    //}
    //public class MateriaSalonDTO
    //{
    //    public int IdMateriaSalon { get; set; }
    //    public string MateriaNombre { get; set; } = string.Empty;
    //    public string SalonNombre { get; set; } = string.Empty;
    //}


    public class AlumnoDTO

    {
    public int NumeroControl { get; set; }

    public string Nombre { get; set; } = null!;

    public string Carrera { get; set; } = null!;

    public string Semestre { get; set; } = null!;

    public int IdHorario { get; set; }

    public string Contrasena { get; set; } = null!;
    }
        }