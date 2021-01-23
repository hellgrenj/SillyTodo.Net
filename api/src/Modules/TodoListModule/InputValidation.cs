using System;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace api.Application.Modules.TodoListModule
{

    public class InputValidation : ActionFilterAttribute
    {
        public object JsonConvert { get; private set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            if (!filterContext.ModelState.IsValid)
            {
                if (filterContext.HttpContext.Request.Method == "GET")
                {
                    var result = new BadRequestResult();
                    filterContext.Result = result;
                }
                else
                {
                    var result = new ContentResult();
                    string content = JsonSerializer.Serialize(filterContext.ModelState);
                    result.Content = content;
                    result.ContentType = "application/json";

                    filterContext.HttpContext.Response.StatusCode = 400;
                    filterContext.Result = result;
                }
            }
        }

    }
}