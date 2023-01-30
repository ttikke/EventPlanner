using Microsoft.AspNetCore.Mvc;

namespace Nullam.Controllers;

public class ErrorsController : ControllerBase
{
    [Route("/error")]
    protected IActionResult Error()
    {
        return Problem();
    }
}