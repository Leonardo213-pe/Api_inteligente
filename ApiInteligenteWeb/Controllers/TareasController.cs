using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiInteligenteWeb.Data;
using ApiInteligenteWeb.Models;

namespace ApiInteligenteWeb.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TareasController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    
    public TareasController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    // GET: api/tareas
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Tarea>>> GetTareas(
        [FromQuery] string? estado,
        [FromQuery] string? prioridad,
        [FromQuery] DateTime? fechaInicio,
        [FromQuery] DateTime? fechaFin)
    {
        var query = _context.Tareas.AsQueryable();
        
        // Validación: fechaInicio no puede ser mayor que fechaFin
        if (fechaInicio.HasValue && fechaFin.HasValue && fechaInicio > fechaFin)
        {
            return BadRequest(new { error = "La fecha de inicio no puede ser mayor que la fecha de fin" });
        }
        
        // Filtrar por Estado
        if (!string.IsNullOrEmpty(estado))
        {
            var estadosValidos = new[] { "Pendiente", "EnProceso", "Completada" };
            if (!estadosValidos.Contains(estado))
            {
                return BadRequest(new { error = "Estado no válido. Valores permitidos: Pendiente, EnProceso, Completada" });
            }
            query = query.Where(t => t.Estado == estado);
        }
        
        // Filtrar por Prioridad
        if (!string.IsNullOrEmpty(prioridad))
        {
            var prioridadesValidas = new[] { "Baja", "Media", "Alta" };
            if (!prioridadesValidas.Contains(prioridad))
            {
                return BadRequest(new { error = "Prioridad no válida. Valores permitidos: Baja, Media, Alta" });
            }
            query = query.Where(t => t.Prioridad == prioridad);
        }
        
        // Filtrar por rango de fechas
        if (fechaInicio.HasValue)
        {
            query = query.Where(t => t.FechaVencimiento >= fechaInicio.Value);
        }
        
        if (fechaFin.HasValue)
        {
            query = query.Where(t => t.FechaVencimiento <= fechaFin.Value);
        }
        
        return await query.ToListAsync();
    }
    
    // GET: api/tareas/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Tarea>> GetTarea(int id)
    {
        var tarea = await _context.Tareas.FindAsync(id);
        
        if (tarea == null)
        {
            return NotFound();
        }
        
        return tarea;
    }
    
    // POST: api/tareas
    [HttpPost]
    public async Task<ActionResult<Tarea>> PostTarea(Tarea tarea)
    {
        // Validación: FechaVencimiento no menor a hoy
        if (tarea.FechaVencimiento < DateTime.Now.Date)
        {
            return BadRequest(new { error = "La fecha de vencimiento no puede ser menor a la fecha actual" });
        }
        
        // Validación: Estado válido
        var estadosValidos = new[] { "Pendiente", "EnProceso", "Completada" };
        if (!estadosValidos.Contains(tarea.Estado))
        {
            return BadRequest(new { error = "Estado no válido. Valores permitidos: Pendiente, EnProceso, Completada" });
        }
        
        // Validación: Prioridad válida
        var prioridadesValidas = new[] { "Baja", "Media", "Alta" };
        if (!prioridadesValidas.Contains(tarea.Prioridad))
        {
            return BadRequest(new { error = "Prioridad no válida. Valores permitidos: Baja, Media, Alta" });
        }
        
        _context.Tareas.Add(tarea);
        await _context.SaveChangesAsync();
        
        return CreatedAtAction(nameof(GetTarea), new { id = tarea.Id }, tarea);
    }
    
    // PUT: api/tareas/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTarea(int id, Tarea tarea)
    {
        if (id != tarea.Id)
        {
            return BadRequest(new { error = "El ID de la URL no coincide con el ID de la tarea" });
        }
        
        if (tarea.FechaVencimiento < DateTime.Now.Date)
        {
            return BadRequest(new { error = "La fecha de vencimiento no puede ser menor a la fecha actual" });
        }
        
        var estadosValidos = new[] { "Pendiente", "EnProceso", "Completada" };
        if (!estadosValidos.Contains(tarea.Estado))
        {
            return BadRequest(new { error = "Estado no válido" });
        }
        
        var prioridadesValidas = new[] { "Baja", "Media", "Alta" };
        if (!prioridadesValidas.Contains(tarea.Prioridad))
        {
            return BadRequest(new { error = "Prioridad no válida" });
        }
        
        _context.Entry(tarea).State = EntityState.Modified;
        
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Tareas.Any(e => e.Id == id))
            {
                return NotFound();
            }
            throw;
        }
        
        return NoContent();
    }
    
    // DELETE: api/tareas/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTarea(int id)
    {
        var tarea = await _context.Tareas.FindAsync(id);
        if (tarea == null)
        {
            return NotFound();
        }
        
        _context.Tareas.Remove(tarea);
        await _context.SaveChangesAsync();
        
        return NoContent();
    }
}