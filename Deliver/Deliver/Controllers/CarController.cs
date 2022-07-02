using Deliver.CustomAttribute;
using Microsoft.AspNetCore.Mvc;

namespace Deliver.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CarController : ControllerBase
    {
    }
}