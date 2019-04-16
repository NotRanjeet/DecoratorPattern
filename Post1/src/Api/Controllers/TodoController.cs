using Core.AppServices;
using Core.Dtos;
using Logic.Todo;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/todo")]
    public class TodoController: BaseController
    {
        private readonly Messages message;

        public TodoController(Messages message)
        {
            this.message = message;
        }


        [HttpGet]
        public IActionResult GetList()
        {
            var list = message.Dispatch(new GetTodoListQuery());
            return Ok(list);
        }


        [HttpPut]
        public IActionResult ToggleTaskItem([FromBody]ToggleItemDto toggle)
        {
            var result = message.Dispatch(new ToggleItemCommand(toggle.Id, toggle.IsDone));
            return FromResult(result);
        }
    }
}
