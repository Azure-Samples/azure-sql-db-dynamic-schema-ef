using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;
using System.Linq;

namespace Azure.SQLDB.Samples.DynamicSchema.Filters
{
    public class ToDoUrlGeneratorFilter : IResultFilter
    {
        public void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.Result is OkObjectResult okResult)
            {
                var request = context.HttpContext.Request;
                var baseUrl = $"{request.Scheme}://{request.Host}/todo/hybrid";

                // Handle single ToDo object
                if (okResult.Value is ToDo todo)
                {
                    if (todo.Id > 0)
                        todo.Url = $"{baseUrl}/{todo.Id}";
                }
                // Handle collection of ToDo objects
                else if (okResult.Value is IEnumerable<ToDo> todos)
                {
                    foreach (var item in todos.Where(t => t.Id > 0))
                    {
                        item.Url = $"{baseUrl}/{item.Id}";
                    }
                }
            }
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
            // Nothing to do after execution
        }
    }
}
