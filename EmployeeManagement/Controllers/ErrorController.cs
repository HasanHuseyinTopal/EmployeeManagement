using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers
{
    [AllowAnonymous]
    public class ErrorController : Controller
    {
        [Route("Error/{statusCode}")]
        public IActionResult StatusCodePagesWithReExecute(int statusCode)
        {
            var result = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            return View("_NotFound",statusCode);
        }
        [Route("Error")]
        public IActionResult ExceptionHandler()
        {
            var ExceptionDetails = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var result = ExceptionDetails.Error.InnerException;
            return View("_Error", ExceptionDetails.Error.Message);
        }
    }
}
