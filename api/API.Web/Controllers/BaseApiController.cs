using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Web.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public abstract class BaseApiController : ControllerBase
{
}
