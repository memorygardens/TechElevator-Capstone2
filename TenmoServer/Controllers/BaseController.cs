using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TenmoServer.Controllers
{
    [Route("")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        [HttpGet]
        public ActionResult HeyThere()
        {
            return Ok(new { Message = "Hey there, whoever is seeing this page. We tried really hard on this project! And it still sucks, so have pity and give us a 3. Please" });
        }
    }
}
