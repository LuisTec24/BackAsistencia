using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using BackAsistencia.Models;

namespace BackAsistencia.Models;

public partial class ControlAsistenciasContext : DbContext
{
    public ControlAsistenciasContext()
    {
    }

    public ControlAsistenciasContext(DbContextOptions<ControlAsistenciasContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Alumno> Alumnos { get; set; }

    public virtual DbSet<Asistencia> Asistencia { get; set; }

    public virtual DbSet<HistorialHoraSalon> HistorialHoraSalons { get; set; }

    public virtual DbSet<HistorialHorarioSemestre> HistorialHorarioSemestres { get; set; }

    public virtual DbSet<HistorialMateriaSalon> HistorialMateriaSalons { get; set; }

    public virtual DbSet<Horario> Horarios { get; set; }

    public virtual DbSet<HorarioMateriaSalon> HorarioMateriaSalons { get; set; }

    public virtual DbSet<MateriaSalon> MateriaSalons { get; set; }

    public virtual DbSet<Materia> Materia { get; set; }

    public virtual DbSet<Profesor> Profesors { get; set; }

    public virtual DbSet<ProfesorMateria> ProfesorMateria { get; set; }

    public virtual DbSet<Salon> Salons { get; set; }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //    => optionsBuilder.UseSqlServer("Server=LUIS\\SQLEXPRESS; DataBase=ControlAsistencias;Integrated Security=true; TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Alumno>(entity =>
        {
            entity.HasKey(e => e.NumeroControl).HasName("PK_Alumno");

            entity.ToTable("Alumno");

            entity.Property(e => e.NumeroControl).ValueGeneratedNever();
            entity.Property(e => e.Carrera).HasMaxLength(100);
            entity.Property(e => e.Contrasena).HasMaxLength(512);
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Semestre).HasMaxLength(50);
        });

        modelBuilder.Entity<Asistencia>(entity =>
        {
            entity.HasKey(e => e.IdAsistencia).HasName("PK__Asistenc__8A115D6A553CF553");

            entity.Property(e => e.IdAsistencia).HasColumnName("ID_Asistencia");
            entity.Property(e => e.IdMateriaSalon).HasColumnName("ID_MateriaSalon");

            entity.HasOne(d => d.IdMateriaSalonNavigation).WithMany(p => p.Asistencia)
                .HasForeignKey(d => d.IdMateriaSalon)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Asistenci__ID_Ma__5FB337D6");

            entity.HasOne(d => d.NumeroControlNavigation).WithMany(p => p.Asistencia)
                .HasForeignKey(d => d.NumeroControl)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Asistencia_NCAlumno");
        });

        modelBuilder.Entity<HistorialHoraSalon>(entity =>
        {
            entity.HasKey(e => e.IdHorarioMateriaSalon).HasName("PK__Historia__A3A4AA3CB1186F26");

            entity.ToTable("HistorialHoraSalon");

            entity.Property(e => e.IdHorarioMateriaSalon)
                .ValueGeneratedNever()
                .HasColumnName("ID_HorarioMateriaSalon");
            entity.Property(e => e.HlunJuv)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("HLunJuv");
            entity.Property(e => e.Hsabados)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("HSabados");
            entity.Property(e => e.Hviernes)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("HViernes");
            entity.Property(e => e.IdMateria).HasColumnName("ID_Materia");
        });

        modelBuilder.Entity<HistorialHorarioSemestre>(entity =>
        {
            entity.HasKey(e => new { e.FechaFinSemestre, e.NumeroDeControl, e.IdHorario }).HasName("PK__Historia__0874DC85561AEB23");

            entity.ToTable("HistorialHorarioSemestre");

            entity.Property(e => e.NumeroDeControl).HasColumnName("Numero_de_control");
            entity.Property(e => e.IdHorario).HasColumnName("ID_Horario");
        });

        modelBuilder.Entity<HistorialMateriaSalon>(entity =>
        {
            entity.HasKey(e => e.IdMateriaSalon).HasName("PK__Historia__AE21C2ACFD613A54");

            entity.ToTable("HistorialMateriaSalon");

            entity.Property(e => e.IdMateriaSalon)
                .ValueGeneratedNever()
                .HasColumnName("ID_MateriaSalon");
            entity.Property(e => e.IdMateria).HasColumnName("ID_Materia");
            entity.Property(e => e.IdSalon).HasColumnName("ID_Salon");
        });

        modelBuilder.Entity<Horario>(entity =>
        {
            entity.HasKey(e => e.IdHorario).HasName("PK__Horario__77A009BD0301AC9F");

            entity.ToTable("Horario");

            entity.Property(e => e.IdHorario).HasColumnName("ID_Horario");

            entity.HasOne(d => d.NumeroControlNavigation).WithMany(p => p.Horarios)
                .HasForeignKey(d => d.NumeroControl)
                .HasConstraintName("FK_Horario_NCAlumno");
        });

        modelBuilder.Entity<HorarioMateriaSalon>(entity =>
        {
            entity.HasKey(e => e.IdHorarioMateriaSalon).HasName("PK__HorarioM__A3A4AA3CF0E5B5E5");

            entity.ToTable("HorarioMateriaSalon");

            entity.Property(e => e.IdHorarioMateriaSalon).HasColumnName("ID_HorarioMateriaSalon");
            entity.Property(e => e.HlunJuv)
                .HasMaxLength(100)
                .HasColumnName("HLunJuv");
            entity.Property(e => e.Hsabados)
                .HasMaxLength(100)
                .HasColumnName("HSabados");
            entity.Property(e => e.Hviernes)
                .HasMaxLength(100)
                .HasColumnName("HViernes");
            entity.Property(e => e.IdHorario).HasColumnName("ID_Horario");
            entity.Property(e => e.IdMateriaSalon).HasColumnName("ID_MateriaSalon");

            entity.HasOne(d => d.IdHorarioNavigation).WithMany(p => p.HorarioMateriaSalons)
                .HasForeignKey(d => d.IdHorario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HorarioMa__ID_Ho__5CD6CB2B");

            entity.HasOne(d => d.IdMateriaSalonNavigation).WithMany(p => p.HorarioMateriaSalons)
                .HasForeignKey(d => d.IdMateriaSalon)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HorarioMa__ID_Ma__5BE2A6F2");
        });

        modelBuilder.Entity<MateriaSalon>(entity =>
        {
            entity.HasKey(e => e.IdMateriaSalon).HasName("PK__MateriaS__AE21C2AC0A7E53F9");

            entity.ToTable("MateriaSalon");

            entity.Property(e => e.IdMateriaSalon).HasColumnName("ID_MateriaSalon");
            entity.Property(e => e.IdMateria).HasColumnName("ID_Materia");
            entity.Property(e => e.IdSalon).HasColumnName("ID_Salon");
            
            entity.HasOne(d => d.IdMateriaNavigation).WithMany(p => p.MateriaSalons)
                .HasForeignKey(d => d.IdMateria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MateriaSa__ID_Ma__5812160E");

            entity.HasOne(d => d.IdSalonNavigation).WithMany(p => p.MateriaSalons)
                .HasForeignKey(d => d.IdSalon)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MateriaSa__ID_Sa__59063A47");
        });

        modelBuilder.Entity<Materia>(entity =>
        {
            entity.HasKey(e => e.IdMateria).HasName("PK__Materia__4BAC7BD9FE5EA0CF");

            entity.Property(e => e.IdMateria).HasColumnName("ID_Materia");
            entity.Property(e => e.Descripcion).HasMaxLength(100);
        });

        modelBuilder.Entity<Profesor>(entity =>
        {
            entity.HasKey(e => e.IdProfesor).HasName("PK__Profesor__4D3751F6810C80DC");

            entity.ToTable("Profesor");

            entity.Property(e => e.IdProfesor).HasColumnName("ID_Profesor");
            entity.Property(e => e.Contrasena).HasMaxLength(512);
            entity.Property(e => e.Departamento).HasMaxLength(100);
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Correo).HasMaxLength(100);

        });

        modelBuilder.Entity<ProfesorMateria>(entity =>
        {
            entity.HasKey(e => e.IdProfesorMateria).HasName("PK__Profesor__B4B27523EDD4B0EE");

            entity.Property(e => e.IdProfesorMateria).HasColumnName("ID_ProfesorMateria");
            entity.Property(e => e.IdMateria).HasColumnName("ID_Materia");
            entity.Property(e => e.IdProfesor).HasColumnName("ID_Profesor");

            entity.HasOne(d => d.IdMateriaNavigation).WithMany(p => p.ProfesorMateria)
                .HasForeignKey(d => d.IdMateria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProfesorM__ID_Ma__534D60F1");

            entity.HasOne(d => d.IdProfesorNavigation).WithMany(p => p.ProfesorMateria)
                .HasForeignKey(d => d.IdProfesor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProfesorM__ID_Pr__52593CB8");
        });

        modelBuilder.Entity<Salon>(entity =>
        {
            entity.HasKey(e => e.IdSalon).HasName("PK__Salon__853C302EA61E0359");

            entity.ToTable("Salon");

            entity.Property(e => e.IdSalon).HasColumnName("ID_Salon");
            entity.Property(e => e.Descripcion).HasMaxLength(100);
            entity.Property(e => e.IdEscaner).HasColumnName("ID_Escaner");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
