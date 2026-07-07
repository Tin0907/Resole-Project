using ASM.Filters;
using ASM.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace ASM.Controllers
{
    [AuthenticationFilter]
    public abstract class BaseController : Controller
    {
        public BaseController()
        {
        }

        protected string GetUserName()
        {
            return HttpContext.Session.GetString(SessionKey.NguoiDung.Username);
        }
        protected string GetFullName()
        {
            return HttpContext.Session.GetString(SessionKey.NguoiDung.FullName);
        }

        protected string GetKHEmail()
        {
            return HttpContext.Session.GetString(SessionKey.KhachHang.KH_Email);
        }
    }
}
