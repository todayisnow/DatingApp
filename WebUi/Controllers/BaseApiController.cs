using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebUi.Helpers;

namespace WebUi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    [ServiceFilter(typeof(LogUserActivity))]
    public class BaseApiController : ControllerBase
    {
       
    }
}
