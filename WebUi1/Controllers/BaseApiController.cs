namespace WebUi1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    [ServiceFilter(typeof(LogUserActivity))]
    public class BaseApiController : ControllerBase
    {

    }
}
