using Core.AppServices;
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
    }
}
