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
    public int NumeroControl { get; set; }

    public string Nombre { get; set; } = null!;

    public string Carrera { get; set; } = null!;

    public string Semestre { get; set; } = null!;

    public string Contrasena { get; set; } = null!;

    }

    public class LoginDTO
    {
        public int NC { get; set; }
        public string PasswordHash { get; set; }
    }


    public class LoginDTOM
    {
        public string Correo { get; set; }
        public string PasswordHash { get; set; }
    }

}