using GestionReportes.Data;
using GestionReportes.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GestionReportes.Data;
using GestionReportes.Models;
using GestionReportes.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

        // Crear Reporte
        [HttpPost("crear-reporte")]
        public async Task<IActionResult> CrearReporte([FromBody] CrearReporte dto)
        {
            try
            {
                var tipo = await _context.TiposReporte.FindAsync(dto.TipoId);
                var vecino = await _context.Vecinos.FindAsync(dto.VecinoId);
                var estado = await _context.EstadosReporte.FindAsync(1);
                if (tipo == null || vecino == null || estado == null)
                {
                    return BadRequest("Tipo, Vecino o Estado no encontrado.");
                }
                var reporte = new Reporte
                {
                    descripcion = dto.Descripcion,
                    ubicacion = dto.Ubicacion,
                    imagen = dto.Imagen,
                    Tipo = tipo,//new TipoReporte { id = dto.TipoId },
                    Vecino = vecino,//new Vecino { id = dto.VecinoId },
                    Estado = estado,//new EstadoReporte { id = 1 }, // Estado inicial: Pendiente
                    fechaCreacion = DateTime.UtcNow
                };

                _context.Reportes.Add(reporte);
                await _context.SaveChangesAsync();

                return Ok("Reporte creado exitosamente.");
            }
            catch (Exception ex)
            {
                // Loguear el error
                return StatusCode(500, $"Error al crear el reporte: {ex.Message}");
            }
        }
        //[Authorize]
        // Listar Reportes
        //[HttpGet("lista-reportes")]
        //public async Task<IActionResult> ListarReportes([FromQuery] int? vecinoId, [FromQuery] int? estadoId, [FromQuery] DateTime? fechaInicio, [FromQuery] DateTime? fechaFin)
        //{
        //    try
        //    {
        //        var query = _context.Reportes
        //                    .Include(r => r.Estado)
        //                    .Include(r => r.Tipo)
        //                    .Include(r => r.Vecino)
        //                    .AsQueryable();

        //        if (estadoId.HasValue)
        //            query = query.Where(r => r.Estado.id == estadoId.Value);

        //        if (vecinoId.HasValue)
        //            query = query.Where(r => r.idVecino == vecinoId.Value);

        //        if (fechaInicio.HasValue && fechaFin.HasValue)
        //            query = query.Where(r => r.fechaCreacion >= fechaInicio.Value && r.fechaCreacion <= fechaFin.Value);


        //        var reportes = await query.ToListAsync();

        //        if (!reportes.Any())
        //        {
        //            return NotFound("No se encontraron reportes con los criterios especificados.");
        //        }

        //        return Ok(reportes);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Loguear el error
        //        return StatusCode(500, $"Error al crear el reporte: {ex.Message}");
        //    }
        //}

        //[Authorize]
        // Listar Reportes sin Funcionario Asignado
        [HttpGet("lista-reportes-sin-funcionario")]
        public async Task<IActionResult> ListarReportesSinFuncionario() 
        {
            try
            {
                // Base de la consulta
                var query = _context.Reportes
                    .Include(r => r.Estado)
                    .Include(r => r.Tipo)
                    .Include(r => r.Vecino)
                    .Include(r => r.HistorialReportes) // Relación con el historial
                    .Where(r => !r.HistorialReportes.Any()) // Filtrar reportes sin historial
                    .AsQueryable();
                // Ejecutar la consulta
                var reportes = await query.ToListAsync();

                // Validar si no se encontraron reportes
                if (!reportes.Any())
                {
                    return NotFound("No se encontraron reportes sin funcionario asignado.");
                }

                // Retornar la lista de reportes
                return Ok(reportes);
            }
            catch (Exception ex)
            {
                // Manejar y registrar errores
                return StatusCode(500, $"Error al listar reportes sin funcionario: {ex.Message}");
            }
        }

        //[Authorize]
        // Consultar Detalles de un Reporte
        [HttpGet("obtener-reporte/{id}")]
        public async Task<IActionResult> ObtenerReporte(int id)
        {
            var query = _context.Reportes
                    .Include(r => r.Estado) // Incluir el estado
                    .Include(r => r.Tipo)   // Incluir el tipo
                    .Include(r => r.HistorialReportes)
                        .ThenInclude(h => h.FuncionarioMunicipal)
                            .ThenInclude(f => f.Usuario) // Incluir el usuario del funcionario
                            .Where(r => r.id == id)
                    .AsQueryable();
            var reporte = await query.Select(r => new
            {
                id = r.id,
                descripcion = r.descripcion,
                direccion = r.ubicacion,
                fechaCreacion = r.fechaCreacion,
                estado = new
                {
                    id = r.Estado.id,
                    nombre = r.Estado.nombre,
                    descripcion = r.Estado.descripcion
                },
                tipo = new
                {
                    id = r.Tipo.id,
                    nombre = r.Tipo.nombre,
                    descripcion = r.Tipo.descripcion
                },
                HistorialReportes = r.HistorialReportes
                        .OrderByDescending(h => h.fecha)
                        .Select(h => new
                        {
                            id = h.id,
                            fecha = h.fecha,
                            observacion = h.observacion,
                            funcionario = h.FuncionarioMunicipal != null
                                ? h.FuncionarioMunicipal.Usuario.nombre
                                : null // Nombre del funcionario si existe
                        }).ToList(),
                UltimaActualizacion = r.HistorialReportes
                        .OrderByDescending(h => h.fecha)
                        .Select(h => h.FuncionarioMunicipal.Usuario.nombre)
                        .FirstOrDefault() // Nombre del último funcionario que actualizó el reporte
            }).FirstOrDefaultAsync();

            if (reporte == null)
                return NotFound("Reporte no encontrado.");

            return Ok(reporte);
        }

        //[Authorize]
        [HttpPost("actualizar-reporte")]
        public async Task<IActionResult> ActualizarEstadoReporte([FromBody] ActualizarEstado dto)
        {
            try
            {
                int idFuncionario = dto.FuncionarioId;//int.Parse(User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value);// ID del usuario autenticado
                var reporte = await _context.Reportes
                                    .Include(r => r.Estado) // Asegúrate de incluir la relación Estado
                                    .FirstOrDefaultAsync(r => r.id == dto.ReporteId);
                if (reporte == null)
                    return NotFound("Reporte no encontrado.");
                var funcionario = await _context.FuncionariosMunicipal.FindAsync(idFuncionario);
                if (funcionario == null)
                    return NotFound("funcionario no encontrado.");

                // Obtener el nuevo Estado
                var nuevoEstado = await _context.EstadosReporte.FirstOrDefaultAsync(e => e.id == dto.EstadoId);
                if (nuevoEstado == null)
                    return NotFound("Estado no encontrado.");

                // Asociar el nuevo Estado al Reporte
                reporte.Estado = nuevoEstado;

                // Registrar en HistorialReporte
                var historial = new HistorialReporte
                {
                    Reporte = reporte,
                    FuncionarioMunicipal = funcionario, 
                    fecha = DateTime.UtcNow,
                    observacion = dto.Observacion
                };

                _context.HistorialReportes.Add(historial); // Agregar el historial al contexto
                await _context.SaveChangesAsync(); // Guardar los cambios en la base de datos

                return Ok("Estado del reporte actualizado y registrado en el historial.");
                }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar el estado del reporte: {ex.Message}");
            }
        }

        [HttpGet("lista-reportes")]
        public async Task<IActionResult> ListarReportes(
            [FromQuery] int? vecinoId,
            [FromQuery] int? funcionarioId,
            [FromQuery] int? estadoId,
            [FromQuery] DateTime? fechaInicio,
            [FromQuery] DateTime? fechaFin)
        {
            try
            {
                // Crear la consulta base
                var query = _context.Reportes
                    .Include(r => r.Estado) // Incluir el estado
                    .Include(r => r.Tipo)   // Incluir el tipo
                    .Include(r => r.HistorialReportes)
                        .ThenInclude(h => h.FuncionarioMunicipal)
                            .ThenInclude(f => f.Usuario) // Incluir el usuario del funcionario
                    .AsQueryable();

                // Aplicar filtros dinámicos
                if (estadoId.HasValue)
                    query = query.Where(r => r.Estado.id == estadoId.Value);

                if (vecinoId.HasValue)
                    query = query.Where(r => r.Vecino.id == vecinoId);

                if (funcionarioId.HasValue)
                    query = query.Where(r => r.HistorialReportes.Any(h => h.FuncionarioMunicipal.id == funcionarioId));

                if (fechaInicio.HasValue && fechaFin.HasValue)
                    query = query.Where(r => r.fechaCreacion >= fechaInicio.Value && r.fechaCreacion <= fechaFin.Value);

                // Proyectar los datos en un objeto anónimo
                var reportes = await query.Select(r => new
                {
                    id = r.id,
                    descripcion = r.descripcion,
                    direccion = r.ubicacion,
                    fechaCreacion = r.fechaCreacion,
                    estado = new
                    {
                        id = r.Estado.id,
                        nombre = r.Estado.nombre,
                        descripcion = r.Estado.descripcion
                    },
                    tipo = new
                    {
                        id = r.Tipo.id,
                        nombre = r.Tipo.nombre,
                        descripcion = r.Tipo.descripcion
                    },
                    HistorialReportes = r.HistorialReportes
                        .OrderByDescending(h => h.fecha)
                        .Select(h => new
                        {
                            id = h.id,
                            fecha = h.fecha,
                            observacion = h.observacion,
                            funcionario = h.FuncionarioMunicipal != null
                                ? h.FuncionarioMunicipal.Usuario.nombre
                                : null // Nombre del funcionario si existe
                        }).ToList(),
                    UltimaActualizacion = r.HistorialReportes
                        .OrderByDescending(h => h.fecha)
                        .Select(h => h.FuncionarioMunicipal.Usuario.nombre)
                        .FirstOrDefault() // Nombre del último funcionario que actualizó el reporte
                }).ToListAsync();

                // Validar si se encontraron reportes
                if (!reportes.Any())
                    return NotFound("No se encontraron reportes con los criterios especificados.");

                return Ok(reportes);
            }
            catch (Exception ex)
            {
                // Loguear el error para debugging
                return StatusCode(500, $"Error al listar reportes: {ex.Message}");
            }
        }

        [HttpPost("atender-reporte")]
        public async Task<IActionResult> AtenderReporte([FromBody] AtenderReporte dto)
        {
            try
            {               
                // Obtener el funcionario actual (esto puede ser extraído del contexto del usuario autenticado)
                var funcionario = await _context.FuncionariosMunicipal
                    .Include(f => f.Usuario)
                    .FirstOrDefaultAsync(f => f.Usuario.id == dto.FuncionarioId);

                if (funcionario == null)
                {
                    return NotFound("Funcionario no encontrado.");
                }

                // Crear una nueva entrada en el historial
                var historial = new HistorialReporte
                {
                    idReporte = dto.ReporteId,
                    idFuncionario = dto.FuncionarioId,
                    fecha = DateTime.UtcNow,
                    observacion = $"{funcionario.Usuario.nombre} tomó el reporte" // Observación
                };

                // Agregar el historial al reporte
                _context.HistorialReportes.Add(historial);


                // Guardar cambios en la base de datos
                await _context.SaveChangesAsync();

                return Ok("Reporte atendido exitosamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al atender el reporte: {ex.Message}");
            }
        }

        //[HttpPost("subir-documento")]
        //[Consumes("multipart/form-data")]
        //public async Task<IActionResult> SubirDocumento([FromForm] IFormFile archivo)
        //{
        //    if (archivo == null || archivo.Length == 0)
        //    {
        //        return BadRequest("No se ha enviado ningún archivo.");
        //    }

        //    try
        //    {
        //        // Carpeta donde se guardarán los archivos
        //        var carpeta = Path.Combine(Directory.GetCurrentDirectory(), "Archivos");
        //        if (!Directory.Exists(carpeta))
        //        {
        //            Directory.CreateDirectory(carpeta);
        //        }

        //        // Generar un nombre único para el archivo
        //        var nombreArchivo = $"{Guid.NewGuid()}_{archivo.FileName}";
        //        var rutaCompleta = Path.Combine(carpeta, nombreArchivo);

        //        // Guardar el archivo en el servidor físico
        //        using (var stream = new FileStream(rutaCompleta, FileMode.Create))
        //        {
        //            await archivo.CopyToAsync(stream);
        //        }

        //        // Guardar la ruta en la base de datos
        //        var documento = new Documento
        //        {
        //            nombreArchivo = archivo.FileName,
        //            ruta = rutaCompleta,
        //            fechaSubida = DateTime.UtcNow
        //        };

        //        _context.Documentos.Add(documento);
        //        await _context.SaveChangesAsync();

        //        // Devolver el ID del documento guardado
        //        return Ok(new { DocumentoId = documento.id });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Error al subir el archivo: {ex.Message}");
        //    }
        //}
    }
}
