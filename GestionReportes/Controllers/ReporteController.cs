using GestionReportes.Data;
using GestionReportes.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GestionReportes.Data;
using GestionReportes.Models;
using GestionReportes.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace GestionReportes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReporteController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ReporteController(AppDbContext context)
        {
            _context = context;
        }
        [Authorize]
        // Crear Reporte
        [HttpPost]
        public async Task<IActionResult> CrearReporte([FromBody] CrearReporte dto)
        {
            var reporte = new Reporte
            {
                Descripcion = dto.Descripcion,
                Ubicacion = dto.Ubicacion,
                Imagen = dto.Imagen,
                idTipo = dto.TipoId,
                idVecino = dto.VecinoId,
                idEstado = 1, // Estado inicial: Pendiente
                FechaCreacion = DateTime.UtcNow
            };

            _context.Reportes.Add(reporte);
            await _context.SaveChangesAsync();

            return Ok("Reporte creado exitosamente.");
        }
        [Authorize]
        // Listar Reportes
        [HttpGet]
        public async Task<IActionResult> ListarReportes([FromQuery] int? estadoId, [FromQuery] DateTime? fechaInicio, [FromQuery] DateTime? fechaFin)
        {
            var query = _context.Reportes.Include(r => r.Estado).Include(r => r.Tipo).AsQueryable();

            if (estadoId.HasValue)
                query = query.Where(r => r.idEstado == estadoId.Value);

            if (fechaInicio.HasValue && fechaFin.HasValue)
                query = query.Where(r => r.FechaCreacion >= fechaInicio.Value && r.FechaCreacion <= fechaFin.Value);

            var reportes = await query.ToListAsync();

            return Ok(reportes);
        }
        [Authorize]
        // Consultar Detalles de un Reporte
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerReporte(int id)
        {
            var reporte = await _context.Reportes
                .Include(r => r.Estado)
                .Include(r => r.Tipo)
                .Include(r => r.Vecino)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reporte == null)
                return NotFound("Reporte no encontrado.");

            return Ok(reporte);
        }
        [Authorize]
        // Actualizar Estado del Reporte
        [HttpPatch("{id}/estado")]
        public async Task<IActionResult> ActualizarEstadoReporte(int id, [FromBody] ActualizarEstado dto)
        {
            var reporte = await _context.Reportes.FirstOrDefaultAsync(r => r.Id == id);
            if (reporte == null)
                return NotFound("Reporte no encontrado.");

            reporte.idEstado = dto.EstadoId;

            // Registrar en HistorialReporte
            var historial = new HistorialReporte
            {
                idReporte = id,
                idFuncionario = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value), // ID del usuario autenticado
                fecha = DateTime.UtcNow,
                observacion = dto.Observacion
            };

            await _context.SaveChangesAsync();

            return Ok("Estado del reporte actualizado.");
        }

        [HttpPost("subir-documento")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> SubirDocumento([FromForm] IFormFile archivo)
        {
            if (archivo == null || archivo.Length == 0)
            {
                return BadRequest("No se ha enviado ningún archivo.");
            }

            try
            {
                // Carpeta donde se guardarán los archivos
                var carpeta = Path.Combine(Directory.GetCurrentDirectory(), "Archivos");
                if (!Directory.Exists(carpeta))
                {
                    Directory.CreateDirectory(carpeta);
                }

                // Generar un nombre único para el archivo
                var nombreArchivo = $"{Guid.NewGuid()}_{archivo.FileName}";
                var rutaCompleta = Path.Combine(carpeta, nombreArchivo);

                // Guardar el archivo en el servidor físico
                using (var stream = new FileStream(rutaCompleta, FileMode.Create))
                {
                    await archivo.CopyToAsync(stream);
                }

                // Guardar la ruta en la base de datos
                var documento = new Documento
                {
                    nombreArchivo = archivo.FileName,
                    ruta = rutaCompleta,
                    fechaSubida = DateTime.UtcNow
                };

                _context.Documentos.Add(documento);
                await _context.SaveChangesAsync();

                // Devolver el ID del documento guardado
                return Ok(new { DocumentoId = documento.id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al subir el archivo: {ex.Message}");
            }
        }
    }
}
