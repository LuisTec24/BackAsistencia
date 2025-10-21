using BackAsistencia.Models; // Asegúrate de tener el namespace correcto
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Registrar el contexto de base de datos
builder.Services.AddDbContext<ControlAsistenciasContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AppCon")));

// Agregar servicios al contenedor
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
// Configurar el pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();