using ASM.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

namespace ASM.Filters
{
    public class AuthenticationFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // The action filter logic.
            Controller controller = filterContext.Controller as Controller;
            var session = filterContext.HttpContext.Session;
            string userName = filterContext.HttpContext.Session.GetString(SessionKey.NguoiDung.Username);
            var sessionStatus = ((userName != null && userName != "") ? true : false);
            if (controller != null)
            {
                if (session == null || !sessionStatus)
                {
                    filterContext.Result =
                           new RedirectToRouteResult(
                               new RouteValueDictionary{
                                   { "controller", "Admin" },
                                   { "action", "Login" }}
                               );

                }
            }
            base.OnActionExecuting(filterContext);
        }
    }
}
