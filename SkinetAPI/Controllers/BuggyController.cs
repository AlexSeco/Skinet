using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using SkinetAPI.Errors;

namespace SkinetAPI.Controllers;

public class BuggyController : BaseController
{
    private readonly Context _context;

    public BuggyController(Context context)
    {
        _context = context;
    }

    [HttpGet("notfound")]
    public ActionResult GetNotFoundRequest()
    {
        return NotFound(new APIResponse(404));
    }

    [HttpGet("servererror")]
    public ActionResult GetServerError()
    {
        var thing = _context.Products.Find(45);

        var thingstoreturn = thing.ToString();

        return Ok(thingstoreturn);
    }

    [HttpGet("badrequest")]
    public ActionResult GetBadRequest()
    {
        return BadRequest(new APIResponse(400));
    }

    [HttpGet("badrequest/{id}")]
    public ActionResult GetNotFoundRequest(int id)
    {
        return Ok();
    }
}
