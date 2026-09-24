using Microsoft.EntityFrameworkCore;
using UTNConecta.API.Data;
using UTNConecta.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Servicios
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddScoped<CarreraService>();
builder.Services.AddScoped<HabilidadService>();
builder.Services.AddScoped<GraduadoService>();



// Conexión a PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();