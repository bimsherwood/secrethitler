using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SecretHitlerWebsite;

public class MissingCookieExceptionFilter : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext filterContext)
    {
        if (filterContext.Exception is MissingCookieException) {
            filterContext.Result = new RedirectToActionResult("InvalidSession", "Home", new Object{});
            filterContext.ExceptionHandled = true;
        }
    }
}