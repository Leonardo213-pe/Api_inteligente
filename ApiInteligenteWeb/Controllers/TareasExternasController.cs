using Microsoft.AspNetCore.Mvc;
using ApiInteligenteWeb.DTOs;

namespace ApiInteligenteWeb.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TareasExternasController : ControllerBase
{
    private readonly HttpClient _httpClient;
    
    public TareasExternasController(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");
    }
    
    // GET: api/tareas-externas
    [HttpGet]
    public async Task<IActionResult> GetTareasExternas()
    {
        try
        {
            var response = await _httpClient.GetAsync("todos");
            
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode(503, new { error = "La API externa no está disponible" });
            }
            
            var todos = await response.Content.ReadFromJsonAsync<List<JsonPlaceholderTodo>>();
            
            var resultado = todos?.Select(t => new TareaExternaDto
            {
                ExternalId = t.Id,
                Titulo = t.Title,
                Completado = t.Completed
            }).ToList();
            
            return Ok(resultado);
        }
        catch (HttpRequestException)
        {
            return StatusCode(503, new { error = "La API externa no está disponible" });
        }
    }
    
    // GET: api/tareas-externas/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTareaExterna(int id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"todos/{id}");
            
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return NotFound(new { error = $"No se encontró la tarea externa con ID {id}" });
            }
            
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode(503, new { error = "La API externa no está disponible" });
            }
            
            var todo = await response.Content.ReadFromJsonAsync<JsonPlaceholderTodo>();
            
            if (todo == null)
            {
                return NotFound(new { error = $"No se encontró la tarea externa con ID {id}" });
            }
            
            var resultado = new TareaExternaDto
            {
                ExternalId = todo.Id,
                Titulo = todo.Title,
                Completado = todo.Completed
            };
            
            return Ok(resultado);
        }
        catch (HttpRequestException)
        {
            return StatusCode(503, new { error = "La API externa no está disponible" });
        }
    }
}

// Clase para mapear la respuesta de JSONPlaceholder
public class JsonPlaceholderTodo
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool Completed { get; set; }
}