using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Api.Sample.Controllers
{
    [Route("api/[controller]")]
    public class IdentiryController : Controller
    {
        [Authorize]
        [HttpGet]
        public IEnumerable<string[]> Get()
        {
            return User.Claims.Select(c => new string[] { c.Type, c.Value });
        }
    }
}