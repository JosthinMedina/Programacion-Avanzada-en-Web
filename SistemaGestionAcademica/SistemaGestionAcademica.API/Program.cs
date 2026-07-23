using SistemaGestionAcademica.API.Interfaces;
using SistemaGestionAcademica.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddScoped<IEstudianteService, EstudianteService>();
builder.Services.AddScoped<IProfesorService, ProfesorService>();

builder.Services.AddScoped<ICursoService, CursoService>();

builder.Services.AddScoped<IMatriculaService, MatriculaService>();

builder.Services.AddScoped<ICalificacionService, CalificacionService>();

builder.Services.AddScoped<IAsistenciaService, AsistenciaService>();

builder.Services.AddScoped<IEventoService, EventoService>();

builder.Services.AddScoped<INotificacionService, NotificacionService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
