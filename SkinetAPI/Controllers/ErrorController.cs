using Microsoft.AspNetCore.Mvc;
using SkinetAPI.Errors;

namespace SkinetAPI.Controllers;

[Route("errors/{code}")]
[ApiExplorerSettings(IgnoreApi = true)]
public class ErrorController : BaseController
{
    public IActionResult Error(int code)
    {
        return new ObjectResult(new APIResponse(code));
    }
}
