using System.Data;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Azure.SQLDB.Samples.DynamicSchema;

[ApiController]
[Route("todo/hybrid")]
public class ToDoHybridController(IConfiguration config, ILogger<ToDoHybridController> logger, ToDoContext context) : ControllerBase
{
    private readonly ILogger<ToDoHybridController> _logger = logger;
    private readonly IConfiguration _config = config;
    private readonly ToDoContext _context = context;
    
    [HttpGet]
    public async Task<IEnumerable<ToDo>> Get()
    {
        var todos = await _context.ToDo.Take(10).ToListAsync();
        var todoList = todos.Select(t => {
            t.Url = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host + $"/{RouteData.Values["controller"]}/{t.Id}";
            return t;
        }).ToList();
        return todoList;
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<ToDo> Get(int id)
    {
        var todo = await _context.ToDo.FindAsync(id);       
        todo.Url = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host + $"/{RouteData.Values["controller"]}/{todo.Id}";
        return todo;
    }

    [HttpPost]        
    public async Task<ToDo> Post([FromBody]ToDo todo)
    {
        await _context.ToDo.AddAsync(todo);
        await _context.SaveChangesAsync();
                    
        return todo;
    }

    [HttpPatch]     
    [Route("{id}")]   
    public async Task<ToDo> Patch(int id, [FromBody]ToDo todo)
    {
        var existing = await _context.ToDo.FindAsync(id);
        existing.Title = todo.Title;
        existing.Completed = todo.Completed;
        existing.Extensions = todo.Extensions;
        _context.SaveChanges();
        existing.Url = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host + $"/{RouteData.Values["controller"]}/{existing.Id}";
        return existing;
    }
    
    [HttpDelete]     
    [Route("{id}")]   
    public async Task<ToDo> Delete(int id)
    {            
        var todo = await _context.ToDo.FindAsync(id);     
        _context.ToDo.Remove(todo);            
        _context.SaveChanges();
        todo.Url = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host + $"/{RouteData.Values["controller"]}/{todo.Id}";
        return todo;
    }     

    [HttpDelete]         
    public void Delete()
    {                    
        _context.ToDo.RemoveRange(_context.ToDo);       
        _context.SaveChanges(); 
    }        
}
