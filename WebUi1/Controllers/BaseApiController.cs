using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebUi1.Helpers;

namespace WebUi1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    [ServiceFilter(typeof(LogUserActivity))]
    public class BaseApiController : ControllerBase
    {
       
    }
}
