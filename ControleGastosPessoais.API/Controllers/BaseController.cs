using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ControleGastosPessoais.API.Controllers;

[ApiController]
public abstract class BaseController : ControllerBase
{
    protected string UserId => User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
}
