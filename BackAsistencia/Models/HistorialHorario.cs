namespace BackAsistencia.Models
{
    public class HistorialHorario
    {

        public int IdHorario { get; set; }

        public DateOnly FechaInicioSemestre { get; set; }

        public DateOnly FechaFinSemestre { get; set; }

        
        public virtual ICollection<HorarioMateriaSalon> HorarioMateriaSalons { get; set; } = new List<HorarioMateriaSalon>();


}
}
