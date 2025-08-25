using System.Data;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Azure.SQLDB.Samples.DynamicSchema;

[ApiController]
[Route("todo/hybrid")]
public class ToDoHybridController(IConfiguration config, ILogger<ToDoHybridController> logger, ToDoContext context) : ControllerBase
{
    private readonly ILogger<ToDoHybridController> _logger = logger;
    private readonly IConfiguration _config = config;
    private readonly ToDoContext _context = context;

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var todos = await _context.ToDo.ToListAsync();
        return new OkObjectResult(todos);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var todo = await _context.ToDo.FindAsync(id);
        if (todo == null) return NotFound();
        return new OkObjectResult(todo);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] ToDo todo)
    {
        await _context.ToDo.AddAsync(todo);
        await _context.SaveChangesAsync();
        return new OkObjectResult(todo);
    }

    [HttpPatch]
    [Route("{id}")]
    public async Task<IActionResult> Patch(int id, [FromBody] ToDo newTodo)
    {
        var existing = await _context.ToDo.FindAsync(id);
        if (existing == null) return NotFound();

        existing.Title = newTodo.Title ?? existing.Title;
        existing.Completed = newTodo.Completed;

        // Merge Extensions using JsonNode
        if (newTodo.Extensions != null)
        {
            // Initialize Extensions if null
            existing.Extensions ??= new ToDoExtension();

            // Create JsonNodes directly from objects (more efficient)
            var existingJson = JsonSerializer.SerializeToNode(existing.Extensions);
            var newJson = JsonSerializer.SerializeToNode(newTodo.Extensions);

            // Merge: copy non-null values from newJson to existingJson
            foreach (var property in newJson.AsObject())
            {
                if (property.Value != null && !IsDefaultJsonValue(property.Value))
                {
                    existingJson[property.Key] = property.Value.DeepClone();
                }
            }

            // Deserialize back to ToDoExtension
            existing.Extensions = existingJson.Deserialize<ToDoExtension>();
        }

        _context.SaveChanges();
        return new OkObjectResult(existing);
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var todo = await _context.ToDo.FindAsync(id);
        if (todo == null) return NotFound();
        _context.ToDo.Remove(todo);
        _context.SaveChanges();
        return new OkObjectResult(todo);
    }

    [HttpDelete]
    public void Delete()
    {
        _context.ToDo.RemoveRange(_context.ToDo);
        _context.SaveChanges();
    }     

    private static bool IsDefaultJsonValue(JsonNode value)
    {
        return value switch
        {
            JsonValue jsonValue when jsonValue.TryGetValue<bool>(out var boolValue) => boolValue == false,
            JsonValue jsonValue when jsonValue.TryGetValue<string>(out var str) => string.IsNullOrEmpty(str),
            JsonValue jsonValue when jsonValue.TryGetValue<int>(out var num) => num == 0,
            JsonValue jsonValue when jsonValue.TryGetValue<decimal>(out var dec) => dec == 0,
            JsonArray arr => arr.Count == 0,
            _ => false
        };
    }  
}
